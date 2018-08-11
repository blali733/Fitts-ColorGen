using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabGen
{
    class Controller
    {
        static void Main(string[] args)
        {
            Color color = new Color(191, 191, 191);
            double diff = 0.001;
            double step = 0.01;
            Model model = new Model(color, diff, step);
            model.GenerateSteps();
            model.FindColours();
        }
    }
}
