using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows;

namespace SHCAIDA
{
    public struct Vector2
    {
        public double leftBorder;
        public double rightBorder;

        public Vector2(double leftBorder, double rightBorder)
        {
            this.leftBorder = leftBorder;
            this.rightBorder = rightBorder;
        }
    }

    public enum GameRole
    {
        Regulator = 0,
        StateSensor,
        OutputSensor
    }

    public struct DataUnit
    {
        public DateTime time;
        public List<DeviceParameters> devices;
        public List<double> values;

        public DataUnit(DateTime time, List<DeviceParameters> devices, List<double> values)
        {
            this.devices = new List<DeviceParameters>();
            this.values = new List<double>();
            this.time = time;
            this.devices = devices ?? throw new ArgumentNullException(nameof(devices));
            this.values = values ?? throw new ArgumentNullException(nameof(values));
        }
    }

    public struct TableUnit
    {
        public List<DeviceParameters> devices;
        public List<Vector2> intervals;

        public TableUnit(List<DeviceParameters> devices, List<Vector2> intervals)
        {
            this.intervals = new List<Vector2>();
            this.devices = devices ?? throw new ArgumentNullException(nameof(devices));
            this.intervals = intervals ?? throw new ArgumentNullException(nameof(intervals));
        }
    }

    [Serializable]
    public class GameNode
    {
        public List<RoledDevice> usedSensors;
        private List<DataUnit> dataUnits;

        public string nodeName;
        public string nodeDescription;

        public int regulatorIntervalCount;
        public int stateSensorIntervalCount;

        private int RegulatorsCount => usedSensors.FindAll(x => x.role == GameRole.Regulator).Count;

        private int StatesCount => usedSensors.FindAll(x => x.role == GameRole.StateSensor).Count;

        private bool CheckOutputsCount()
        {
            if (usedSensors.FindAll(x => x.role == GameRole.OutputSensor).Count == 1)
                return true;
            else throw new ArgumentOutOfRangeException();
        }

        public GameNode(string nodeName, string nodeDescription, int regulatorIntervalCount, int stateSensorIntervalCount)
        {
            usedSensors = new List<RoledDevice>();
            this.nodeName = nodeName ?? throw new ArgumentNullException(nameof(nodeName));
            this.nodeDescription = nodeDescription ?? throw new ArgumentNullException(nameof(nodeDescription));
            this.regulatorIntervalCount = regulatorIntervalCount;
            this.stateSensorIntervalCount = stateSensorIntervalCount;

        }

        public string NodeStats
        {
            get
            {
                var res = "Название: " + nodeName + "\nКоличество интервалов регуляторов: " + regulatorIntervalCount + "\nРегуляторы, которые входят в узел:\n";
                foreach (var sensor in usedSensors.Where(sensor => sensor.role == GameRole.Regulator).Select(sensor => sensor))
                    res += (ProgramMainframe.GetSensorNameById(sensor.deviceID, sensor.deviceType) + "\n");
                res += "Количество интервалов датчиков состояний: " + stateSensorIntervalCount + "\nДатчики состояний:\n";
                foreach (var sensor in usedSensors.Where(sensor => sensor.role == GameRole.StateSensor).Select(sensor => sensor))
                    res += (ProgramMainframe.GetSensorNameById(sensor.deviceID, sensor.deviceType) + "\n");
                res += "Оцениваемые датчики:\n";
                foreach (var sensor in usedSensors.Where(sensor => sensor.role == GameRole.OutputSensor).Select(sensor => sensor))
                    res += (ProgramMainframe.GetSensorNameById(sensor.deviceID, sensor.deviceType) + "\n");
                return res;
            }
        }

