public static class LA44
    {
        public static SexyBitmap Read(BinaryStream bs, int width, int height)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            TextureColor[] pixels = image.GetPixels();
            int S = width * height;

            for (int i = 0; i < S; i++)
            {
                byte temp = bs.ReadByte();
                int a = temp & 0xF;
                temp >>= 4;
                byte l = (byte)(temp | (temp << 4));

                pixels[i] = new(l, l, l, (byte)(a | (a << 4)) );
            }
			
			image.SetPixels(pixels);
			
            return image;
        }

        public static int Write(BinaryStream bs, SexyBitmap image)
        {
            TextureColor[] pixels = image.GetPixels();
            int S = image.Width * image.Height;

            for (int i = 0; i < S; i++)
            {
                TextureColor color = pixels[i];
                byte luminance = (byte)(color.Red * 0.299 + color.Green * 0.587 + color.Blue * 0.114);
                byte alpha = (byte)(color.Alpha >> 4);

                bs.WriteByte((byte)(((luminance & 0xF0) | alpha)));
            }
            return image.Width;
        }
    }