public static class RGBA5551
    {
        public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            int S = width * height;
            TextureColor[] pixels = image.GetPixels();
            ushort temp;
            int r, g, b;

            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadUShort(endian);
				
                r = (temp & 0xF800) >> 11;
                g = (temp & 0x7C0) >> 6;
                b = (temp & 0x3E) >> 1;
				
                pixels[i] = new(
                    (byte)((r << 3) | (r >> 2)),
                    (byte)((g << 3) | (g >> 2)),
                    (byte)((b << 3) | (b >> 2)),
                    (byte)-(temp & 0x1)
                );
            }
			
			image.SetPixels(pixels);

            return image;
        }

        public static int Write(BinaryStream bs, SexyBitmap image, Endian endian = default)
        {
            TextureColor[] pixels = image.GetPixels();
            int S = image.Square;

            for (int i = 0; i < S; i++)
            {
                bs.WriteUShort((ushort)(
                    ((pixels[i].Alpha & 0x80) >> 7) |
                    ((pixels[i].Blue & 0xF8) >> 2) |
                    ((pixels[i].Green & 0xF8) << 3) |
                    ((pixels[i].Red & 0xF8) << 8)
                ), endian);
            }

            return image.Width << 1;
        }
    }