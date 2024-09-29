using System;
using System.Runtime.InteropServices;

[ StructLayout(LayoutKind.Explicit, Size = 4) ]

// Represents a Color in the BGRA format, mostly used for Images

public struct TextureColor
{
// Default Color
	 
public static readonly TextureColor Default = new(0, 0, 0, 0);

[FieldOffset(0)]
public byte Blue;

[FieldOffset(1)]
public byte Green;

[FieldOffset(2)]
public byte Red;

[FieldOffset(3)]
public byte Alpha;

public TextureColor(byte r, byte g, byte b, byte a)
{
this = default; // must be fully assigned before control is returned to the caller" error

Red = r;
Green = g;
Blue = b;
Alpha = a;
}

public TextureColor(byte r, byte g, byte b)
{
this = default; // must be fully assigned before control is returned to the caller" error
			
Red = r;
Green = g;
Blue = b;
Alpha = 255;
}

public TextureColor WithRed(byte red) => new(red, Green, Blue, Alpha);

public TextureColor WithGreen(byte green) => new(Red, green, Blue, Alpha);

public TextureColor WithBlue(byte blue) => new(Red, Green, blue, Alpha);

public TextureColor WithAlpha(byte alpha) => new(Red, Green, Blue, alpha);

public override string ToString() => $"#{Alpha:x2}{Red:x2}{Green:x2}{Blue:x2}";

public static implicit operator TextureColor(uint color)
{
byte[] bytes = BitConverter.GetBytes(color);
			
return new(bytes[2], bytes[1], bytes[0], bytes[3]);
}

public static explicit operator uint(TextureColor color)
{
byte[] bytes = new byte[] { color.Blue, color.Green, color.Red, color.Alpha };

return BitConverter.ToUInt32(bytes, 0);
}

}