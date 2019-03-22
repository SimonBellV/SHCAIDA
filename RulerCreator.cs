using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Accord.Fuzzy;

namespace SHCAIDA
{
    public struct LingVariable
    {
        public string name;
        public List<string> labels;

        public LingVariable(string name)
        {
            this.name = name;
            labels = new List<string>();
        }
    }
    static class RulerCreator
    {
        public static List<LingVariable> variableTree;
        //public static bool enableStatusVariable;
        //public static List<FuzzySet> fuzzySets;
        public static List<LinguisticVariable> fuzzySensors;
        //public static List<string> fuzzySetsSensorNames;
        //public static List<RockwellSensor> rockwellSensors;

        public static void InitRuleCreation()
        {
            variableTree = new List<LingVariable>();
            //enableStatusVariable = false;
            //fuzzySets = new List<FuzzySet>();
            //fuzzySetsSensorNames = new List<string>();
            fuzzySensors = new List<LinguisticVariable>();
        }

        private static int FSContainsVar(string name)
        {
            for(var i = 0; i < fuzzySensors.Count; i++)
                if (fuzzySensors[i].Name == name)
                    return i;
            return -1;
        }

        public static List<string> GetLabels(string lingVariable)
        {
            var labels = new List<string>();
            foreach (var val in variableTree)
                if (val.name == lingVariable)
                {
                    labels.AddRange(val.labels);
                    break;
                }
            return labels;
        }
        public static void AddLingVariable(string name, float start, float end)
        {
            if (variableTree.FindIndex(x => x.name == name) == -1)
            {
                fuzzySensors.Add(new LinguisticVariable(name, start, end));
                variableTree.Add(new LingVariable(name));
            }
        }
        public static void AddLabel(string varName, FuzzySet label)
        {
            int pos = FSContainsVar(varName);
            if (pos != -1 && fuzzySensors[pos].Start <= label.LeftLimit && fuzzySensors[pos].End >= label.RightLimit)
            {
                fuzzySensors[pos].AddLabel(label);
                variableTree[pos].labels.Add(label.Name);
            }
            else
                MessageBox.Show("Произошла ошибка при добавлении статуса переменной");
        }
    }
}
