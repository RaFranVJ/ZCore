using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;

public class ImageBitmap : SexyBitmap
{
    public override int Width => m_width;
    public override int Height => m_height;

    private readonly Image<Bgra32> m_image;
    private readonly int m_width;
    private readonly int m_height;

    public static PngEncoder DefaultPngSettings => new()
        {
            FilterMethod = PngFilterMethod.None,
            CompressionLevel = PngCompressionLevel.Level1
        };

    public ImageBitmap()
    {
    }

    public ImageBitmap(int width, int height)
    {
        Configuration customConfig = Configuration.Default.Clone();
        customConfig.PreferContiguousImageBuffers = true;

        m_image = new Image<Bgra32>(customConfig, width, height);
		
        if (!m_image.DangerousTryGetSinglePixelMemory(out Memory<Bgra32> memory))
        throw new Exception("This can only happen with multi-GB images or when PreferContiguousImageBuffers is not set to true.");
        
        m_width = m_image.Width;
        m_height = m_image.Height;
    }

    public ImageBitmap(Stream stream, DecoderOptions options = null)
    {
        options ??= new();

        m_image = Image.Load<Bgra32>(options, stream);
        if (!m_image.DangerousTryGetSinglePixelMemory(out Memory<Bgra32> memory))
        {
            throw new Exception("This can only happen with multi-GB images or when PreferContiguousImageBuffers is not set to true.");
        }

        m_width = m_image.Width;
        m_height = m_image.Height;
    }

    public ImageBitmap(string filePath, DecoderOptions options = null)
    {
        options ??= new();

        m_image = Image.Load<Bgra32>(options, filePath);
        if (!m_image.DangerousTryGetSinglePixelMemory(out Memory<Bgra32> memory))
        {
            throw new Exception("This can only happen with multi-GB images or when PreferContiguousImageBuffers is not set to true.");
        }

        m_width = m_image.Width;
        m_height = m_image.Height;
    }

    protected override SexyBitmap Create(int width, int height) => new ImageBitmap(width, height);

    protected override SexyBitmap Create(Stream stream) => new ImageBitmap(stream);

    protected override SexyBitmap Create(string filePath) => new ImageBitmap(filePath);
	
	// Get Pixels 

    public override TextureColor[] GetPixels()
    {
        if (!m_image.DangerousTryGetSinglePixelMemory(out Memory<Bgra32> pixelMemory) )
        {
            throw new Exception("Unable to get Pixel Memory.");
        }

        Span<Bgra32> pixelSpan = pixelMemory.Span;
        var textureColors = new TextureColor[pixelSpan.Length];

        for (int i = 0; i < pixelSpan.Length; i++)
        {
            Bgra32 pixel = pixelSpan[i];
            textureColors[i] = new(pixel.R, pixel.G, pixel.B, pixel.A);
        }

        return textureColors;
    }

    public override void SetPixels(TextureColor[] iPixels)
    {
		
        if (iPixels.Length != m_width * m_height)
        throw new ArgumentException($"Pixels ({iPixels.Length}) must match Image Size ({m_width}x{m_height})", nameof(iPixels) );
      

        if (!m_image.DangerousTryGetSinglePixelMemory(out Memory<Bgra32> pixelMemory) )
        throw new Exception("Unable to get Pixel Memory.");
        

        Span<Bgra32> pixelSpan = pixelMemory.Span;

        for (int i = 0; i < iPixels.Length; i++)
        {
            TextureColor color = iPixels[i];
            pixelSpan[i] = new(color.Blue, color.Green, color.Red, color.Alpha);
        }
    }

    public void Save(string filePath, PngEncoder settings) => m_image.SaveAsPng(filePath, settings);
    
    public override void Save(string filePath) => Save(filePath, DefaultPngSettings);

    public void Save(Stream stream, PngEncoder settings) => m_image.SaveAsPng(stream, settings);
    
    public override void Save(Stream stream) => Save(stream, DefaultPngSettings);

    public override void Dispose() => m_image?.Dispose();
    
}
