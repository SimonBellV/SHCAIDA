using Accord.Fuzzy;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SHCAIDA
{
    public class LingVariable
    {
        public string name;
        public float start;
        public float end;
        public string sourceType;
        public string source;
        public List<Status> labels;

        public LingVariable(string sourceType, string source, string name, float start, float end)
        {
            this.sourceType = sourceType;
            this.source = source;
            this.name = name;
            labels = new List<Status>();
            this.start = start;
            this.end = end;
        }

        public Status GetLabel(string label) => labels.Find(x => x.name == label);

        public Accord.Fuzzy.LinguisticVariable ConvertToLinguisticVariable
        {
            get
            {
                Accord.Fuzzy.LinguisticVariable t = new Accord.Fuzzy.LinguisticVariable(name, start, end);
                foreach (var label in labels)
                    t.AddLabel(label.GetGetFuzzy());
                return t;
            }
        }

        public bool IsLoggingActive(string status) => labels.Find(x => x.name == status).isLogging;

        public void UpdateLogging(string status, bool newWay) => labels.Find(x => x.name == status).isLogging = newWay;
    }

    public class Status
    {
        public readonly bool type; //false - trapezoidal, true - z-type
        public string name;
        public float V1;
        public float V2;
        public float V3;
        public float V4;
        public bool isLogging;

        public string String => name + " - " + V1 + " ," + V2 + " ," + V3 + " ," + V4;

        public FuzzySet GetGetFuzzy()
        {
            if (!type)
                return new FuzzySet(name, new TrapezoidalFunction(V1, V2, V3, V4));
            else if (V3 == float.MinValue)
                return new FuzzySet(name, new TrapezoidalFunction(V1, V2, TrapezoidalFunction.EdgeType.Left));
            else
                return new FuzzySet(name, new TrapezoidalFunction(V1, V2, TrapezoidalFunction.EdgeType.Right));
        }

        public Status(string name, string V1, string V2, string V3, string V4)
        {
            isLogging = false;
            this.name = name;
            try
            {
                this.V1 = Convert.ToSingle(V1);
                this.V2 = Convert.ToSingle(V2);
                if (V3.ToLower() == "left" || V3.ToLower() == "лево")
                {
                    this.V3 = float.MinValue;
                    this.V4 = 0;
                    type = true;
                }
                else if (V3.ToLower() == "right" || V3.ToLower() == "право")
                {
                    this.V3 = float.MaxValue;
                    this.V4 = 0;
                    type = true;
                }
                else
                {
                    this.V3 = Convert.ToSingle(V3);
                    this.V4 = Convert.ToSingle(V4);
                    type = false;
                }                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                this.V1 = 0;
                this.V2 = 0;
                this.V3 = 0;
                this.V4 = 0;
                type = false;
            }
        }

        public Status()
        {
            name = "";
            V1 = 0;
            V2 = 0;
            V3 = 0;
            V4 = 0;
            isLogging = false;
        }
    }
}