        public void FillData(DateTime begin, DateTime end)
        {
            foreach (var device in usedSensors)
            {
                SQLiteParameter left = new SQLiteParameter("@leftData", begin.ToOADate());
                SQLiteParameter right = new SQLiteParameter("@rightData", end.ToOADate());
                SQLiteParameter deviceID = new SQLiteParameter("@device", device.deviceID);
                SQLiteParameter[] a = new SQLiteParameter[4];
                a[0] = left;
                a[1] = right;
                a[2] = deviceID;
                device.values = new List<BasicValue>();
                switch (device.deviceType)
                {
                    case TypeOfDataSources.Siemens:
                        {
                            SQLiteParameter clientID = new SQLiteParameter("@client", ProgramMainframe.Ssconnections.Find(x => x.Sensor.ID == device.deviceID).Client.ID);
                            a[3] = clientID;
                            var result = ProgramMainframe.Valuesdb.Values.SqlQuery("SELECT *  FROM Values WHERE DeviceID=@device AND Date >= @leftData AND Date <= @rightData AND ClientID = @client", a).ToList();
                            foreach (var res in result)
                                device.values.Add(new BasicValue(res.DeviceValue, (res.Date)));
                        }
                        break;
                    case TypeOfDataSources.Mssql:
                        {
                            SQLiteParameter clientID = new SQLiteParameter("@client", ProgramMainframe.Mssqlconnections.Find(x => x.Sensor.ID == device.deviceID).Client.ID);
                            a[3] = clientID;
                            var result = ProgramMainframe.Valuesdb.Values.SqlQuery("SELECT *  FROM Values WHERE DeviceID=@device AND Date >= @leftData AND Date <= @rightData AND ClientID = @client", a).ToList();
                            foreach (var res in result)
                                device.values.Add(new BasicValue(res.DeviceValue, (res.Date)));
                        }
                        break;
                    default:
                        throw new ArgumentException();
                }
            }

            dataUnits = new List<DataUnit>();
            var output = usedSensors.Find(x => x.role == GameRole.OutputSensor);

            foreach (var value in output.values)
            {
                DateTime someDate = (value.Date);
                DataUnit someData = new DataUnit();
                someData.devices.Add(output);
                someData.values.Add(output.values.Find(x => x.Date == someDate).DeviceValue);
                foreach (var device in usedSensors)
                {
                    var a = device.values.Find(x => (x.Date) == someDate);
                    if (a == null)
                        break;
                    else
                    {
                        someData.devices.Add(device);
                        someData.values.Add(a.DeviceValue);
                    }
                }
                dataUnits.Add(someData);
            }
            if (dataUnits.Count == 0)
                MessageBox.Show("Поиск пригодных к анализу данных по выбраным устройствам не дал результатов");
        }

        public Tuple<List<TableUnit>, List<TableUnit>, double[,]> PlayGame()
        {
            if (DataConsistent)
            {
                DeviceParameters[,] regulatorsCombos = new DeviceParameters[RegulatorsCount, regulatorIntervalCount];
                DeviceParameters[,] stateSensorsCombos = new DeviceParameters[StatesCount, stateSensorIntervalCount];
                List<TableUnit> regRows = new List<TableUnit>();//комбинации этих самих интервалов
                List<TableUnit> stateColumns = new List<TableUnit>();

                foreach (var device in usedSensors)
                {
                    int deviceIndex = usedSensors.Where(x => x.role == device.role).ToList().IndexOf(device);
                    switch (device.role)
                    {
                        case GameRole.Regulator:
                            {
                                device.SetupIntervals(regulatorIntervalCount);
                                for (var i = 0; i < regulatorIntervalCount; i++)
                                    regulatorsCombos[deviceIndex, i] = new DeviceParameters(device.deviceID, device.role, device.deviceType, device.intervals[i]);
                            }
                            break;
                        case GameRole.StateSensor:
                            {
                                device.SetupIntervals(stateSensorIntervalCount);
                                for (var i = 0; i < stateSensorIntervalCount; i++)
                                    stateSensorsCombos[deviceIndex, i] = new DeviceParameters(device.deviceID, device.role, device.deviceType, device.intervals[i]);
                            }
                            break;
                        default:
                            break;
                    }
                }

                regRows = ConvertToTable(regulatorsCombos, RegulatorsCount, regulatorIntervalCount);
                stateColumns = ConvertToTable(stateSensorsCombos, StatesCount, stateSensorIntervalCount);

                List<double>[,] outputTableValues = new List<double>[regRows.Count, stateColumns.Count];
                for (var i = 0; i < regRows.Count; i++)
                    for (var j = 0; j < stateColumns.Count; j++)
                        outputTableValues[i, j] = new List<double>();

                foreach (var unit in dataUnits)//для каждого unit находим соответствие в таблице значений
                {
                    int regRowIndex = -1;
                    int stateColumnIndex = -1;
                    foreach (var combo in regRows)
                    {
                        bool check = true;
                        foreach (var device in unit.devices)
                        {
                            var comparableToUnitDeviceIndexInCombos = combo.devices.FindIndex(x => x == device);
                            var comparableToUnitDeviceIndexInUnit = unit.devices.FindIndex(x => x == device);
                            if (!(unit.values[comparableToUnitDeviceIndexInUnit] <= combo.intervals[comparableToUnitDeviceIndexInCombos].rightBorder &&
                                unit.values[comparableToUnitDeviceIndexInUnit] >= combo.intervals[comparableToUnitDeviceIndexInCombos].leftBorder))
                                check = false;
                        }
                        if (check == true)
                            regRowIndex = regRows.IndexOf(combo);
                    }
                    foreach (var combo in stateColumns)
                    {
                        bool check = true;
                        foreach (var device in unit.devices)
                        {
                            var comparableToUnitDeviceIndexInCombos = combo.devices.FindIndex(x => x == device);
                            var comparableToUnitDeviceIndexInUnit = unit.devices.FindIndex(x => x == device);
                            if (!(unit.values[comparableToUnitDeviceIndexInUnit] <= combo.intervals[comparableToUnitDeviceIndexInCombos].rightBorder &&
                                unit.values[comparableToUnitDeviceIndexInUnit] >= combo.intervals[comparableToUnitDeviceIndexInCombos].leftBorder))
                                check = false;
                        }
                        if (check == true)
                            stateColumnIndex = regRows.IndexOf(combo);
                    }
                    if (regRowIndex == -1 || stateColumnIndex == -1) // проверка на то, что система отработала норм и значения попали в интервал
                        throw new ApplicationException();
                    else
                        outputTableValues[regRowIndex, stateColumnIndex].Add(unit.values[unit.devices.FindIndex(x => x.role == GameRole.OutputSensor)]);
                }

                //процедура очистки таблицы
                //выбираем те строки и столбцы, в которых есть значения
                bool[] appropriateRows = new bool[regRows.Count];
                bool[] appropriateColumns = new bool[stateColumns.Count];
                int appropriateRowsCount = 0;
                int appropriateColumnsCount = 0;

                for (var i = 0; i < regRows.Count; i++)
                {
                    bool check = false;
                    for (var j = 0; j < stateColumns.Count; j++)
                        if (outputTableValues[i, j].Count != 0)
                        {
                            check = true;
                            break;
                        }
                    appropriateRows[i] = check;
                    appropriateRowsCount++;
                }

                for (var j = 0; j < stateColumns.Count; j++)
                {
                    bool check = false;
                    for (var i = 0; i < regRows.Count; i++)
                        if (outputTableValues[i, j].Count != 0)
                        {
                            check = true;
                            break;
                        }
                    appropriateColumns[j] = check;
                    appropriateColumnsCount++;
                }

                double[,] finaleMatrix = new double[appropriateRowsCount, appropriateColumnsCount];

                int i1;
                int j1 = 0;
                for (var i = 0; i < regRows.Count; i++)
                {
                    i1 = 0;
                    if (appropriateRows[i])
                    {
                        for (var j = 0; j < stateColumns.Count; j++)
                        {
                            if (appropriateColumns[j])
                            {
                                finaleMatrix[i1, j1] = MidValInList(outputTableValues[i, j]);
                                j1++;
                            }
                            else
                            {
                                stateColumns.RemoveAt(j);
                                j--;
                            }
                        }
                        i1++;
                    }
                    else
                    {
                        regRows.RemoveAt(i);
                        i--;
                    }
                }
                return Tuple.Create(regRows, stateColumns, finaleMatrix);
            }
            else throw new ApplicationException();
        }

