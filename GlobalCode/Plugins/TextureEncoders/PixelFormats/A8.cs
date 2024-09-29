using System;

public static class A8
{
	
    public static SexyBitmap Read(BinaryStream bs, int width, int height)
    {
        SexyBitmap image = SexyBitmap.CreateNew(width, height);
        int S = width * height;
        TextureColor[] pixels = new TextureColor[S]; // Assuming 4 bytes per pixel (BGRA)

        for (int i = 0; i < S; i++)
        {
            pixels[i] = new TextureColor(255, 255, 255, bs.ReadByte() ); // r, g, b, a
        }

        image.SetPixels(pixels);
        return image;
    }

    public static int Write(BinaryStream bs, SexyBitmap image)
    {
        TextureColor[] pixels = image.GetPixels();
        int S = image.Square;

        for (int i = 0; i < S; i++)
        {
            bs.WriteByte(pixels[i].Alpha);
        }
        return image.Width;
    }
}
