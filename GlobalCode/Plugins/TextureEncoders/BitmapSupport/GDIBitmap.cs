using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

/// <summary> It's fast but just supports Windows in .NET 7 and later versions </summary>

#pragma warning disable CA1416 // Validate platform compatibility

public class GDIBitmap : SexyBitmap
{
public override int Width => m_width;

public override int Height => m_height;

private readonly Bitmap m_bitmap;

private readonly int m_width;

private readonly int m_height;

public GDIBitmap()
{ 
}

public GDIBitmap(int width, int height)
{
m_bitmap = new(width, height, PixelFormat.Format32bppArgb);

m_width = m_bitmap.Width;
m_height = m_bitmap.Height;
}

public GDIBitmap(Stream stream)
{
m_bitmap = new(Image.FromStream(stream) );
			
m_width = m_bitmap.Width;
m_height = m_bitmap.Height;
}

public GDIBitmap(string filePath)
{
m_bitmap = new(filePath);
			
m_width = m_bitmap.Width;
m_height = m_bitmap.Height;
}

protected override SexyBitmap Create(int width, int height) => new GDIBitmap(width, height);

protected override SexyBitmap Create(Stream stream) => new GDIBitmap(stream);

protected override SexyBitmap Create(string filePath) => new GDIBitmap(filePath);

// Get Pixels from Bitmap

public override TextureColor[] GetPixels()
{
var pixels = new TextureColor[m_width * m_height];

var data = m_bitmap.LockBits( new Rectangle(0, 0, m_width, m_height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

int stride = data.Stride;
byte[] byteArray = new byte[stride * m_height];

Marshal.Copy(data.Scan0, byteArray, 0, byteArray.Length);

for(int y = 0; y < m_height; y++)
{
	
for (int x = 0; x < m_width; x++)
{
int index = y * stride + x * 4;
pixels[y * m_width + x] = new(byteArray[index + 2], byteArray[index + 1], byteArray[index], byteArray[index + 3] );
}

}

m_bitmap.UnlockBits(data);
		
return pixels;
}

// Set Pixels to Bitmap

public override void SetPixels(TextureColor[] iPixels)
{

if(iPixels.Length != m_width * m_height)
throw new ArgumentException($"Pixels ({iPixels.Length}) must match Image Size ({m_width}x{m_height})", nameof(iPixels) );

var data = m_bitmap.LockBits(new Rectangle(0, 0, m_width, m_height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
int stride = data.Stride;

byte[] byteArray = new byte[stride * m_height];

for(int y = 0; y < m_height; y++)
{
	
for (int x = 0; x < m_width; x++)
{
int index = y * m_width + x;
int byteIndex = y * stride + x * 4;

byteArray[byteIndex] = iPixels[index].Blue;
byteArray[byteIndex + 1] = iPixels[index].Green;
byteArray[byteIndex + 2] = iPixels[index].Red;
byteArray[byteIndex + 3] = iPixels[index].Alpha;
}

}

Marshal.Copy(byteArray, 0, data.Scan0, byteArray.Length);
	
m_bitmap.UnlockBits(data);
}

public override void Save(string filePath) => m_bitmap.Save(filePath, ImageFormat.Png);

public override void Save(Stream stream) => m_bitmap.Save(stream, ImageFormat.Png);

public override void Dispose() => m_bitmap?.Dispose();

#pragma warning restore CA1416 // Validate platform compatibility
}