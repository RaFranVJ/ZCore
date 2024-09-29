﻿using System;

public static class DXT5_RGBA_Morton
{

private static readonly int[] Order = { 0, 2, 8, 10, 1, 3, 9, 11, 4, 6, 12, 14, 5, 7, 13, 15 };

        public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
        {
            bool t = false;
            int newwidth = width;
            int newheight = height;
            if ((newwidth & (newwidth - 1)) != 0)
            {
                newwidth = 0b10 << ((int)Math.Floor(Math.Log2(newwidth)));
                t = true;
            }
            if ((newheight & (newheight - 1)) != 0)
            {
                newheight = 0b10 << ((int)Math.Floor(Math.Log2(newheight)));
                t = true;
            }
            SexyBitmap image = SexyBitmap.CreateNew(newwidth, newheight);
            var pixels = image.GetPixels();
            var color = new TextureColor[16];
            var tempa = new int[2];
            var tempalpha = new int[8];
            long AlphaUInt48;
            var tempc = new ushort[2];
            var tempcolor = new TextureColor[4];
            var ColorByte = new byte[4];
            var alpha = new byte[16];
            int temp;
            int r, g, b;
            int pixelOffset = 0;
            int minwh = newwidth > newheight ? newheight : newwidth;
            int mink = (int)Math.Log(minwh, 2);
            bool bigwidth = newwidth > newheight;
            for (int y = 0; y < newheight; y += 4)
            {
                for (int x = 0; x < newwidth; x += 4)
                {
                    temp = bs.ReadUShort(endian);
                    tempa[0] = temp & 0xFF;
                    tempa[1] = temp >> 8;

                    AlphaUInt48 = bs.ReadUShort(endian) | ( ( (long)bs.ReadUShort(endian) ) << 16) | 
                    ( (  (long)bs.ReadUShort(endian) ) << 32);
                    
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
                    
                    tempc[0] = bs.ReadUShort(endian);
                    tempc[1] = bs.ReadUShort(endian);
                    temp = bs.ReadUShort(endian);
                    ColorByte[0] = (byte)(temp & 0xFF);
                    ColorByte[1] = (byte)(temp >> 8);
                    temp = bs.ReadUShort(endian);
                    ColorByte[2] = (byte)(temp & 0xFF);
                    ColorByte[3] = (byte)(temp >> 8);
                    
                    b = tempc[0] & 0x1F;
                    g = (tempc[0] & 0x7E0) >> 5;
                    r = (tempc[0] & 0xF800) >> 11;
                    tempcolor[0] = new( (byte)(r << 3 | r >> 2), (byte)(g << 2 | g >> 3), (byte)(b << 3 | b >> 2) );
					
                    b = tempc[1] & 0x1F;
                    g = (tempc[1] & 0x7E0) >> 5;
                    r = (tempc[1] & 0xF800) >> 11;
					
                    tempcolor[1] = new( (byte)(r << 3 | r >> 2), (byte)(g << 2 | g >> 3), (byte)(b << 3 | b >> 2));
                    tempcolor[2] = new ( (byte)(((tempcolor[0].Red << 1) + tempcolor[1].Red + 1) / 3), (byte)(((tempcolor[0].Green << 1) + tempcolor[1].Green + 1) / 3), (byte)(((tempcolor[0].Blue << 1) + tempcolor[1].Blue + 1) / 3));
                    tempcolor[3] = new( (byte)((tempcolor[0].Red + (tempcolor[1].Red << 1) + 1) / 3), (byte)((tempcolor[0].Green + (tempcolor[1].Green << 1) + 1) / 3), (byte)((tempcolor[0].Blue + (tempcolor[1].Blue << 1) + 1) / 3));
                    
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
                    
                    for (int i = 0; i < 16; i++)
                    {
                        pixels[GetIndex(pixelOffset + Order[i], minwh, mink, bigwidth, newwidth)] = color[i];
                    }
                    pixelOffset += 16;
                }
            }
			
			image.SetPixels(pixels);
			
            if (t)
            {
                SexyBitmap image2 = image.Cut(0, 0, width, height);
                image.Dispose();
				
                return image2;
            }
			
            return image;
        }

       private static int GetIndex(int i, int min, int k, bool bw, int width)
        {
            int x, y;
            int mx = 0, my = 0;
            for (int j = 0; j < 16; j++)
            {
                mx |= (i & (1 << (j << 1))) >> j;
                my |= (i & ((1 << (j << 1)) << 1)) >> j;
            }
            my >>= 1;
            if (bw)
            {
                int j = i >> (2 * k) << (2 * k) | (my & (min - 1)) << k | (mx & (min - 1)) << 0;
                x = j / min;
                y = j % min;
            }
            else
            {
                int j = i >> (2 * k) << (2 * k) | (mx & (min - 1)) << k | (my & (min - 1)) << 0;
                x = j % min;
                y = j / min;
            }
			
            return (y * width) + x;
        }

       private static byte C(int v)
        {
            if (v >= 255) return 255;
            if (v <= 0) return 0;
            return (byte)v;
        }

        public static int Write(BinaryStream bs, SexyBitmap image, Endian endian = default)
        {
            int ans = image.Width;
            bool t = false;
            int newwidth = image.Width;
            int newheight = image.Height;
            if ((newwidth & (newwidth - 1)) != 0)
            {
                newwidth = 0b10 << ((int)Math.Floor(Math.Log2(newwidth)));
                t = true;
            }
            if ((newheight & (newheight - 1)) != 0)
            {
                newheight = 0b10 << ((int)Math.Floor(Math.Log2(newheight)));
                t = true;
            }
            if (t)
            {
                SexyBitmap image2 = SexyBitmap.CreateNew(newwidth, newheight);
                image.MoveTo(image2, 0, 0);
                image = image2;
            }
            var pixels = image.GetPixels();
            var temp = new ushort[4];
            var color = new TextureColor[16];
            byte maxalpha, minalpha;
            TextureColor min, max;
            int result;
            int tempvalue;
            int pixelOffset = 0;
            int minwh = newwidth > newheight ? newheight : newwidth;
            int mink = (int)Math.Log(minwh, 2);
            bool bigwidth = newwidth > newheight;
            for (int i = 0; i < newheight; i += 4)
            {
                for (int w = 0; w < newwidth; w += 4)
                {
                    maxalpha = 0;
                    minalpha = 255;
                    
                    for (int n = 0; n < 16; n++)
                    {
                        color[n] = pixels[GetIndex(pixelOffset + Order[n], minwh, mink, bigwidth, newwidth)];
                        byte a = color[n].Alpha;
                        if (a > maxalpha) maxalpha = a;
                        if (a < minalpha) minalpha = a;
                    }
                    pixelOffset += 16;
                    
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
                        long flag = 0;
                        int pos = 0;
                        for (int ii = 0; ii < 16; ii++)
                        {
                            flag |= ((long)alphabytes[ii]) << pos;
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
                    
                    DXTEncode.GetMinMaxColorsByEuclideanDistance(color, out min, out max);
                    result = DXTEncode.EmitColorIndices(color, min, max);
                    
                    bs.WriteUShort(DXTEncode.ColorTo565(max), endian);
                    bs.WriteUShort(DXTEncode.ColorTo565(min), endian);
                    bs.WriteUShort( (ushort)(result & 0xFFFF), endian);
                    bs.WriteUShort( (ushort)(result >> 16), endian);
                }
            }
			
            if (t)
            {
                image.Dispose();
            }
			
            return ans;
        }
    }