using System;

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
}