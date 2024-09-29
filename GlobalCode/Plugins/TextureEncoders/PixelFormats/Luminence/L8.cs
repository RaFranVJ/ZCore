public static class L8
 {
        public static SexyBitmap Read(BinaryStream bs, int width, int height)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            TextureColor[] pixels = image.GetPixels();
            int S = width * height;
			
            for (int i = 0; i < S; i++)
            {
                byte l = bs.ReadByte();
                pixels[i] = new(l, l, l, 255);
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
                bs.WriteByte(luminance);
            }
			
            return image.Width;
        }
    }