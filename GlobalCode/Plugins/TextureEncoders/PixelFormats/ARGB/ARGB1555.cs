using System;
using System.IO;

public static class ARGB1555
{

        public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            int S = width * height;
            TextureColor[] pixels = new TextureColor[S];
            ushort temp;
            int r, g, b;
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadUShort(endian);
                r = (temp & 0x7C00) >> 10;
                g = (temp & 0x3E0) >> 5;
                b = temp & 0x1F;
                pixels[i] = new( (byte)( (r << 3) | (r >> 2) ), (byte)( (g << 3) | (g >> 2) ), (byte)( (b << 3) | (b >> 2) ), (byte)-(temp >> 15) );
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
                bs.WriteUShort((ushort)(((pixels[i].Alpha & 0x80) << 8) | (pixels[i].Blue >> 3) | ((pixels[i].Green & 0xF8) << 2) | ((pixels[i].Red & 0xF8) << 7)), endian);
            }
            return image.Width << 1;
        }
    }
