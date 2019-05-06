using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCAIDA
{
    public enum GameRole
    {
        Regulator = 0,
        StateSensor,
        OutputSensor
    }

    [Serializable]
    public struct RoledSiemensSensor
    {
        public int deviceID;
        public GameRole role;

        public RoledSiemensSensor(int deviceID, GameRole role)
        {
            this.deviceID = deviceID;
            this.role = role;
        }

        public string SensorName(int id) => ProgramMainframe.GetSensorNameById(id);
    }

    public struct ValueHeap
    {
        public RoledSiemensSensor device;
        public double value;

        public ValueHeap(RoledSiemensSensor device, double value)
        {
            this.device = device;
            this.value = value;
        }
    }

    [Serializable]
    public class GameNode
    {
        public List<RoledSiemensSensor> usedSensors;
        public string nodeName;
        public string nodeDescription;
        public int regulatorIntervalCount;
        public int stateSensorIntervalCount;
        public List<DeviceInterval>[] outputColumns;
        public List<DeviceInterval>[] regulatorRows;
        public List<double>[][] outputValues;
        public List<ValueHeap> bareValues;

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
            usedSensors = new List<RoledSiemensSensor>();
            this.nodeName = nodeName ?? throw new ArgumentNullException(nameof(nodeName));
            this.nodeDescription = nodeDescription ?? throw new ArgumentNullException(nameof(nodeDescription));
            this.regulatorIntervalCount = regulatorIntervalCount;
            this.stateSensorIntervalCount = stateSensorIntervalCount;
        }

        public string NodeStats()
        {
            string res = "Название: " + nodeName + "\nКоличество интервалов регуляторов: " + regulatorIntervalCount + "\nРегуляторы, которые входят в узел:\n";
            foreach (var sensor in usedSensors.Where(sensor => sensor.role == GameRole.Regulator).Select(sensor => sensor))
            {
                res += (ProgramMainframe.GetSensorNameById(sensor.deviceID) + "\n");
            }
            res += "Количество интервалов датчиков состояний: " + stateSensorIntervalCount + "\nДатчики состояний:\n";
            foreach (var sensor in usedSensors.Where(sensor => sensor.role == GameRole.StateSensor).Select(sensor => sensor))
            {
                res += (ProgramMainframe.GetSensorNameById(sensor.deviceID) + "\n");
            }
            res += "Оцениваемые датчики:\n";
            foreach (var sensor in usedSensors.Where(sensor => sensor.role == GameRole.OutputSensor).Select(sensor => sensor))
            {
                res += (ProgramMainframe.GetSensorNameById(sensor.deviceID) + "\n");
            }
            return res;
        }

        public void BuildGameNode()
        {

        }

        public void GetNodeDevicesValues(List<ValueHeap> values)
        {
            foreach (var value in values)
            {

            }
        }
    }
}
