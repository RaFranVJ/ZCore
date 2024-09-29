public static class DXT5_RGBA
{
        public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
        {
            SexyBitmap image = SexyBitmap.CreateNew(width, height);
            TextureColor[] pixels = new TextureColor[width * height];
            TextureColor[] color = new TextureColor[16];
            int[] tempa = new int[2];
            int[] tempalpha = new int[8];
            ulong AlphaUInt48;
            ushort[] tempc = new ushort[2];
            TextureColor[] tempcolor = new TextureColor[4];
            byte[] ColorByte = new byte[4];
            byte[] alpha = new byte[16];
            int temp, r, g, b;

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    temp = bs.ReadUShort(endian);
                    tempa[0] = temp & 0xFF;
                    tempa[1] = temp >> 8;
                    
                    AlphaUInt48 = bs.ReadUShort(endian) | ( ( (ulong)bs.ReadUShort(endian) ) << 16) | 
                    (((ulong)bs.ReadUShort(endian) ) << 32);

                    // Calculate alpha values
					
                    if (tempa[0] > tempa[1])
                    {
                        tempalpha[0] = tempa[0];
                        tempalpha[1] = tempa[1];
                        tempalpha[2] = (6 * tempa[0] + tempa[1]) / 7;
                        tempalpha[3] = (5 * tempa[0] + (tempa[1] << 1)) / 7;
                        tempalpha[4] = ((tempa[0] << 2) + 3 * tempa[1]) / 7;
                        tempalpha[5] = (3 * tempa[0] + (tempa[1] << 2)) / 7;
                        tempalpha[6] = ((tempa[0] << 1) + 5 * tempa[1]) / 7;
                        tempalpha[7] = (tempa[0] + 6 * tempa[1]) / 7;
                    }
					
                    else
                    {
                        tempalpha[0] = tempa[0];
                        tempalpha[1] = tempa[1];
                        tempalpha[2] = ((tempa[0] << 2) + tempa[1]) / 5;
                        tempalpha[3] = (3 * tempa[0] + (tempa[1] << 1)) / 5;
                        tempalpha[4] = ((tempa[0] << 1) + 3 * tempa[1]) / 5;
                        tempalpha[5] = (tempa[0] + (tempa[1] << 2)) / 5;
                        tempalpha[6] = 0;
                        tempalpha[7] = 255;
                    }
					
                    for (int i = 0; i < 16; i++)
                    {
                        alpha[i] = (byte)tempalpha[AlphaUInt48 & 0b111];
                        AlphaUInt48 >>= 3;
                    }

                    // Calculate color values
					
                    tempc[0] = bs.ReadUShort(endian);
                    tempc[1] = bs.ReadUShort(endian);
                    temp = bs.ReadUShort(endian);
                    ColorByte[0] = (byte)(temp & 0xFF);
                    ColorByte[1] = (byte)(temp >> 8);
                    temp = bs.ReadUShort(endian);
                    ColorByte[2] = (byte)(temp & 0xFF);
                    ColorByte[3] = (byte)(temp >> 8);

                    // Calculate color values
					
                    b = tempc[0] & 0x1F;
                    g = (tempc[0] & 0x7E0) >> 5;
                    r = (tempc[0] & 0xF800) >> 11;
                    tempcolor[0] = new( (byte)(r << 3 | r >> 2), (byte)(g << 2 | g >> 3), (byte)(b << 3 | b >> 2) );

                    b = tempc[1] & 0x1F;
                    g = (tempc[1] & 0x7E0) >> 5;
                    r = (tempc[1] & 0xF800) >> 11;
                    tempcolor[1] = new( (byte)(r << 3 | r >> 2), (byte)(g << 2 | g >> 3), (byte)(b << 3 | b >> 2) );

                    tempcolor[2] = new(
                        (byte)(((tempcolor[0].Red << 1) + tempcolor[1].Red + 1) / 3),
                        (byte)(((tempcolor[0].Green << 1) + tempcolor[1].Green + 1) / 3),
                        (byte)(((tempcolor[0].Blue << 1) + tempcolor[1].Blue + 1) / 3)
                    );

                    tempcolor[3] = new(
                        (byte)((tempcolor[0].Red + (tempcolor[1].Red << 1) + 1) / 3),
                        (byte)((tempcolor[0].Green + (tempcolor[1].Green << 1) + 1) / 3),
                        (byte)((tempcolor[0].Blue + (tempcolor[1].Blue << 1) + 1) / 3)
                    );

                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            int k = (i << 2) | j;
                            int bb = ColorByte[i] & 0b11;
                            color[k] = new(tempcolor[bb].Red, tempcolor[bb].Green, tempcolor[bb].Blue, alpha[k] );
                            ColorByte[i] >>= 2;
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

            // Set the pixels in the image
			
            image.SetPixels(pixels);
            return image;
        }

      private static byte C(int v)
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
            byte maxalpha, minalpha;
            TextureColor min, max;
            int result;
            int tempvalue;

            for (int i = 0; i < height; i += 4)
            {
                for (int w = 0; w < width; w += 4)
                {
                    maxalpha = 0;
                    minalpha = 255;

                    // Copy color
					
                    for (int j = 0; j < 4; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            if ((i + j) < height && (w + k) < width)
                            {
                                int n = (j << 2) | k;
                                color[n] = pixels[(i + j) * width + w + k];
                                byte a = color[n].Alpha;
                                if (a > maxalpha) maxalpha = a;
                                if (a < minalpha) minalpha = a;
                            }
                            else
                            {
                                color[(j << 2) | k] = TextureColor.Default;
                                minalpha = 0;
                            }
                        }
                    }

                    // Alpha code, only use a1 > a2 mode
					
                    if (minalpha == maxalpha)
                    {
                        temp[0] = (ushort)((minalpha << 8) | maxalpha);
                        temp[1] = 0;
                        temp[2] = 0;
                        temp[3] = 0;
                    }
                    else
                    {
                        tempvalue = (maxalpha - minalpha) >> 4;
                        maxalpha = C(maxalpha - tempvalue);
                        minalpha = C(minalpha + tempvalue);
                        temp[0] = (ushort)((minalpha << 8) | maxalpha);
                        byte[] alphabytes = DXTEncode.EmitAlphaIndices(color, minalpha, maxalpha);
                        ulong flag = 0;
                        int pos = 0;
                        for (int ii = 0; ii < 16; ii++)
                        {
                            flag |= ((ulong)alphabytes[ii]) << pos;
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
                    bs.WriteUShort((ushort)(result >> 16), endian);
                }
            }
            return width;
        }
    }