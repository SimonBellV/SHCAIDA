﻿using System;
using System.Collections.Generic;
using System.Windows;

namespace SHCAIDA
{
    [Serializable]
    public class DeviceParameters
    {
        public int deviceID;
        public GameRole role;
        public TypeOfDataSources deviceType;
        public Vector2 interval;

        public DeviceParameters(int deviceID, GameRole role, TypeOfDataSources type, Vector2 interval)
        {
            this.deviceID = deviceID;
            this.role = role;
            deviceType = type;
            this.interval = interval;
        }

        public DeviceParameters(int deviceID, GameRole role, TypeOfDataSources type)
        {
            this.deviceID = deviceID;
            this.role = role;
            deviceType = type;
        }
    }
    [Serializable]
    public class RoledDevice : DeviceParameters
    {
        public List<BasicValue> values;
        public List<Vector2> intervals;

        public RoledDevice(int deviceID, GameRole role, TypeOfDataSources type) : base(deviceID, role, type)
        {
            values = new List<BasicValue>();
        }

        public string SensorName => this.deviceType == TypeOfDataSources.Siemens ? ProgramMainframe.GetSensorNameById(deviceID, deviceType) : throw new ArgumentException();

        public void SetupIntervals(int intervalsCount)
        {
            intervals = new List<Vector2>();
            if (values.Count < 2 || intervalsCount < 2)
                MessageBox.Show("Ошибка данных");
            else
            {
                double minValue = values[0].DeviceValue;
                double maxValue = values[0].DeviceValue;
                for (var i = 1; i < values.Count; i++)
                {
                    if (values[i].DeviceValue > maxValue)
                        maxValue = values[i].DeviceValue;
                    if (values[i].DeviceValue < minValue)
                        minValue = values[i].DeviceValue;
                }
                double step = (maxValue - minValue) / intervalsCount;
                for (int i = 0; i < intervalsCount; i++)
                {
                    intervals.Add(new Vector2(minValue, minValue + step));
                    minValue += step;
                }
            }
        }
    }
}