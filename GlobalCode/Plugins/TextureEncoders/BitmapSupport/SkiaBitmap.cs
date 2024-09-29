using SkiaSharp;
using System;
using System.IO;
using System.Runtime.InteropServices;

/// <summary> It is slower than GDIBitmap (SkiaSharp is also unsafe, but allows Cross-platform Support) </summary>

public class SkiaBitmap : SexyBitmap
{
public override int Width => m_width;

public override int Height => m_height;

private readonly SKBitmap m_bitmap;

private readonly int m_width;

private readonly int m_height;

public static SKImageInfo GetDefaultImgInfo(int width, int height) => new()
{
ColorType = SKColorType.Bgra8888,
AlphaType = SKAlphaType.Unpremul,
ColorSpace = null,
Width = width,
Height = height
};

public static SKPngEncoderOptions DefaultEncoderOptions => new() { ZLibLevel = 1 };

public SkiaBitmap()
{
}

public SkiaBitmap(int width, int height)
{
m_bitmap = new(width, height, SKColorType.Bgra8888, SKAlphaType.Unpremul);

m_width = m_bitmap.Width;
m_height = m_bitmap.Height;
}

public SkiaBitmap(Stream stream)
{
using SKCodec sKCodec = SKCodec.Create(stream);

var imgInfo = GetDefaultImgInfo(sKCodec.Info.Width, sKCodec.Info.Height);
m_bitmap = SKBitmap.Decode(sKCodec, imgInfo);
            
m_width = m_bitmap.Width;
m_height = m_bitmap.Height;
}

public SkiaBitmap(Stream stream, SKImageInfo imgInfo)
{
using SKCodec sKCodec = SKCodec.Create(stream);
          
m_bitmap = SKBitmap.Decode(sKCodec, imgInfo);
            
m_width = m_bitmap.Width;
m_height = m_bitmap.Height;
}

public SkiaBitmap(string filePath)
{
using SKCodec sKCodec = SKCodec.Create(filePath);

var imgInfo = GetDefaultImgInfo(sKCodec.Info.Width, sKCodec.Info.Height);
m_bitmap = SKBitmap.Decode(sKCodec, imgInfo);
         
m_width = m_bitmap.Width;
m_height = m_bitmap.Height;
}

public SkiaBitmap(string filePath, SKImageInfo imgInfo)
{
using SKCodec sKCodec = SKCodec.Create(filePath);
            
m_bitmap = SKBitmap.Decode(sKCodec, imgInfo);
         
m_width = m_bitmap.Width;
m_height = m_bitmap.Height;
}

protected override SexyBitmap Create(int width, int height) => new SkiaBitmap(width, height);

protected override SexyBitmap Create(Stream stream) => new SkiaBitmap(stream);

protected override SexyBitmap Create(string filePath) => new SkiaBitmap(filePath);

// Get Pixels from SKBitmap

public override TextureColor[] GetPixels()
{
TextureColor[] pixels = new TextureColor[m_width * m_height];
var skColor = m_bitmap.Pixels;

for(int i = 0; i < skColor.Length; i++)
pixels[i] = new(skColor[i].Red, skColor[i].Green, skColor[i].Blue, skColor[i].Alpha);

return pixels;
}

// Set Pixels to SKBitmap

public override void SetPixels(TextureColor[] iPixels)
{

if(iPixels.Length != m_width * m_height)
throw new ArgumentException($"Pixels ({iPixels.Length}) must match Image Size ({m_width}x{m_height})", nameof(iPixels) );

var skColor = m_bitmap.Pixels;

for(int i = 0; i < iPixels.Length; i++)
skColor[i] = new(iPixels[i].Red, iPixels[i].Green, iPixels[i].Blue, iPixels[i].Alpha);

m_bitmap.Pixels = skColor;
}

public void Save(string filePath, SKPngEncoderOptions encoderCfg)
{
using SKPixmap sKPixmap = m_bitmap.PeekPixels();
using SKData p = sKPixmap?.Encode(encoderCfg);
            
using FileStream fs = new(filePath, FileMode.Create);        
byte[] t = p.ToArray();

fs.Write(t, 0, t.Length);
}

public override void Save(string filePath) => Save(filePath, DefaultEncoderOptions);

public void Save(Stream stream, SKPngEncoderOptions encoderCfg)
{
using SKPixmap sKPixmap = m_bitmap.PeekPixels();
using SKData p = sKPixmap?.Encode(encoderCfg);
          
byte[] t = p.ToArray();
stream.Write(t, 0, t.Length);   
}

public override void Save(Stream stream) => Save(stream, DefaultEncoderOptions);

public override void Dispose() => m_bitmap?.Dispose();
}