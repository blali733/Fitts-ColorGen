using System;

namespace LabGen.Helpers
{
    public class Color
    {
        // Low effort pseudo (double instead of float) clone of Unity's Color class.
        public double r, g, b, a;

        public Color()
        {
            r = 0F;
            g = 0F;
            b = 0F;
            a = 0F;
        }

        public Color(double r, double g, double b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            a = 0F;
        }

        public Color(int r, int g, int b)
        {
            this.r = r / 255F;
            this.g = g / 255F;
            this.b = b / 255F;
            a = 0F;
        }

        public Color(double r, double g, double b, double a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
    }
}