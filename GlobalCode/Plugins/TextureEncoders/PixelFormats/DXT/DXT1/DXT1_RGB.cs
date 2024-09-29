public static class DXT1_RGB
{

        public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            TextureColor[] pixels = new TextureColor[width * height];
            TextureColor[] color = new TextureColor[16];
            ushort[] tempc = new ushort[2];
            TextureColor[] tempcolor = new TextureColor[4];
            byte[] ColorByte = new byte[4];
            int temp;
            int r, g, b;
            for (int y = 0; y < height; y += 4)
            {
				
                for (int x = 0; x < width; x += 4)
                {
                    // Compute color values
					
                    tempc[0] = bs.ReadUShort(endian);
                    tempc[1] = bs.ReadUShort(endian);
                    temp = bs.ReadUShort(endian);
                    ColorByte[0] = (byte)(temp & 0xFF);
                    ColorByte[1] = (byte)(temp >> 8);
                    temp = bs.ReadUShort(endian);
                    ColorByte[2] = (byte)(temp & 0xFF);
                    ColorByte[3] = (byte)(temp >> 8);
                    
                    // Compute color values
					
                    b = tempc[0] & 0x1F;
                    g = (tempc[0] & 0x7E0) >> 5;
                    r = (tempc[0] & 0xF800) >> 11;
                    tempcolor[0] = new( (byte)(r << 3 | r >> 2), (byte)(g << 2 | g >> 3), (byte)(b << 3 | b >> 2) );
                    b = tempc[1] & 0x1F;
                    g = (tempc[1] & 0x7E0) >> 5;
                    r = (tempc[1] & 0xF800) >> 11;
                    tempcolor[1] = new( (byte)(r << 3 | r >> 2), (byte)(g << 2 | g >> 3), (byte)(b << 3 | b >> 2) );

                    if (tempc[0] > tempc[1] )
                    {
                        tempcolor[2] = new(
                            (byte)(((tempcolor[0].Red << 1) + tempcolor[1].Red + 1) / 3), 
                            (byte)(((tempcolor[0].Green << 1) + tempcolor[1].Green + 1) / 3), 
                            (byte)(((tempcolor[0].Blue << 1) + tempcolor[1].Blue + 1) / 3));
							
                        tempcolor[3] = new(
                            (byte)((tempcolor[0].Red + (tempcolor[1].Red << 1) + 1) / 3), 
                            (byte)((tempcolor[0].Green + (tempcolor[1].Green << 1) + 1) / 3), 
                            (byte)((tempcolor[0].Blue + (tempcolor[1].Blue << 1) + 1) / 3));
                    }
					
                    else
                    {
                        tempcolor[2] = new(
                            (byte)((tempcolor[0].Red + tempcolor[1].Red) >> 1), 
                            (byte)((tempcolor[0].Green + tempcolor[1].Green) >> 1), 
                            (byte)((tempcolor[0].Blue + tempcolor[1].Blue) >> 1));
							
                        tempcolor[3] = TextureColor.Default;
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            int k = (i << 2) | j;
                            int bb = ColorByte[i] & 0b11;
                            color[k] = new(tempcolor[bb].Red, tempcolor[bb].Green, tempcolor[bb].Blue, tempcolor[bb].Alpha);
                            ColorByte[i] >>= 2;
                        }
                    }

                    // Assign values
					
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
            TextureColor[] color = new TextureColor[16];
            TextureColor min, max;
            int result;
            for (int i = 0; i < height; i += 4)
            {
                for (int w = 0; w < width; w += 4)
                {
                    // Copy color
					
                    for (int j = 0; j < 4; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            if ((i + j) < height && (w + k) < width)
                            {
                                color[(j << 2) | k] = pixels[(i + j) * width + w + k];
                            }
                            else
                            {
                                color[(j << 2) | k] = TextureColor.Default;
                            }
                        }
                    }

                    // Color code
					
                    DXTEncode.GetMinMaxColorsByEuclideanDistance(color, out min, out max);
                    result = DXTEncode.EmitColorIndices(color, min, max);

                    // Write
                    bs.WriteUShort(DXTEncode.ColorTo565(max), endian);
                    bs.WriteUShort(DXTEncode.ColorTo565(min), endian);
                    bs.WriteUShort( (ushort)(result & 0xFFFF), endian);
                    bs.WriteUShort( (ushort)(result >> 16), endian);
                }
            }
            return width >> 1;
        }
    }