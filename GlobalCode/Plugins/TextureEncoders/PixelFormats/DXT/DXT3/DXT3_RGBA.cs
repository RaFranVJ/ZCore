public static class DXT3_RGBA
    {
        public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            TextureColor[] pixels = new TextureColor[width * height];
            TextureColor[] color = new TextureColor[16];
            ushort[] tempa = new ushort[4];
            ushort[] tempc = new ushort[2];
            TextureColor[] tempcolor = new TextureColor[4];
            byte[] ColorByte = new byte[4];
            byte[] alpha = new byte[16];
            int temp;

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    // Read alpha
					
                    for (int i = 0; i < 4; i++)
                    {
                        tempa[i] = bs.ReadUShort(endian);
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            temp = tempa[j] & 0xF;
                            alpha[(j << 2) | i] = (byte)((temp << 4) | temp);
                            tempa[j] >>= 4;
                        }
                    }

                    // Read color
					
                    tempc[0] = bs.ReadUShort(endian);
                    tempc[1] = bs.ReadUShort(endian);
                    temp = bs.ReadUShort(endian);
                    ColorByte[0] = (byte)(temp & 0xFF);
                    ColorByte[1] = (byte)(temp >> 8);
                    temp = bs.ReadUShort(endian);
                    ColorByte[2] = (byte)(temp & 0xFF);
                    ColorByte[3] = (byte)(temp >> 8);

                    // Compute colors
					
                    tempcolor[0] = Convert565ToColor(tempc[0]);
                    tempcolor[1] = Convert565ToColor(tempc[1]);
                    tempcolor[2] = InterpolateColor(tempcolor[0], tempcolor[1], 2, 1);
                    tempcolor[3] = InterpolateColor(tempcolor[0], tempcolor[1], 1, 2);

                    // Set Color Array
					
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            int k = (i << 2) | j;
                            int bb = ColorByte[i] & 0b11;
                            color[k] = new(tempcolor[bb].Red, tempcolor[bb].Green, tempcolor[bb].Blue, alpha[k]);
                            ColorByte[i] >>= 2;
                        }
                    }

                    // Set image Color
					
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if ((x + j) < width && (y + i) < height)
                            {
                                pixels[(i + y) * width + x + j] = color[(i << 2) | j];
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
            ushort temp;
            TextureColor[] color = new TextureColor[16];
            TextureColor min, max;
            int result;

            for (int i = 0; i < height; i += 4)
            {
                for (int w = 0; w < width; w += 4)
                {
                    // Copy colores
                    for (int j = 0; j < 4; j++)
                    {
                        temp = 0;
                        for (int k = 0; k < 4; k++)
                        {
                            if ((i + j) < height && (w + k) < width)
                            {
                                int n = (j << 2) | k;
                                color[n] = pixels[(i + j) * width + w + k];
                                temp |= (ushort)((color[n].Alpha >> 4) << (k << 2));
                            }
                            else
                            {
                                color[(j << 2) | k] = TextureColor.Default;
                            }
                        }
                        bs.WriteUShort(temp, endian);
                    }

                    // Code color
					
                    DXTEncode.GetMinMaxColorsByEuclideanDistance(color, out min, out max);
                    result = DXTEncode.EmitColorIndices(color, min, max);

                    // Write Result
					
                    bs.WriteUShort(DXTEncode.ColorTo565(max), endian);
                    bs.WriteUShort(DXTEncode.ColorTo565(min), endian);
                    bs.WriteUShort( (ushort)(result & 0xFFFF), endian);
                    bs.WriteUShort( (ushort)(result >> 16), endian);
                }
            }

            return width;
        }

        private static TextureColor Convert565ToColor(ushort color)
        {
            int b = color & 0x1F;
            int g = (color & 0x7E0) >> 5;
            int r = (color & 0xF800) >> 11;
			
            return new(
                (byte)(r << 3 | r >> 2),
                (byte)(g << 2 | g >> 3),
                (byte)(b << 3 | b >> 2));
				
        }

        private static TextureColor InterpolateColor(TextureColor c1, TextureColor c2, int weight1, int weight2)
        {
		
            return new(
                (byte)((c1.Red * weight1 + c2.Red * weight2) / (weight1 + weight2)),
                (byte)((c1.Green * weight1 + c2.Green * weight2) / (weight1 + weight2)),
                (byte)((c1.Blue * weight1 + c2.Blue * weight2) / (weight1 + weight2)));
        }
    }