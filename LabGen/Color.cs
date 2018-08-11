using System;

namespace LabGen
{
    public class Color
    {
        // Low effort clone of Unity's Color class.
        public float r, g, b, a;

        public Color()
        {
            r = 0F;
            g = 0F;
            b = 0F;
            a = 0F;
        }

        public Color(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            a = 0F;
        }

        public Color(double r, double g, double b)
        {
            this.r = (float)Math.Round(r, 4);
            this.g = (float)Math.Round(g, 4);
            this.b = (float)Math.Round(b, 4);
            a = 0F;
        }

        public Color(int r, int g, int b)
        {
            this.r = r / 255F;
            this.g = g / 255F;
            this.b = b / 255F;
            a = 0F;
        }

        public Color(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
    }
}