// https://github.com/powervr-graphics/Native_SDK/blob/master/framework/PVRCore/texture/PVRTDecompress.cpp

using System;

public static class PVRTCDecode
{
/** <summary>  Get Color A From ColorData </summary>

<param name="ColorData"></param>

 <returns></returns> */
	
private static TextureColor GetColorA(uint ColorData)
    {
        TextureColor color;
        if ((ColorData & 0x8000) != 0)
        {
            color.Red = (byte)((ColorData & 0x7c00) >> 10);
            color.Green = (byte)((ColorData & 0x3e0) >> 5);
            color.Blue = (byte)((ColorData & 0x1e) | ((ColorData & 0x1e) >> 4));
            color.Alpha = (byte)0xf;
        }
        else
        {
            color.Red = (byte)(((ColorData & 0xf00) >> 7) | ((ColorData & 0xf00) >> 11));
            color.Green = (byte)(((ColorData & 0xf0) >> 3) | ((ColorData & 0xf0) >> 7));
            color.Blue = (byte)(((ColorData & 0xe) << 1) | ((ColorData & 0xe) >> 2));
            color.Alpha = (byte)((ColorData & 0x7000) >> 11);
        }
        return color;
    }

    /// <summary>
    /// Get Color B From ColorData
    /// </summary>
    /// <param name="ColorData"></param>
    /// <returns></returns>
	
private static TextureColor GetColorB(uint ColorData)
    {
        TextureColor color;
        if ((ColorData & 0x80000000) != 0)
        {
            color.Red = (byte)((ColorData & 0x7c000000) >> 26);
            color.Green = (byte)((ColorData & 0x3e00000) >> 21);
            color.Blue = (byte)((ColorData & 0x1f0000) >> 16);
            color.Alpha = (byte)0xf;
        }
        else
        {
            color.Red = (byte)(((ColorData & 0xf000000) >> 23) | ((ColorData & 0xf000000) >> 27));
            color.Green = (byte)(((ColorData & 0xf00000) >> 19) | ((ColorData & 0xf00000) >> 23));
            color.Blue = (byte)(((ColorData & 0xf0000) >> 15) | ((ColorData & 0xf0000) >> 19));
            color.Alpha = (byte)((ColorData & 0x70000000) >> 27);
        }
        return color;
    }

