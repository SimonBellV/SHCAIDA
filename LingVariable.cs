using System.Collections.Generic;

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

        public Accord.Fuzzy.LinguisticVariable ConvertToLinguisticVariable
        {
            get
            {
                Accord.Fuzzy.LinguisticVariable t = new Accord.Fuzzy.LinguisticVariable(name, start, end);
                foreach (var label in labels)
                    t.AddLabel(label.GetFuzzy());
                return t;
            }
        }

        public bool IsLoggingActive(string status) => labels.Find(x => x.name == status).isLogging;

        public void UpdateLogging(string status, bool newWay) => labels.Find(x => x.name == status).isLogging = newWay;
    }
}