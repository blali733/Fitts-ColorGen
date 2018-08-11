using System.Collections.Generic;

namespace LabGen
{
    public class Container
    {
        public List<float> Labels;
        public List<List<List<float>>> Payload;

        public Container()
        {
            Labels = new List<float>();
            Payload = new List<List<List<float>>>();
        }
    }
}