    /// <summary>
    /// Interpolate 4*4 colors by 4 color in PVRTC
    /// </summary>
    /// <param name="P"></param>
    /// <param name="Q"></param>
    /// <param name="R"></param>
    /// <param name="S"></param>
    /// <param name="pPixel"></param>
    /// <param name="bpp"></param>
	
private static void InterpolateColors(TextureColor P, TextureColor Q, TextureColor R, TextureColor S, 
Pixel128S[] pPixel, byte bpp)
    {
        int WordWidth = bpp == 2 ? 8 : 4;
        int WordHeight = 4;
        Pixel128S hP = new Pixel128S
        {
            red = P.Red,
            green = P.Green,
            blue = P.Blue,
            alpha = P.Alpha
        };
        Pixel128S hQ = new Pixel128S
        {
            red = Q.Red,
            green = Q.Green,
            blue = Q.Blue,
            alpha = Q.Alpha
        };
        Pixel128S hR = new Pixel128S
        {
            red = R.Red,
            green = R.Green,
            blue = R.Blue,
            alpha = R.Alpha
        };
        Pixel128S hS = new Pixel128S
        {
            red = S.Red,
            green = S.Green,
            blue = S.Blue,
            alpha = S.Alpha
        };
        Pixel128S QminusP = new Pixel128S
        {
            red = hQ.red - hP.red,
            green = hQ.green - hP.green,
            blue = hQ.blue - hP.blue,
            alpha = hQ.alpha - hP.alpha
        };
        Pixel128S SminusR = new Pixel128S
        {
            red = hS.red - hR.red,
            green = hS.green - hR.green,
            blue = hS.blue - hR.blue,
            alpha = hS.alpha - hR.alpha
        };
        hP.red *= WordWidth;
        hP.green *= WordWidth;
        hP.blue *= WordWidth;
        hP.alpha *= WordWidth;
        hR.red *= WordWidth;
        hR.green *= WordWidth;
        hR.blue *= WordWidth;
        hR.alpha *= WordWidth;
        if (bpp == 2)
        {
            for (int x = 0; x < WordWidth; x++)
            {
                Pixel128S result = new Pixel128S
                {
                    red = 4 * hP.red,
                    green = 4 * hP.green,
                    blue = 4 * hP.blue,
                    alpha = 4 * hP.alpha
                };
                Pixel128S dY = new Pixel128S
                {
                    red = hR.red - hP.red,
                    green = hR.green - hP.green,
                    blue = hR.blue - hP.blue,
                    alpha = hR.alpha - hP.alpha
                };
                for (int y = 0; y < WordHeight; y++)
                {
                    pPixel[y * WordWidth + x].red = (result.red >> 7) + (result.red >> 2);
                    pPixel[y * WordWidth + x].green = (result.green >> 7) + (result.green >> 2);
                    pPixel[y * WordWidth + x].blue = (result.blue >> 7) + (result.blue >> 2);
                    pPixel[y * WordWidth + x].alpha = (result.alpha >> 5) + (result.alpha >> 1);
                    result.red += dY.red;
                    result.green += dY.green;
                    result.blue += dY.blue;
                    result.alpha += dY.alpha;
                }
                hP.red += QminusP.red;
                hP.green += QminusP.green;
                hP.blue += QminusP.blue;
                hP.alpha += QminusP.alpha;
                hR.red += SminusR.red;
                hR.green += SminusR.green;
                hR.blue += SminusR.blue;
                hR.alpha += SminusR.alpha;
            }
        }
        else
        {
            for (int y = 0; y < WordHeight; y++)
            {
                Pixel128S result = new Pixel128S
                {
                    red = 4 * hP.red,
                    green = 4 * hP.green,
                    blue = 4 * hP.blue,
                    alpha = 4 * hP.alpha
                };
                Pixel128S dY = new Pixel128S
                {
                    red = hR.red - hP.red,
                    green = hR.green - hP.green,
                    blue = hR.blue - hP.blue,
                    alpha = hR.alpha - hP.alpha
                };
                for (int x = 0; x < WordWidth; x++)
                {
                    pPixel[y * WordWidth + x].red = (result.red >> 6) + (result.red >> 1);
                    pPixel[y * WordWidth + x].green = (result.green >> 6) + (result.green >> 1);
                    pPixel[y * WordWidth + x].blue = (result.blue >> 6) + (result.blue >> 1);
                    pPixel[y * WordWidth + x].alpha = (result.alpha >> 4) + result.alpha;
                    result.red += dY.red;
                    result.green += dY.green;
                    result.blue += dY.blue;
                    result.alpha += dY.alpha;
                }
                hP.red += QminusP.red;
                hP.green += QminusP.green;
                hP.blue += QminusP.blue;
                hP.alpha += QminusP.alpha;
                hR.red += SminusR.red;
                hR.green += SminusR.green;
                hR.blue += SminusR.blue;
                hR.alpha += SminusR.alpha;
            }
        }
    }
 /// <summary>
/// Get color index for linear interpolation
/// </summary>
/// <param name="ModulationValues"></param>
/// <param name="ModulationModes"></param>
/// <param name="xPos"></param>
/// <param name="yPos"></param>
/// <param name="bpp"></param>
/// <returns></returns>
static int GetModulationValues(int[][] ModulationValues, int[][] ModulationModes, uint xPos, uint yPos, byte bpp)
{
    if (bpp == 2)
    {
        if (ModulationModes[xPos][yPos] == 0)
        {
            return RepVals0[ModulationValues[xPos][yPos]];
        }
        else
        {
            if (((xPos ^ yPos) & 1) == 0)
            {
                return RepVals0[ModulationValues[xPos][yPos]];
            }
            else if (ModulationModes[xPos][yPos] == 1)
            {
                return (RepVals0[ModulationValues[xPos][yPos - 1]] + RepVals0[ModulationValues[xPos][yPos + 1]] + 
                        RepVals0[ModulationValues[xPos - 1][yPos]] + RepVals0[ModulationValues[xPos + 1][yPos]] + 2) >> 2;
            }
            else if (ModulationModes[xPos][yPos] == 2)
            {
                return (RepVals0[ModulationValues[xPos - 1][yPos]] + RepVals0[ModulationValues[xPos + 1][yPos]] + 1) >> 1;
            }
            else
            {
                return (RepVals0[ModulationValues[xPos][yPos - 1]] + RepVals0[ModulationValues[xPos][yPos + 1]] + 1) >> 1;
            }
        }
    }
    else if (bpp == 4)
    {
        return ModulationValues[xPos][yPos];
    }
    return 0;
}



