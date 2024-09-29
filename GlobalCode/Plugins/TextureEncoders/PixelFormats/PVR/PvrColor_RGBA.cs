public class PvrColor_RGBA
{
public int Red{ get; set; }


public int Green{ get; set; }

public int Blue{ get; set; }

 public int Alpha{ get; set; }

 public PvrColor_RGBA()
 {

 }

  public PvrColor_RGBA(int red, int green, int blue, int alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }


    public PvrColor_RGBA(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public static PvrColor_RGBA operator +(PvrColor_RGBA c1, PvrColor_RGBA c2)
        {
            return new PvrColor_RGBA(c1.Red + c2.Red, c1.Green + c2.Green, c1.Blue + c2.Blue, c1.Alpha + c2.Alpha);
        }

        public static PvrColor_RGBA operator -(PvrColor_RGBA c1, PvrColor_RGBA c2)
        {
            return new PvrColor_RGBA(c1.Red - c2.Red, c1.Green - c2.Green, c1.Blue - c2.Blue, c1.Alpha - c2.Alpha);
        }

        public static PvrColor_RGBA operator *(PvrColor_RGBA c, byte factor)
        {
            return new PvrColor_RGBA(c.Red * factor, c.Green * factor, c.Blue * factor, c.Alpha * factor);
        }

        public static int operator %(PvrColor_RGBA c1, PvrColor_RGBA c2)
        {
            // Dot product of two PvrColor_RGBA
            return c1.Red * c2.Red + c1.Green * c2.Green + c1.Blue * c2.Blue + c1.Alpha * c2.Alpha;
        }
		
}