public static class DXT4_RGBA
{

        public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            TextureColor[] pixels = new TextureColor[width * height];
            TextureColor[] color = new TextureColor[16];
            byte[] tempAlpha = new byte[8];
            byte[] alpha = new byte[16];
            byte[] colorByte = new byte[4];
            ushort[] tempc = new ushort[2];

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    int temp = bs.ReadUShort(endian);
                    byte[] tempa = new byte[2] { (byte)(temp & 0xFF), (byte)(temp >> 8) };
                    ulong AlphaUInt48 = bs.ReadUShort(endian) | ((ulong)bs.ReadUShort(endian) << 16) | ((ulong)bs.ReadUShort(endian) << 32);

                    // Calculate alpha values
					
                    if (tempa[0] > tempa[1] )
                    {
                        tempAlpha[0] = tempa[0];
                        tempAlpha[1] = tempa[1];
                        tempAlpha[2] = (byte)((6 * tempa[0] + tempa[1]) / 7);
                        tempAlpha[3] = (byte)((5 * tempa[0] + (tempa[1] << 1)) / 7);
                        tempAlpha[4] = (byte)(((tempa[0] << 2) + 3 * tempa[1]) / 7);
                        tempAlpha[5] = (byte)((3 * tempa[0] + (tempa[1] << 2)) / 7);
                        tempAlpha[6] = (byte)(((tempa[0] << 1) + 5 * tempa[1]) / 7);
                        tempAlpha[7] = (byte)((tempa[0] + 6 * tempa[1]) / 7);
                    }
                    else
                    {
                        tempAlpha[0] = tempa[0];
                        tempAlpha[1] = tempa[1];
                        tempAlpha[2] = (byte)(((tempa[0] << 2) + tempa[1]) / 5);
                        tempAlpha[3] = (byte)((3 * tempa[0] + (tempa[1] << 1)) / 5);
                        tempAlpha[4] = (byte)(((tempa[0] << 1) + 3 * tempa[1]) / 5);
                        tempAlpha[5] = (byte)((tempa[0] + (tempa[1] << 2)) / 5);
                        tempAlpha[6] = 0;
                        tempAlpha[7] = 255;
                    }
					
                    for (int i = 0; i < 16; i++)
                    {
                        alpha[i] = tempAlpha[AlphaUInt48 & 0b111];
                        AlphaUInt48 >>= 3;
                    }

                    // Calculate color values
					
                    tempc[0] = bs.ReadUShort(endian);
                    tempc[1] = bs.ReadUShort(endian);
                    int tempColor = bs.ReadUShort(endian);
                    colorByte[0] = (byte)(tempColor & 0xFF);
                    colorByte[1] = (byte)(tempColor >> 8);
                    tempColor = bs.ReadUShort(endian);
                    colorByte[2] = (byte)(tempColor & 0xFF);
                    colorByte[3] = (byte)(tempColor >> 8);

                    // Decode color
					
                    int b, g, r;
                    TextureColor[] tempColorArray = new TextureColor[4];
                    
                    b = tempc[0] & 0x1F;
                    g = (tempc[0] & 0x7E0) >> 5;
                    r = (tempc[0] & 0xF800) >> 11;
                    tempColorArray[0] = new((byte)(r << 3 | r >> 2), (byte)(g << 2 | g >> 3), (byte)(b << 3 | b >> 2));

                    b = tempc[1] & 0x1F;
                    g = (tempc[1] & 0x7E0) >> 5;
                    r = (tempc[1] & 0xF800) >> 11;
                    tempColorArray[1] = new( (byte)(r << 3 | r >> 2), (byte)(g << 2 | g >> 3), (byte)(b << 3 | b >> 2));

                    tempColorArray[2] = new(
                        (byte)(((tempColorArray[0].Red << 1) + tempColorArray[1].Red + 1) / 3),
                        (byte)(((tempColorArray[0].Green << 1) + tempColorArray[1].Green + 1) / 3),
                        (byte)(((tempColorArray[0].Blue << 1) + tempColorArray[1].Blue + 1) / 3)
                    );

