using System;
using System.IO;


public static class ARGB4444
{
        public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            int S = width * height;
            TextureColor[] pixels = new TextureColor[S];
            ushort temp;
            int r, g, b, a;
			
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadUShort(endian);
				
                a = temp >> 12;
                r = (temp & 0xF00) >> 8;
                g = (temp & 0xF0) >> 4;
                b = temp & 0xF;
				
                pixels[i] = new ( (byte)( (r << 4) | r), (byte)( (g << 4) | g), (byte)( (b << 4) | b), (byte)( (a << 4) | a) );
            }
            image.SetPixels(pixels);
            return image;
        }

        public static int Write(BinaryStream bs, SexyBitmap image, Endian endian = default)
        {
            TextureColor[] pixels = image.GetPixels();
            int S = pixels.Length;
            for (int i = 0; i < S; i++)
            {
            
                bs.WriteUShort((ushort)((pixels[i].Blue >> 4) | (pixels[i].Green & 0xF0) | 
                ((pixels[i].Red & 0xF0) << 4) | ((pixels[i].Alpha & 0xF0) << 8)), endian);
                
            }
            return image.Width << 1;
        }
    }
