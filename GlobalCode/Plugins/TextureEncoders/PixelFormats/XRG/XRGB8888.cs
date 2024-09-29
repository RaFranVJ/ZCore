public static class XRGB8888
    {
        public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            TextureColor[] pixels = image.GetPixels();
            uint temp;
            for (int i = 0; i < width * height; i++)
            {
                temp = bs.ReadUInt(endian);
                pixels[i] = new TextureColor(
                    (byte)((temp & 0xFF0000) >> 16),
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
                bs.WriteUInt(0xFF000000 | ((uint)pixels[i].Red << 16) | ((uint)pixels[i].Green << 8) | pixels[i].Blue, endian);
            }
            return image.Width * 4;
        }
    }
