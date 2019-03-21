using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Fuzzy;

namespace SHCAIDA
{
    static class RulerCreator
    {
        public static bool enableStatusVariable;
        public static List<FuzzySet> fuzzySets;
        public static List<LinguisticVariable> fuzzySensors;
        //public static List<string> fuzzySetsSensorNames;
        //public static List<RockwellSensor> rockwellSensors;

        public static void InitRuleCreation()
        {
            enableStatusVariable = false;
            fuzzySets = new List<FuzzySet>();
            //fuzzySetsSensorNames = new List<string>();
            fuzzySensors = new List<LinguisticVariable>();
        }

        public static int FSContainsVar(string name)
        {
            for(var i = 0; i < fuzzySensors.Count; i++)
                if (fuzzySensors[i].Name == name)
                    return i;
            return -1;
        }
        //public static void AddStatusToVar(FuzzySet status, )
    }
}
