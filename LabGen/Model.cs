using System;
using System.Collections.Generic;
using Colourful;
using Colourful.Conversion;

namespace LabGen
{
    public class Model
    {
        private Container _generatedDataContainer;
        private readonly LabColor _skyboxColor;
        private double _diff;
        private double _step;

        public Model(Color color, double diff, double step)
        {
            _generatedDataContainer = new Container();
            _skyboxColor = Color2Lab(color);
            _diff = diff;
            _step = step;
        }

        private double ColorDiffBg(Color color)
        {
            LabColor temp = Color2Lab(color);
            return LabDiff(temp, _skyboxColor);
        }
        private static Color Lab2Color(LabColor color)
        {
            Color output = new Color();
            var converter = new ColourfulConverter { WhitePoint = Illuminants.D50, TargetRGBWorkingSpace = RGBWorkingSpaces.sRGB };
            RGBColor rgbColor = converter.ToRGB(color);
            output.r = (float)rgbColor.R;
            output.g = (float)rgbColor.G;
            output.b = (float)rgbColor.B;
            output.a = 1.0f;
            return output;
        }

        private static LabColor Color2Lab(Color color)
        {
            var converter = new ColourfulConverter { WhitePoint = Illuminants.D50, TargetRGBWorkingSpace = RGBWorkingSpaces.sRGB };
            RGBColor rgbColor = new RGBColor(color.r, color.g, color.b);
            LabColor output = converter.ToLab(rgbColor);
            return output;
        }

        private static double LabDiff(LabColor c1, LabColor c2)
        {
            // https://en.wikipedia.org/wiki/Color_difference#CIE76
            double dL = c1.L - c2.L;
            double da = c1.a - c2.a;
            double db = c1.b - c2.b;
            double sqareSum = Math.Pow(dL, 2) + Math.Pow(da, 2) + Math.Pow(db, 2);
            return Math.Sqrt(sqareSum);
        }

        private static List<float> Color2List(Color color)
        {
            List<float> colorFloats = new List<float>();
            colorFloats.Add(color.r);
            colorFloats.Add(color.g);
            colorFloats.Add(color.b);
            return colorFloats;
        }

        public void GenerateSteps()
        {
            for (double i = 1.1; i < 2.1F; i += 0.1F)
            {
                _generatedDataContainer.Labels.Add((float)Math.Round(Math.Pow(10, i), 2));
            }
            System.Console.WriteLine("Done");
        }

        public void FindColours()
        {
            for (double r = 0.0; r <= 1.0; r += _step)
            {
                for (double g = 0.0; r <= 1.0; r += _step)
                {
                    for (double b = 0.0; r <= 1.0; r += _step)
                    {
                        Color color = new Color(r, g, b);
                        double colorDiff = ColorDiffBg(color);
                    }
                }
            }
        }
    }
}