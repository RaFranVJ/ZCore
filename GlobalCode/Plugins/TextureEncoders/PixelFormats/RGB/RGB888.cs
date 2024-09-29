public static class RGB888
{
		
        public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            TextureColor[] pixels = image.GetPixels();
            int S = width * height;

            for (int i = 0; i < S; i++)
            {
                uint temp = bs.ReadUTripleByte(endian);
                pixels[i] = new TextureColor(
                    (byte)(temp >> 16),
                    (byte)((temp & 0xFF00) >> 8),
                    (byte)(temp & 0xFF)
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
                bs.WriteUTripleByte( (uint)((pixels[i].Red << 16) | (pixels[i].Green << 8) | pixels[i].Blue), endian);
            }

            return image.Width * 3;
        }
    }