        private double MidValInList(List<double> list)
        {
            double sum = 0;
            foreach (var value in list)
                sum += value;
            return sum / list.Count;
        }

        private List<TableUnit> ConvertToTable(DeviceParameters[,] devices, int deviceCount, int intervalsCount) // сразу составляем полный перечень возможных комбинаций
        {
            List<TableUnit> result = new List<TableUnit>();
            int[] indexes = new int[deviceCount];
            List<DeviceParameters> parameters = new List<DeviceParameters>();
            for (var i = 0; i < deviceCount; i++)
            {
                parameters.Add(devices[i, 0]);
                indexes[i] = 0;
            }
            for (var i = 0; i < devices.Length; i++)
            {
                List<Vector2> ints = new List<Vector2>();
                for (var j = 0; j < deviceCount; j++)
                    ints.Add(devices[j, indexes[j]].interval);
                result.Add(new TableUnit(parameters, ints));
                UpdateIndexes(ref indexes, deviceCount - 1, intervalsCount);
            }
            return result;
        }

        private void UpdateIndexes(ref int[] indexes, int pos, int margin) // рекурсивный метод, должен перебирать комбинации индексов по двумерному масииву по столбцам
        {
            indexes[pos]++;
            if (indexes[pos] >= margin)
            {
                indexes[pos] = 0;
                if (pos - 1 >= 0)
                    UpdateIndexes(ref indexes, pos - 1, margin);
                else return;
            }
            else return;
        }

        public bool DataConsistent
        {
            get
            {
                if (usedSensors.Count == 0)
                    return false;
                int valuesCount = usedSensors[0].values.Count;
                foreach (var device in usedSensors)
                    if (device.values.Count != valuesCount)
                        return false;
                if (dataUnits.Count == 0)
                    return false;
                return true;
            }
        }
    }
}