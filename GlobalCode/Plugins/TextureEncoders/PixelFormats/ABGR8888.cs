using System;

// Error: Parsed Images become Blue

public static class ABGR8888
{
// Read Bitmap

public static SexyBitmap Read(BinaryStream bs, int width, int height, Endian endian = default)
{
SexyBitmap image = SexyBitmap.CreateNew(width, height);
int S = width * height;

TextureColor[] pixels = new TextureColor[S];
uint temp;

for (int i = 0; i < S; i++)
{
temp = bs.ReadUInt(endian);

pixels[i] = new( (byte)(temp & 0xFF), (byte)( (temp & 0xFF00) >> 8), (byte)( (temp & 0xFF0000) >> 16), (byte)(temp >> 24) );
}

image.SetPixels(pixels);

return image;
}

// Write pixels to Bitmap

public static int Write(BinaryStream bs, SexyBitmap image, Endian endian = default)
{
TextureColor[] pixels = image.GetPixels();
int S = image.Square;

for (int i = 0; i < S; i++)
bs.WriteUInt( ( (uint)pixels[i].Alpha << 24) | ( (uint)pixels[i].Blue << 16) | ( (uint)pixels[i].Green << 8) | pixels[i].Red, endian);

return image.Width << 2;
}

}