    /// <summary>
    /// Get PVRTC Modulation Values
    /// </summary>
    /// <param name="word"></param>
    /// <param name="offsetX"></param>
    /// <param name="offsetY"></param>
    /// <param name="ModulationValues"></param>
    /// <param name="ModulationModes"></param>
    /// <param name="bpp"></param>
	
   private static void GetModulationValues(PVRTCWord word, int offsetX, int offsetY, out uint ModulationValues, 
   out uint ModulationModes, int bpp)
    {
        uint WordModMode = word.ModulationData & 1;
        if (bpp == 2)
        {
            if (WordModMode != 0)
            {
                uint Offsets = (uint)(2 * (offsetX & 3) + 8 * (offsetY & 3));
                ModulationModes = 1;
                ModulationValues = (word.ModulationData >> (int)Offsets) & 3;
            }
            else
            {
                uint Offsets = (uint)((offsetX & 3) / 2 + 4 * (offsetY & 3));
                ModulationModes = 0;
                ModulationValues = (word.ModulationData >> (int)Offsets) & 1;
            }
        }
        else
        {
            ModulationModes = (word.ModulationData >> 1) & 1;
            if (ModulationModes != 0)
            {
                uint Offsets = (uint)(4 * (offsetX & 3) + 16 * (offsetY & 3));
                ModulationValues = (word.ModulationData >> (int)Offsets) & 15;
            }
            else
            {
                uint Offsets = (uint)(4 * (offsetX & 3) + 16 * (offsetY & 3));
                ModulationValues = (word.ModulationData >> (int)Offsets) & 3;
            }
        }
    }

/// <summary>
/// Get ModulationValues from PVRTCWord
/// </summary>
/// <param name="word"></param>
/// <param name="offsetX"></param>
/// <param name="offsetY"></param>
/// <param name="ModulationValues"></param>
/// <param name="ModulationModes"></param>
/// <param name="bpp"></param>
static void UnpackModulations(PVRTCWord word, int offsetX, int offsetY, int[][] ModulationValues, int[][] ModulationModes, byte bpp)
{
    uint WordModMode = word.ColorData & 0x1;
    uint ModulationBits = word.ModulationData;
    if (bpp == 2)
    {
        if (WordModMode != 0)
        {
            if ((ModulationBits & 0x1) != 0)
            {
                if ((ModulationBits & (0x1 << 20)) != 0)
                {
                    WordModMode = 3;
                }
                else
                {
                    WordModMode = 2;
                }

                if ((ModulationBits & (0x1 << 21)) != 0)
                {
                    ModulationBits |= (0x1 << 20);
                }
                else
                {
                    ModulationBits &= ~((uint)0x1 << 20);
                }
            }

            if ((ModulationBits & 0x2) != 0)
            {
                ModulationBits |= 0x1;
            }
            else
            {
                ModulationBits &= ~(uint)0x1;
            }

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    ModulationModes[x + offsetX][y + offsetY] = (int)WordModMode;

                    if (((x ^ y) & 1) == 0)
                    {
                        ModulationValues[x + offsetX][y + offsetY] = (int)(ModulationBits & 3);
                        ModulationBits >>= 2;
                    }
                }
            }
        }
        else
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    ModulationModes[x + offsetX][y + offsetY] = (int)WordModMode;