                    tempColorArray[3] = new(
                        (byte)((tempColorArray[0].Red + (tempColorArray[1].Red << 1) + 1) / 3),
                        (byte)((tempColorArray[0].Green + (tempColorArray[1].Green << 1) + 1) / 3),
                        (byte)((tempColorArray[0].Blue + (tempColorArray[1].Blue << 1) + 1) / 3)
                    );

                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            int k = (i << 2) | j;
                            int bb = colorByte[i] & 0b11;
                            if (alpha[k] == 0)
                            {
                                color[k] = TextureColor.Default;
                            }
                            else
                            {
                                color[k] = new(
                                    C((tempColorArray[bb].Red << 8) / alpha[k]),
                                    C((tempColorArray[bb].Green << 8) / alpha[k]),
                                    C((tempColorArray[bb].Blue << 8) / alpha[k]),
                                    alpha[k]
                                );
                            }
							
                            colorByte[i] >>= 2;
                        }
                    }

                    // Assign colors to image
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

        static byte C(int v)
        {
            if (v >= 255) return 255;
            if (v <= 0) return 0;
            return (byte)v;
        }

        public static int Write(BinaryStream bs, SexyBitmap image, Endian endian = default)
        {
            TextureColor[] pixels = image.GetPixels();
            int width = image.Width;
            int height = image.Height;
            ushort[] temp = new ushort[4];
            TextureColor[] color = new TextureColor[16];
            byte maxAlpha, minAlpha;
            TextureColor min, max;
            int result;
            int tempValue;

            for (int i = 0; i < height; i += 4)
            {
                for (int w = 0; w < width; w += 4)
                {
                    maxAlpha = 0;
                    minAlpha = 255;

                    // Copy color
                    for (int j = 0; j < 4; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            if ((i + j) < height && (w + k) < width)
                            {
                                int n = (j << 2) | k;
                                int index = (i + j) * width + w + k;
                                byte apix = pixels[index].Alpha;
                                color[n] = new TextureColor(
                                    C((pixels[index].Red * apix) >> 8),
                                    C((pixels[index].Green * apix) >> 8),
                                    C((pixels[index].Blue * apix) >> 8),
                                    apix
                                );
                                byte a = color[n].Alpha;
                                if (a > maxAlpha) maxAlpha = a;
                                if (a < minAlpha) minAlpha = a;
                            }
                            else
                            {
                                color[(j << 2) | k] = TextureColor.Default;
                                minAlpha = 0;
                            }
                        }
                    }

                    // Alpha code, only use a1 > a2 mode
					
                    if (minAlpha == maxAlpha)
                    {
                        temp[0] = (ushort)((minAlpha << 8) | maxAlpha);
                        temp[1] = 0;
                        temp[2] = 0;
                        temp[3] = 0;
                    }
                    else
                    {
                        tempValue = (maxAlpha - minAlpha) >> 4;
                        maxAlpha = C(maxAlpha - tempValue);
                        minAlpha = C(minAlpha + tempValue);
                        temp[0] = (ushort)((minAlpha << 8) | maxAlpha);
                        byte[] alphaBytes = DXTEncode.EmitAlphaIndices(color, minAlpha, maxAlpha);
                        ulong flag = 0;
                        int pos = 0;
                        for (int ii = 0; ii < 16; ii++)
                        {
                            flag |= ((ulong)alphaBytes[ii]) << pos;
                            pos += 3;
                        }
                        temp[1] = (ushort)(flag & 0xFFFF);
                        temp[2] = (ushort)((flag >> 16) & 0xFFFF);
                        temp[3] = (ushort)(flag >> 32);
                    }

                    for (int ii = 0; ii < 4; ii++)
                    {
                        bs.WriteUShort(temp[ii], endian);
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
            return width;
        }
    }