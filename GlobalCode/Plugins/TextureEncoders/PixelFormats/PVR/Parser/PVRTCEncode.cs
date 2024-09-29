public static class PVRTCEncode
{
        public static PvrTcPacket[] EncodeRGBA4Bpp(TextureColor[] colors, int width)
        {
            int blocks = width >> 2;
            int blockMask = blocks - 1;
            PvrTcPacket[] packets = new PvrTcPacket[(width * width) >> 4];

            for (int y = 0; y < blocks; y++)
            {
                for (int x = 0; x < blocks; x++)
                {
                    CalculateBoundingBox(colors, width, x, y, out PvrColor min, out PvrColor max);

                    PvrTcPacket packet = new();

                    packet.SetColorA_RGBA(min);
                    packet.SetColorB_RGBA(max);
                    packets[GetMortonNumber(x, y)] = packet;
                }
            }

            for (int y = 0; y < blocks; y++)
            {
                for (int x = 0; x < blocks; x++)
                {
                    byte[][] factorfather = PvrTcPacket.BILINEAR_FACTORS;
                    int factorindex = 0;
                    int dataindex = (y << 2) * width + (x << 2);
                    uint ModulationData = 0;

                    for (int py = 0; py < 4; py++)
                    {
                        int yOffset = (py < 2) ? -1 : 0;
                        int y0 = (y + yOffset) & blockMask;
                        int y1 = (y0 + 1) & blockMask;

                        for (int px = 0; px < 4; px++)
                        {
                            byte[] factor = factorfather[factorindex];
                            int xOffset = (px < 2) ? -1 : 0;
                            int x0 = (x + xOffset) & blockMask;
                            int x1 = (x0 + 1) & blockMask;

                            PvrTcPacket p0 = packets[GetMortonNumber(x0, y0)];
                            PvrTcPacket p1 = packets[GetMortonNumber(x1, y0)];
                            PvrTcPacket p2 = packets[GetMortonNumber(x0, y1)];
                            PvrTcPacket p3 = packets[GetMortonNumber(x1, y1)];

                            PvrColor_RGBA ca = p0.GetColorA_RGBA() * factor[0] +
                                           p1.GetColorA_RGBA() * factor[1] +
                                           p2.GetColorA_RGBA() * factor[2] +
                                           p3.GetColorA_RGBA() * factor[3];

                            PvrColor_RGBA cb = p0.GetColorB_RGBA() * factor[0] +
                                           p1.GetColorB_RGBA() * factor[1] +
                                           p2.GetColorB_RGBA() * factor[2] +
                                           p3.GetColorB_RGBA() * factor[3];

                            var pixel = colors[dataindex + py * width + px];
                            PvrColor_RGBA d = cb - ca;
                            PvrColor_RGBA p = new(pixel.Red << 4, pixel.Green << 4, pixel.Blue << 4, pixel.Alpha << 4);
                            PvrColor_RGBA v = p - ca;
                            int projection = (v % d) << 4;
                            int lengthSquared = d % d;

                            if (projection > 3 * lengthSquared) ModulationData++;
                            if (projection > 8 * lengthSquared) ModulationData++;
                            if (projection > 13 * lengthSquared) ModulationData++;

                            ModulationData = RotateRight(ModulationData, 2);
                            factorindex++;
                        }
                    }
                    packets[GetMortonNumber(x, y)].ModulationData = ModulationData;
                }
            }
            return packets;
        }

        public static PvrTcPacket[] EncodeRGB4Bpp(TextureColor[] colors, int width)
        {
            int blocks = width >> 2;
            int blockMask = blocks - 1;
            PvrTcPacket[] packets = new PvrTcPacket[(width * width) >> 4];

            for (int y = 0; y < blocks; y++)
            {
                for (int x = 0; x < blocks; x++)
                {
                    CalculateBoundingBox(colors, width, x, y, out PvrColor min, out PvrColor max);
                    PvrTcPacket packet = new PvrTcPacket
                    {
                        UsePunchThroughAlpha = false
                    };
                    packet.SetColorA_RGB(min);
                    packet.SetColorB_RGB(max);
                    packets[GetMortonNumber(x, y)] = packet;
                }
            }

            for (int y = 0; y < blocks; y++)
            {
                for (int x = 0; x < blocks; x++)
                {
                    byte[][] factorfather = PvrTcPacket.BILINEAR_FACTORS;
                    int factorindex = 0;
                    int dataindex = (y << 2) * width + (x << 2);
                    uint ModulationData = 0;

                    for (int py = 0; py < 4; py++)
                    {
                        int yOffset = (py < 2) ? -1 : 0;
                        int y0 = (y + yOffset) & blockMask;
                        int y1 = (y0 + 1) & blockMask;

                        for (int px = 0; px < 4; px++)
                        {
                            byte[] factor = factorfather[factorindex];
                            int xOffset = (px < 2) ? -1 : 0;
                            int x0 = (x + xOffset) & blockMask;
                            int x1 = (x0 + 1) & blockMask;

                            PvrTcPacket p0 = packets[GetMortonNumber(x0, y0)];
                            PvrTcPacket p1 = packets[GetMortonNumber(x1, y0)];
                            PvrTcPacket p2 = packets[GetMortonNumber(x0, y1)];
                            PvrTcPacket p3 = packets[GetMortonNumber(x1, y1)];

                            PvrColor_RGB ca = p0.GetColorA_RGB() * factor[0] +
                                          p1.GetColorA_RGB() * factor[1] +
                                          p2.GetColorA_RGB() * factor[2] +
                                          p3.GetColorA_RGB() * factor[3];

                            PvrColor_RGB cb = p0.GetColorB_RGB() * factor[0] +
                                          p1.GetColorB_RGB() * factor[1] +
                                          p2.GetColorB_RGB() * factor[2] +
                                          p3.GetColorB_RGB() * factor[3];

                            var pixel = colors[dataindex + py * width + px];
                            PvrColor_RGB d = cb - ca;
                            PvrColor_RGB p = new(pixel.Red << 4, pixel.Green << 4, pixel.Blue << 4);
                            PvrColor_RGB v = p - ca;
                            int projection = (v % d) << 4;
                            int lengthSquared = d % d;

                            if (projection > 3 * lengthSquared) ModulationData++;
                            if (projection > 8 * lengthSquared) ModulationData++;
                            if (projection > 13 * lengthSquared) ModulationData++;

                            ModulationData = RotateRight(ModulationData, 2);
                            factorindex++;
                        }
                    }
                    packets[GetMortonNumber(x, y)].ModulationData = ModulationData;
                }
            }
            return packets;
        }

private static void CalculateBoundingBox(TextureColor[] colors, int width, int blockX, int blockY, 
out PvrColor min, out PvrColor max)
        {
            byte maxr = 0, maxg = 0, maxb = 0, maxa = 0;
            byte minr = 255, ming = 255, minb = 255, mina = 255;
            int beginindex = (blockY << 2) * width + (blockX << 2);

            for (int i = 0; i < 4; i++)
            {
                int nindex = beginindex + i * width;

                for (int j = 0; j < 4; j++)
                {
                    int index = nindex + j;
                    byte temp;

                    temp = colors[index].Red;

                    if (temp > maxr) maxr = temp;
                    if (temp < minr) minr = temp;

                    temp = colors[index].Green;

                    if (temp > maxg) maxg = temp;
                    if (temp < ming) ming = temp;

                    temp = colors[index].Blue;

                    if (temp > maxb) maxb = temp;
                    if (temp < minb) minb = temp;

                    temp = colors[index].Alpha;

                    if (temp > maxa) maxa = temp;
                    if (temp < mina) mina = temp;
                }
            }
			
            min = new(minr, ming, minb, mina);
            max = new(maxr, maxg, maxb, maxa);
        }

        private static uint RotateRight(uint value, int shift)
        {
            return (value >> shift) | (value << (32 - shift));
        }

        private static int GetMortonNumber(int x, int y)
        {
            return (int)(((((x & 0x01) << 0) | ((x & 0x02) << 1) | ((x & 0x04) << 2) | ((x & 0x08) << 3) |
                            ((x & 0x10) << 4) | ((x & 0x20) << 5) | ((x & 0x40) << 6) | ((x & 0x80) << 7) |
                            ((x & 0x0100) << 8) | ((x & 0x0200) << 9) | ((x & 0x0400) << 10) | ((x & 0x0800) << 11) |
                            ((x & 0x1000) << 12) | ((x & 0x2000) << 13) | ((x & 0x4000) << 14) | ((x & 0x8000) << 15) |
                            ((y & 0x01) << 1) | ((y & 0x02) << 2) | ((y & 0x04) << 3) | ((y & 0x08) << 4) |
                            ((y & 0x10) << 5) | ((y & 0x20) << 6) | ((y & 0x40) << 7) | ((y & 0x80) << 8) |
                            ((y & 0x0100) << 9) | ((y & 0x0200) << 10) | ((y & 0x0400) << 11) | ((y & 0x0800) << 12) |
                            ((y & 0x1000) << 13) | ((y & 0x2000) << 14) | ((y & 0x4000) << 15) | ((y & 0x8000) << 16)) >> 1) );
        }
    }