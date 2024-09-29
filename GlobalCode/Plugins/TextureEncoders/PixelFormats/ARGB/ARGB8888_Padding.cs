public static class ARGB8888_Padding
    {
	
        public static SexyBitmap Read(BinaryStream bs, int width, int height, int blockSize, Endian endian = default)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            TextureColor[] pixels = new TextureColor[width * height];
            uint temp;
            long off = bs.Position;
            int times = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    temp = bs.ReadUInt(endian);
                    pixels[i * width + j] = new( (byte)((temp & 0xFF0000) >> 16), (byte)((temp & 0xFF00) >> 8), (byte)(temp & 0xFF), (byte)(temp >> 24));
                }
                bs.Position = off + (++times) * blockSize;
            }
            image.SetPixels(pixels);
            return image;
        }

        public static int Write(BinaryStream bs, SexyBitmap image, int blockSize, Endian endian = default)
        {
            TextureColor[] pixels = image.GetPixels();
            int height = image.Height;
            int width = image.Width;
            int CDSize = blockSize - (width << 2);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    TextureColor pixel = pixels[i * width + j];
                    bs.WriteUInt(((uint)pixel.Alpha << 24) | ((uint)pixel.Red << 16) | ((uint)pixel.Green << 8) | pixel.Blue, endian);
                }
                for (int j = 0; j < CDSize; j++)
                {
                    bs.WriteByte(0x0);
                }
            }
            return blockSize;
        }
    }