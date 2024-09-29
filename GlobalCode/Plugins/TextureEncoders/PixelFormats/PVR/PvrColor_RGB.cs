    public struct PvrColor_RGB
    {
        public int Red;
        public int Green;
        public int Blue;

        public PvrColor_RGB(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public static PvrColor_RGB operator +(PvrColor_RGB c1, PvrColor_RGB c2)
        {
            return new PvrColor_RGB(c1.Red + c2.Red, c1.Green + c2.Green, c1.Blue + c2.Blue);
        }

                public static PvrColor_RGB operator -(PvrColor_RGB c1, PvrColor_RGB c2)
        {
            return new PvrColor_RGB(c1.Red - c2.Red, c1.Green - c2.Green, c1.Blue - c2.Blue);
        }

        public static PvrColor_RGB operator *(PvrColor_RGB c, byte factor)
        {
            return new PvrColor_RGB(c.Red * factor, c.Green * factor, c.Blue * factor);
        }

        public static int operator %(PvrColor_RGB c1, PvrColor_RGB c2)
        {
            // Dot product of two PvrColor_RGB
            return (c1.Red * c2.Red + c1.Green * c2.Green + c1.Blue * c2.Blue);
        }
    }