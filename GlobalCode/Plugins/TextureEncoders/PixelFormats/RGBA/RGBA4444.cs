public static class RGBA4444
    {
        public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            TextureColor[] pixels = image.GetPixels();
            int S = width * height;

            for (int i = 0; i < S; i++)
            {
                ushort temp = bs.ReadUShort(endian);
				
                int r = temp >> 12;
                int g = (temp & 0xF00) >> 8;
                int b = (temp & 0xF0) >> 4;
                int a = temp & 0xF;
				
                pixels[i] = new(
                    (byte)((r << 4) | r),
                    (byte)((g << 4) | g),
                    (byte)((b << 4) | b),
                    (byte)((a << 4) | a)
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
                bs.WriteUShort( (ushort)(
                    (pixels[i].Alpha >> 4) |
                    (pixels[i].Blue & 0xF0) |
                    ((pixels[i].Green & 0xF0) << 4) |
                    ((pixels[i].Red & 0xF0) << 8)
                ), endian);
            }

            return image.Width << 1;
        }
    }