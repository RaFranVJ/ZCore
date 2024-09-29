public static class RGB565
{
        public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            int S = width * height;
            TextureColor[] pixels = image.GetPixels();
            
            for (int i = 0; i < S; i++)
            {
                ushort temp = bs.ReadUShort(endian);
				
                int r = temp >> 11;
                int g = (temp & 0x7E0) >> 5;
                int b = temp & 0x1F;
				
                pixels[i] = new(
                    (byte)((r << 3) | (r >> 2)),
                    (byte)((g << 2) | (g >> 4)),
                    (byte)((b << 3) | (b >> 2))
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
                var pixel = pixels[i];
                ushort rgb565 = (ushort)((pixel.Blue >> 3) | ((pixel.Green & 0xFC) << 3) | ((pixel.Red & 0xF8) << 8));
                bs.WriteUShort(rgb565, endian);
            }
			
            return image.Width * 2;
        }
    }