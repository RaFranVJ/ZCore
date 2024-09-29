/// <summary> reference: https://www.researchgate.net/publication/259000525_Real-Time_DXT_Compression </summary>
	
public static class DXTEncode
    {
        public static ushort ColorTo565(TextureColor color)
        {
            return (ushort)(((color.Red >> 3) << 11) | ((color.Green >> 2) << 5) | (color.Blue >> 3));
        }

        static void SwapColors(ref TextureColor c1, ref TextureColor c2)
        {
            TextureColor temp = c1;
            c1 = c2;
            c2 = temp;
        }

        // Use Euclidean distance
        static int ColorDistance(TextureColor c1, TextureColor c2)
        {
            return Pow2(c1.Red - c1.Red) + Pow2(c1.Green - c2.Green) + Pow2(c1.Blue - c2.Blue);
        }

        static int Pow2(int input)
        {
            return input * input;
        }

        public static void GetMinMaxColorsByEuclideanDistance(TextureColor[] colorBlock, out TextureColor minColor,
        out TextureColor maxColor)
        {
            minColor = TextureColor.Default;
            maxColor = TextureColor.Default;
            int maxDistance = -1;
            
            for (int i = 0; i < 15; i++)
            {
                for (int j = i + 1; j < 16; j++)
                {
                    int distance = ColorDistance(colorBlock[i], colorBlock[j]);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        minColor = colorBlock[i];
                        maxColor = colorBlock[j];
                    }
                }
            }
            if (ColorTo565(maxColor) < ColorTo565(minColor))
            {
                SwapColors(ref minColor, ref maxColor);
            }
        }

        public static void GetMinMaxColorsByEuclideanDistanceForDXT1RGBA(TextureColor[] colorBlock, out TextureColor minColor,
         out TextureColor maxColor)
        {
            minColor = TextureColor.Default;
            maxColor = TextureColor.Default;
            int maxDistance = -1;

            for (int i = 0; i < 15; i++)
            {
                if (colorBlock[i].Alpha < 0x80) continue;
                for (int j = i + 1; j < 16; j++)
                {
                    if (colorBlock[j].Alpha < 0x80) continue;
                    int distance = ColorDistance(colorBlock[i], colorBlock[j]);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        minColor = colorBlock[i];
                        maxColor = colorBlock[j];
                    }
                }
            }
            if (ColorTo565(maxColor) < ColorTo565(minColor))
            {
                SwapColors(ref minColor, ref maxColor);
            }
        }

        static int Abs(int v)
        {
            return v < 0 ? -v : v;
        }

        public static int EmitColorIndices(TextureColor[] colorBlock, TextureColor minColor, TextureColor maxColor)
        {
            int[] colors = new int[16];
            int result = 0;
            colors[0] = (maxColor.Red & 0xF8) | (maxColor.Red >> 5);
            colors[1] = (maxColor.Green & 0xFC) | (maxColor.Green >> 6);
            colors[2] = (maxColor.Blue & 0xF8) | (maxColor.Blue >> 5);
            colors[4] = (minColor.Red & 0xF8) | (minColor.Red >> 5);
            colors[5] = (minColor.Green & 0xFC) | (minColor.Green >> 6);
            colors[6] = (minColor.Blue & 0xF8) | (minColor.Blue >> 5);
            colors[8] = ((colors[0] << 1) + colors[4]) / 3;
            colors[9] = ((colors[1] << 1) + colors[5]) / 3;
            colors[10] = ((colors[2] << 1) + colors[6]) / 3;
            colors[12] = (colors[0] + (colors[4] << 1)) / 3;
            colors[13] = (colors[1] + (colors[5] << 1)) / 3;
            colors[14] = (colors[2] + (colors[6] << 1)) / 3;
            for (int i = 15; i >= 0; i--)
            {
                int c0 = colorBlock[i].Red;
                int c1 = colorBlock[i].Green;
                int c2 = colorBlock[i].Blue;
                int d0 = Abs(colors[0] - c0) + Abs(colors[1] - c1) + Abs(colors[2] - c2);
                int d1 = Abs(colors[4] - c0) + Abs(colors[5] - c1) + Abs(colors[6] - c2);
                int d2 = Abs(colors[8] - c0) + Abs(colors[9] - c1) + Abs(colors[10] - c2);
                int d3 = Abs(colors[12] - c0) + Abs(colors[13] - c1) + Abs(colors[14] - c2);
                int b0 = d0 > d3 ? 1 : 0;
                int b1 = d1 > d2 ? 1 : 0;
                int b2 = d0 > d2 ? 1 : 0;
                int b3 = d1 > d3 ? 1 : 0;
                int b4 = d2 > d3 ? 1 : 0;
                int x0 = b1 & b2;
                int x1 = b0 & b3;
                int x2 = b0 & b4;
                result |= (x2 | ((x0 | x1) << 1)) << (i << 1);
            }
            return result;
        }

        public static int EmitColorIndicesForDXT1RGBA(TextureColor[] colorBlock, TextureColor minColor, TextureColor maxColor)
        {
            int[] colors = new int[16];
            int result = 0;
            colors[0] = (maxColor.Red & 0xF8) | (maxColor.Red >> 5);
            colors[1] = (maxColor.Green & 0xFC) | (maxColor.Green >> 6);
            colors[2] = (maxColor.Blue & 0xF8) | (maxColor.Blue >> 5);
            colors[4] = (minColor.Red & 0xF8) | (minColor.Red >> 5);
            colors[5] = (minColor.Green & 0xFC) | (minColor.Green >> 6);
            colors[6] = (minColor.Blue & 0xF8) | (minColor.Blue >> 5);
            colors[8] = (colors[0] + colors[4]) >> 1;
            colors[9] = (colors[1] + colors[5]) >> 1;
            colors[10] = (colors[2] + colors[6]) >> 1;
            colors[12] = 0;
            colors[13] = 0;
            colors[14] = 0;
            for (int i = 15; i >= 0; i--)
            {
                if (colorBlock[i].Alpha < 0x80)
                {
                    result |= (0b11) << (i << 1);
                }
                else
                {
                    int c0 = colorBlock[i].Red;
                    int c1 = colorBlock[i].Green;
                    int c2 = colorBlock[i].Blue;
                    int d0 = Abs(colors[0] - c0) + Abs(colors[1] - c1) + Abs(colors[2] - c2);
                    int d1 = Abs(colors[4] - c0) + Abs(colors[5] - c1) + Abs(colors[6] - c2);
                    int d2 = Abs(colors[8] - c0) + Abs(colors[9] - c1) + Abs(colors[10] - c2);
                    if (d0 > d2 && d1 > d2)
                    {
                        result |= (0b10) << (i << 1);
                    }
                    else if (d1 > d0)
                    {
                        result |= (0b01) << (i << 1);
                    }
                }
            }
            return result;
        }

        public static byte[] EmitAlphaIndices(TextureColor[] colorBlock, byte minAlpha, byte maxAlpha)
        {
            byte[] indices = new byte[16];
            byte[] alphas = new byte[8];
            alphas[0] = maxAlpha;
            alphas[1] = minAlpha;
            alphas[2] = (byte)((6 * maxAlpha + minAlpha) / 7);
            alphas[3] = (byte)((5 * maxAlpha + (minAlpha << 1)) / 7);
            alphas[4] = (byte)(((maxAlpha << 2) + 3 * minAlpha) / 7);
            alphas[5] = (byte)((3 * maxAlpha + (minAlpha << 2)) / 7);
            alphas[6] = (byte)(((maxAlpha << 1) + 5 * minAlpha) / 7);
            alphas[7] = (byte)((maxAlpha + 6 * minAlpha) / 7);
            for (int i = 0; i < 16; i++)
            {
                int minDistance = int.MaxValue;
                byte a = colorBlock[i].Alpha;
                for (byte j = 0; j < 8; j++)
                {
                    int dist = Abs(a - alphas[j]);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        indices[i] = j;
                    }
                }
            }
            return indices;
        }
    }