                    if ((ModulationBits & 1) != 0)
                    {
                        ModulationValues[x + offsetX][y + offsetY] = 0x3;
                    }
                    else
                    {
                        ModulationValues[x + offsetX][y + offsetY] = 0x0;
                    }
                    ModulationBits >>= 1;
                }
            }
        }
    }
    else
    {
        if (WordModMode != 0)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    ModulationValues[y + offsetY][x + offsetX] = (int)(ModulationBits & 3);
                    if (ModulationValues[y + offsetY][x + offsetX] == 1)
                    {
                        ModulationValues[y + offsetY][x + offsetX] = 4;
                    }
                    else if (ModulationValues[y + offsetY][x + offsetX] == 2)
                    {
                        ModulationValues[y + offsetY][x + offsetX] = 14;
                    }
                    else if (ModulationValues[y + offsetY][x + offsetX] == 3)
                    {
                        ModulationValues[y + offsetY][x + offsetX] = 8;
                    }
                    ModulationBits >>= 2;
                }
            }
        }
        else
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    ModulationValues[y + offsetY][x + offsetX] = (int)(ModulationBits & 3);
                    ModulationValues[y + offsetY][x + offsetX] *= 3;
                    if (ModulationValues[y + offsetY][x + offsetX] > 3) { ModulationValues[y + offsetY][x + offsetX] -= 1; }
                    ModulationBits >>= 2;
                }
            }
        }
    }
}

