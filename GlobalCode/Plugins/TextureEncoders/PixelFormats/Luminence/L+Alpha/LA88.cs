public static class LA88
    {
        public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            TextureColor[] pixels = image.GetPixels();
            int S = width * height;

            for (int i = 0; i < S; i++)
            {
                ushort temp = bs.ReadUShort(endian);
                byte l = (byte)(temp >> 8);
                byte a = (byte)(temp & 0xFF);

                pixels[i] = new(l, l, l, a);
            }
			
			image.SetPixels(pixels);

            return image;
        }

        public static int Write(BinaryStream bs, SexyBitmap image, Endian endian = default)
        {
            TextureColor[] pixels = image.GetPixels();
            int S = image.Width * image.Height;

            for (int i = 0; i < S; i++)
            {
                TextureColor color = pixels[i];
                byte luminance = (byte)(color.Red * 0.299 + color.Green * 0.587 + color.Blue * 0.114);
                ushort temp = (ushort)((luminance << 8) | color.Alpha);

                bs.WriteUShort(temp, endian);
            }

            return image.Width * 2;
        }
    }