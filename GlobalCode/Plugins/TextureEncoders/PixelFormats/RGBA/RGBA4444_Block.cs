public static class RGBA4444_Block
    {
        public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            TextureColor[] pixels = image.GetPixels();
            int S = width * height;

            for (int i = 0; i < height; i += 32)
            {
                for (int w = 0; w < width; w += 32)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        for (int k = 0; k < 32; k++)
                        {
                            ushort temp = bs.ReadUShort(endian);
                            if ((i + j) < height && (w + k) < width)
                            {
                                int r = temp >> 12;
                                int g = (temp & 0xF00) >> 8;
                                int b = (temp & 0xF0) >> 4;
                                int a = temp & 0xF;
                                pixels[(i + j) * width + w + k] = new(
                                    (byte)((r << 4) | r),
                                    (byte)((g << 4) | g),
                                    (byte)((b << 4) | b),
                                    (byte)((a << 4) | a)
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
            int newwidth = width;
            
            if ((newwidth & 31) != 0)
            {
                newwidth |= 31;
                newwidth++;
            }

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
                                int temp = (i + j) * width + w + k;
                                bs.WriteUShort( (ushort)(
                                    (pixels[temp].Alpha >> 4) |
                                    (pixels[temp].Blue & 0xF0) |
                                    ((pixels[temp].Green & 0xF0) << 4) |
                                    ((pixels[temp].Red & 0xF0) << 8)
                                ), endian);
                            }
                            else
                            {
                                bs.WriteUShort(0, endian);
                            }
                        }
                    }
                }
            }

            return newwidth << 1;
        }
    }