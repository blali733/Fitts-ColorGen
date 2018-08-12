using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabGen.Helpers;

namespace LabGen
{
    class Controller
    {
        static void Main(string[] args)
        {
            Color color = new Color(191, 191, 191);
            double diff = 0.0001;
            double step = 0.005;
            double jnd = 10;
            Model model = new Model(color, diff, step, jnd);
            model.GenerateSteps();
            model.FindColours();
            model.WriteResults();
        }
    }
}
