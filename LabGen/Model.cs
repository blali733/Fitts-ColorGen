using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Colourful;
using Colourful.Conversion;
using LabGen.Helpers;
using Newtonsoft.Json;

namespace LabGen
{
    public class Model
    {
        private Container _generatedDataContainer;
        private List<List<LabColor>> _jndPayloadsList;
        private readonly LabColor _skyboxColor;
        private readonly double _diff;
        private readonly double _step;
        private double _jnd;

        public Model(Color color, double diff, double step, double jnd)
        {
            _generatedDataContainer = new Container();
            _skyboxColor = Color2Lab(color);
            _diff = diff;
            _step = step;
            _jnd = jnd;
            _jndPayloadsList = new List<List<LabColor>>();
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
            output.r = rgbColor.R;
            output.g = rgbColor.G;
            output.b = rgbColor.B;
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

        private static List<double> Color2List(Color color)
        {
            List<double> colordoubles = new List<double>();
            colordoubles.Add(color.r);
            colordoubles.Add(color.g);
            colordoubles.Add(color.b);
            return colordoubles;
        }

        public void GenerateSteps()
        {
            int[] vals = {7, 9, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20};
            foreach (var i in vals)
            {
                _generatedDataContainer.Labels.Add(Math.Round(Math.Pow(10, 0.1 * i), 2));
                _generatedDataContainer.Payload.Add(new List<List<double>>());
                _jndPayloadsList.Add(new List<LabColor>());
            }
            Console.WriteLine("Done");
        }

        public void FindColours()
        {
            double maxDistance = 0;
            int maxRange = (int)(1 / _step);
            for (int rc = 0; rc < maxRange; rc++)
            {
                double r = rc * _step;
                for (int gc = 0; gc < maxRange; gc++)
                {
                    double g = gc * _step;
                    for (int bc = 0; bc < maxRange; bc++)
                    {
                        double b = bc * _step;
                        Color color = new Color(r, g, b);
                        double colorDiff = ColorDiffBg(color);
                        double roundColorDiff = Math.Round(colorDiff, 2);
                        if (roundColorDiff > maxDistance)
                            maxDistance = roundColorDiff;
                        if (_generatedDataContainer.Labels.Contains(roundColorDiff))
                        {
                            if (Math.Abs(colorDiff - roundColorDiff) < _diff)
                            {
                                // check if Just Noticeable Difference (jnd) is maintained between consecutive matches:
                                int fuse = 0;
                                LabColor tempLabColor = Color2Lab(color);
                                foreach (var colorMatch in _jndPayloadsList[_generatedDataContainer.Labels.IndexOf(roundColorDiff)])
                                {
                                    if (LabDiff(colorMatch, tempLabColor) < _jnd)
                                    {
                                        fuse++;
                                        break;
                                    }
                                }
                                if (fuse == 0 && _generatedDataContainer.Payload[_generatedDataContainer.Labels.IndexOf(roundColorDiff)].Count < 25)
                                {
                                    _generatedDataContainer
                                        .Payload[_generatedDataContainer.Labels.IndexOf(roundColorDiff)]
                                        .Add(Color2List(color));
                                    _jndPayloadsList[_generatedDataContainer.Labels.IndexOf(roundColorDiff)].Add(Color2Lab(color));
                                    // Debug display only, thus not in view part
                                    System.Console.WriteLine(
                                        $"Found match with distance {roundColorDiff} at [{r}R, {g}G, {b}B]!");
                                    string str = "Counts: ";
                                    foreach (var cat in _generatedDataContainer.Payload)
                                    {
                                        str += $"{cat.Count}, ";
                                    }
                                    System.Console.WriteLine(str);
                                }
                                else
                                {
                                    // Debug display only, thus not in view part
                                    System.Console.WriteLine(
                                        $"Discarded match with distance {roundColorDiff} at [{r}R, {g}G, {b}B]!");
                                }
                            }
                        }
                    }
                }
            }
        }

        public void WriteResults()
        {
            _generatedDataContainer.Name = $"space_log_{_step}_{_diff}";
            File.WriteAllText($"./{_generatedDataContainer.Name}.json", JsonConvert.SerializeObject(_generatedDataContainer));
        }
    }
}