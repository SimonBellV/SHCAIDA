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
                    t.AddLabel(label.GetFuzzy);
                return t;
            }
        }
    }

    public class Status
    {
        public string name;
        public float V1;
        public float V2;
        public float V3;
        public float V4;

        public string String => name + " - " + V1 + " ," + V2 + " ," + V3 + " ," + V4;

        public Accord.Fuzzy.FuzzySet GetFuzzy => new Accord.Fuzzy.FuzzySet(name, new Accord.Fuzzy.TrapezoidalFunction(V1, V2, V3, V4));

        public Status(string name, string V1, string V2, string V3, string V4)
        {
            this.name = name;
            try
            {
                this.V1 = Convert.ToSingle(V1);
                this.V2 = Convert.ToSingle(V2);
                this.V3 = Convert.ToSingle(V3);
                this.V4 = Convert.ToSingle(V4);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                this.V1 = 0;
                this.V2 = 0;
                this.V3 = 0;
                this.V4 = 0;
            }
        }

        public Status()
        {
            name = "";
            V1 = 0;
            V2 = 0;
            V3 = 0;
            V4 = 0;
        }
    }
}