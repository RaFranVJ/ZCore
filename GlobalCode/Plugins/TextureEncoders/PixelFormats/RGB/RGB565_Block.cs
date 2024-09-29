public static class RGB565_Block
{
	
        public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            TextureColor[] pixels = image.GetPixels();

            for (int i = 0; i < height; i += 32)
            {
                for (int w = 0; w < width; w += 32)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        for (int k = 0; k < 32; k++)
                        {
                            if ((i + j) < height && (w + k) < width)
                            {
                                ushort temp = bs.ReadUShort(endian);
                                int index = (i + j) * width + w + k;
                                pixels[index] = new(
                                    (byte)((temp & 0xF800) >> 8),
                                    (byte)((temp & 0x7E0) >> 3),
                                    (byte)((temp & 0x1F) << 3),
                                    255
                                );
                            }
                        }
                    }
                }
            }
			
			image.SetPixels(pixels);

            return image;
        }

        public static int Write(BinaryStream bs, SexyBitmap image, Endian endian = default)
        {
            TextureColor[] pixels = image.GetPixels();
            int width = image.Width;
            int height = image.Height;
            int newWidth = width;

            if ((newWidth & 31) != 0)
            {
                newWidth |= 31;
                newWidth++;
            }

            for (int i = 0; i < height; i += 32)
            {
                for (int w = 0; w < newWidth; w += 32)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        for (int k = 0; k < 32; k++)
                        {
                            if ((i + j) < height && (w + k) < width)
                            {
                                int index = (i + j) * width + w + k;
                                var pixel = pixels[index];
                                ushort rgb565 = (ushort)(
                                    ((pixel.Blue & 0xF8) >> 3) |
                                    ((pixel.Green & 0xFC) << 3) |
                                    ((pixel.Red & 0xF8) << 8)
                                );
                                bs.WriteUShort(rgb565, endian);
                            }
							
                            else
                            {
                                bs.WriteUShort(0, endian);
                            }
                        }
                    }
                }
            }

            return newWidth * 2;
        }
    }