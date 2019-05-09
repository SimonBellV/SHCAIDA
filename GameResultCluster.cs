using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCAIDA
{
    public struct DeviceInterval
    {
        public RoledSiemensSensor device;
        public double leftBorder;
        public double rightBorder;

        public DeviceInterval(RoledSiemensSensor device, double leftBorder, double rightBorder)
        {
            this.device = device;
            this.leftBorder = leftBorder;
            this.rightBorder = rightBorder;
        }
    }
    public class GameResultCluster
    {
        
    }
}
