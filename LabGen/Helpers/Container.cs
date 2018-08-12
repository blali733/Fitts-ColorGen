using System.Collections.Generic;
using System.Security.Policy;

namespace LabGen.Helpers
{
    public class Container
    {
        public string Name;
        public List<double> Labels;
        public List<List<List<double>>> Payload;

        public Container()
        {
            Name = "";
            Labels = new List<double>();
            Payload = new List<List<List<double>>>();
        }
    }
}