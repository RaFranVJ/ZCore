public static class RGBA5551_Block
    {
        public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            TextureColor[] pixels = image.GetPixels();
            ushort temp;
            int r, g, b;

            for (int i = 0; i < height; i += 32)
            {
                for (int w = 0; w < width; w += 32)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        for (int k = 0; k < 32; k++)
                        {
                            temp = bs.ReadUShort(endian);
							
                            if ((i + j) < height && (w + k) < width)
                            {
                                r = (temp & 0xF800) >> 11;
                                g = (temp & 0x7C0) >> 6;
                                b = (temp & 0x3E) >> 1;
                                pixels[(i + j) * width + w + k] = new(
                                    (byte)((r << 3) | (r >> 2)),
                                    (byte)((g << 3) | (g >> 2)),
                                    (byte)((b << 3) | (b >> 2)),
                                    (byte)-(temp & 0x1)
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
                                bs.WriteUShort((ushort)(
                                    ((pixels[temp].Alpha & 0x80) >> 7) |
                                    ((pixels[temp].Blue & 0xF8) >> 2) |
                                    ((pixels[temp].Green & 0xF8) << 3) |
                                    ((pixels[temp].Red & 0xF8) << 8)
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