static readonly int[] RepVals0 = { 0, 3, 5, 8 };


    static void PvrtcGetDecompressedPixels(PVRTCWord P, PVRTCWord Q, PVRTCWord R, PVRTCWord S, TextureColor[] pColorData,
    byte bpp, int[][] ModulationValues, int[][] ModulationModes, Pixel128S[] upscaledColorA, Pixel128S[] upscaledColorB)
{
    uint WordWidth = bpp == 2 ? 8u : 4u;
    uint WordHeight = 4;
    UnpackModulations(P, 0, 0, ModulationValues, ModulationModes, bpp);
    UnpackModulations(Q, (int)WordWidth, 0, ModulationValues, ModulationModes, bpp);
    UnpackModulations(R, 0, (int)WordHeight, ModulationValues, ModulationModes, bpp);
    UnpackModulations(S, (int)WordWidth, (int)WordHeight, ModulationValues, ModulationModes, bpp);
    InterpolateColors(GetColorA(P.ColorData), GetColorA(Q.ColorData), GetColorA(R.ColorData), GetColorA(S.ColorData), upscaledColorA, bpp);
    InterpolateColors(GetColorB(P.ColorData), GetColorB(Q.ColorData), GetColorB(R.ColorData), GetColorB(S.ColorData), upscaledColorB, bpp);
    for (uint y = 0; y < WordHeight; y++)
    {
        for (uint x = 0; x < WordWidth; x++)
        {
            int mod = GetModulationValues(ModulationValues, ModulationModes, x + WordWidth / 2, y + WordHeight / 2, bpp);
            bool punchthroughAlpha = false;
            if (mod > 10)
            {
                punchthroughAlpha = true;
                mod -= 10;
            }
            Pixel128S result = new Pixel128S();
            uint yWordWidthx = y * WordWidth + x;
            result.red = (upscaledColorA[yWordWidthx].red * (8 - mod) + upscaledColorB[yWordWidthx].red * mod) / 8;
            result.green = (upscaledColorA[yWordWidthx].green * (8 - mod) + upscaledColorB[yWordWidthx].green * mod) / 8;
            result.blue = (upscaledColorA[yWordWidthx].blue * (8 - mod) + upscaledColorB[yWordWidthx].blue * mod) / 8;
            if (punchthroughAlpha)
            {
                result.alpha = 0;
            }
            else
            {
                result.alpha = (upscaledColorA[yWordWidthx].alpha * (8 - mod) + upscaledColorB[yWordWidthx].alpha * mod) / 8;
            }
            if (bpp == 2)
            {
                pColorData[yWordWidthx].Red = (byte)result.red;
                pColorData[yWordWidthx].Green = (byte)result.green;
                pColorData[yWordWidthx].Blue = (byte)result.blue;
                pColorData[yWordWidthx].Alpha = (byte)result.alpha;
            }
            else if (bpp == 4)
            {
                uint index = y + x * WordHeight;
                pColorData[index].Red = (byte)result.red;
                pColorData[index].Green = (byte)result.green;
                pColorData[index].Blue = (byte)result.blue;
                pColorData[index].Alpha = (byte)result.alpha;
            }
        }
    }
}

        /// <summary>
        /// Get the index of PVRTC words
        /// </summary>
        /// <param name="numWords"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        static uint WrapWordIndex(uint numWords, int word)
        {
            return (uint)((word + numWords) % numWords);
        }

                static uint TwiddleUV(uint XSize, uint YSize, uint XPos, uint YPos)
        {
            uint MinDimension = XSize;
            uint MaxValue = YPos;
            uint Twiddled = 0;
            uint SrcBitPos = 1;
            uint DstBitPos = 1;
            int ShiftCount = 0;
            if (YSize < XSize)
            {
                MinDimension = YSize;
                MaxValue = XPos;
            }
            while (SrcBitPos < MinDimension)
            {
                if ((YPos & SrcBitPos) != 0)
                {
                    Twiddled |= DstBitPos;
                }
                if ((XPos & SrcBitPos) != 0)
                {
                    Twiddled |= DstBitPos << 1;
                }
                SrcBitPos <<= 1;
                DstBitPos <<= 2;
                ShiftCount++;
            }
            MaxValue >>= ShiftCount;
            Twiddled |= MaxValue << (ShiftCount << 1);
            
            return Twiddled;
        }


    public static void DecompressPVRTC(byte[] pCompressedData, TextureColor[] pDecompressedData, uint Width,
    uint Height, byte bpp)
{
    uint WordWidth = bpp == 2 ? 8u : 4u;
    uint WordHeight = 4;
    int NumXWords = (int)(Width / WordWidth);
    int NumYWords = (int)(Height / WordHeight);

    uint[] pWordMembers = new uint[pCompressedData.Length / 4];
    Buffer.BlockCopy(pCompressedData, 0, pWordMembers, 0, pCompressedData.Length);

    TextureColor[] pPixels = new TextureColor[WordWidth * WordHeight];
    uint[] WordOffsets = new uint[4];

    // Alloc Memory

    int[][] ModulationValues = new int[16][];
    int[][] ModulationModes = new int[16][];

    for (int i = 0; i < 16; i++)
    {
        ModulationValues[i] = new int[8];
        ModulationModes[i] = new int[8];
    }

    Pixel128S[] upscaledColorA = new Pixel128S[32];
    Pixel128S[] upscaledColorB = new Pixel128S[32];

    // Decompress
    for (int wordY = -1; wordY < NumYWords - 1; wordY++)
    {
        for (int wordX = -1; wordX < NumXWords - 1; wordX++)
        {
            PVRTCWordIndices indices = new PVRTCWordIndices
            {
                P = new int[] { (int)WrapWordIndex((uint)NumXWords, wordX), (int)WrapWordIndex((uint)NumYWords, wordY) },
                Q = new int[] { (int)WrapWordIndex((uint)NumXWords, wordX + 1), (int)WrapWordIndex((uint)NumYWords, wordY) },
                R = new int[] { (int)WrapWordIndex((uint)NumXWords, wordX), (int)WrapWordIndex((uint)NumYWords, wordY + 1) },
                S = new int[] { (int)WrapWordIndex((uint)NumXWords, wordX + 1), (int)WrapWordIndex((uint)NumYWords, wordY + 1) }
            };

            WordOffsets[0] = TwiddleUV((uint)NumXWords, (uint)NumYWords, (uint)indices.P[0], (uint)indices.P[1]) << 1;
            WordOffsets[1] = TwiddleUV((uint)NumXWords, (uint)NumYWords, (uint)indices.Q[0], (uint)indices.Q[1]) << 1;
            WordOffsets[2] = TwiddleUV((uint)NumXWords, (uint)NumYWords, (uint)indices.R[0], (uint)indices.R[1]) << 1;
            WordOffsets[3] = TwiddleUV((uint)NumXWords, (uint)NumYWords, (uint)indices.S[0], (uint)indices.S[1]) << 1;

            PVRTCWord P = new PVRTCWord
            {
                ColorData = pWordMembers[WordOffsets[0] + 1],
                ModulationData = pWordMembers[WordOffsets[0]]
            };
            PVRTCWord Q = new PVRTCWord
            {
                ColorData = pWordMembers[WordOffsets[1] + 1],
                ModulationData = pWordMembers[WordOffsets[1]]
            };
            PVRTCWord R = new PVRTCWord
            {
                ColorData = pWordMembers[WordOffsets[2] + 1],
                ModulationData = pWordMembers[WordOffsets[2]]
            };
            PVRTCWord S = new PVRTCWord
            {
                ColorData = pWordMembers[WordOffsets[3] + 1],
                ModulationData = pWordMembers[WordOffsets[3]]
  
            };

        }

    }

}


    /// <summary>
    /// Decode a PVRTC Texture to RGBA
    /// </summary>
    /// <param name="pDecompressedData">result data</param>
    /// <param name="pCompressedData">PVRTC data</param>
    /// <param name="nWidth">Width of texture</param>
    /// <param name="nHeight">Height of texture</param>
    /// <param name="nBPP">Bits Per Pixel of texture</param>
    /// <param name="Is2bpp">if this is PVRTC2</param>
	
    public static void DecompressPVRTC(TextureColor[] pDecompressedData, PVRTCWord[] pCompressedData, uint nWidth,
    uint nHeight, byte nBPP)
    {
        int WordWidth = nBPP == 2 ? 8 : 4;
        int WordHeight = 4;
        int XBlockSize = (int) (nWidth / WordWidth);
        int YBlockSize = (int) (nHeight / WordHeight);
        Pixel128S[] pPixels = new Pixel128S[WordWidth * WordHeight];

        for (int y = 0; y < YBlockSize; y++)
        {
            for (int x = 0; x < XBlockSize; x++)
            {
                PVRTCWordIndices wordsIndices = new PVRTCWordIndices
                {
                    P = new int[2] { (x - 1 + XBlockSize) % XBlockSize, (y - 1 + YBlockSize) % YBlockSize },
                    Q = new int[2] { x, (y - 1 + YBlockSize) % YBlockSize },
                    R = new int[2] { (x - 1 + XBlockSize) % XBlockSize, y },
                    S = new int[2] { x, y }
                };

                PVRTCWord PWord = pCompressedData[wordsIndices.P[1] * XBlockSize + wordsIndices.P[0]];
                PVRTCWord QWord = pCompressedData[wordsIndices.Q[1] * XBlockSize + wordsIndices.Q[0]];
                PVRTCWord RWord = pCompressedData[wordsIndices.R[1] * XBlockSize + wordsIndices.R[0]];
                PVRTCWord SWord = pCompressedData[wordsIndices.S[1] * XBlockSize + wordsIndices.S[0]];

                TextureColor PColor = GetColorA(PWord.ColorData);
                TextureColor QColor = GetColorA(QWord.ColorData);
                TextureColor RColor = GetColorA(RWord.ColorData);
                TextureColor SColor = GetColorA(SWord.ColorData);

                InterpolateColors(PColor, QColor, RColor, SColor, pPixels, nBPP);

                for (int yOffset = 0; yOffset < WordHeight; yOffset++)
                {
                    for (int xOffset = 0; xOffset < WordWidth; xOffset++)
                    {
                        GetModulationValues(SWord, xOffset, yOffset, out uint ModulationValue, out uint ModulationMode, nBPP);

                        Pixel128S result;
                        if (ModulationMode != 0)
                        {
                            result = new Pixel128S
                            {
                                red = (pPixels[yOffset * WordWidth + xOffset].red * (int)ModulationValue) >> 2,
                                green = (pPixels[yOffset * WordWidth + xOffset].green * (int)ModulationValue) >> 2,
                                blue = (pPixels[yOffset * WordWidth + xOffset].blue * (int)ModulationValue) >> 2,
                                alpha = (pPixels[yOffset * WordWidth + xOffset].alpha * (int)ModulationValue) >> 2
                            };
                        }
                        else
                        {
                            result = pPixels[yOffset * WordWidth + xOffset];
                        }

                        pDecompressedData[(y * WordHeight + yOffset) * nWidth + (x * WordWidth + xOffset)] = new TextureColor
                        {
                            Red = (byte)result.red,
                            Green = (byte)result.green,
                            Blue = (byte)result.blue,
                            Alpha = (byte)result.alpha
                        };
                    }
                }
            }
        }
    }

}