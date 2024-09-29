using System;
using System.IO;

// Represents a Bitmap used for Manipulating Image Data. (Original Source is YFBitmap, extracted from PopStudio)

public abstract class SexyBitmap : IDisposable
{
internal static SexyBitmap InnerData;

public static void Regist<T>() where T : SexyBitmap, new() => InnerData = new T();

public static void Regist(object obj) => InnerData = (obj is SexyBitmap bitmap) ? bitmap : InnerData;

public static SexyBitmap CreateNew(int width, int height) => InnerData?.Create(width, height);

public static SexyBitmap CreateNew(Stream stream) => InnerData?.Create(stream);

public static SexyBitmap CreateNew(string filePath) => InnerData?.Create(filePath);

public abstract int Width{ get; }

public abstract int Height{ get; }

/** <summary> Gets the Pixels of an Image (must be BB GG RR AA) </summary>

<returns>The Pixels</returns> */

public abstract TextureColor[] GetPixels();

public abstract void SetPixels(TextureColor[] iPixels);

public abstract void Save(string filePath);

public abstract void Save(Stream stream);

protected abstract SexyBitmap Create(int width, int height);

protected abstract SexyBitmap Create(Stream stream);
		
protected abstract SexyBitmap Create(string filePath);
	
public abstract void Dispose();

public virtual int Square => Width * Height;

public virtual void MoveTo(SexyBitmap decimg, int mX, int mY)
{
TextureColor[] res = GetPixels();
TextureColor[] dec = decimg.GetPixels();

for(int y = 0; y < Height; y++)
{

for (int x = 0; x < Width; x++)
{

if ( ( (x + mX) >= decimg.Width) || ( (y + mY) >= decimg.Height) || ((x + mX) < 0) || ((y + mY) < 0) )
continue;
            
			
else
dec[ (y + mY) * decimg.Width + x + mX ] = res[y * Width + x];
        
}

}

SetPixels(dec);
}

public virtual SexyBitmap Cut(int mX, int mY, int mWidth, int mHeight)
{
SexyBitmap ans = Create(mWidth, mHeight);

    TextureColor[] res = GetPixels();
    TextureColor[] dec = ans.GetPixels();
	
    for (int y = 0; y < mHeight; y++)
    {
        for (int x = 0; x < mWidth; x++)
        {
            if (((x + mX) >= Width) || ((y + mY) >= Height) || ((x + mX) < 0) || ((y + mY) < 0))
            {
                dec[y * mWidth + x] = TextureColor.Default;
            }
            else
            {
                dec[y * mWidth + x] = res[ (y + mY) * Width + x + mX ];
            }
        }
    }
	
	ans.SetPixels(dec);
	
    return ans;
}

public virtual SexyBitmap Rotate0()
{
int resH = Height;
int resW = Width;

SexyBitmap N = Create(resW, resH);
TextureColor[] res = GetPixels();

TextureColor[] dec = N.GetPixels();
Array.Copy(res, dec, res.Length);

N.SetPixels(dec);

return N;
}

public virtual SexyBitmap Rotate90()
{
    int resH = Height;
    int resW = Width;
    SexyBitmap N = Create(resH, resW);
    TextureColor[] res = GetPixels();
    TextureColor[] dec = N.GetPixels();
	
    for (int y = 0; y < resH; y++)
    {
        for (int x = 0; x < resW; x++)
        {
            dec[x * resH + (resH - y - 1)] = res[y * resW + x];
        }
    }
	
	N.SetPixels(dec);
	
    return N;
}

public virtual SexyBitmap Rotate180()
{
    int resH = Height;
    int resW = Width;
    SexyBitmap N = Create(resW, resH);
    TextureColor[] res = GetPixels();
    TextureColor[] dec = N.GetPixels();
	
    for (int y = 0; y < resH; y++)
    {
        for (int x = 0; x < resW; x++)
        {
            dec[(resH - y - 1) * resW + (resW - x - 1)] = res[y * resW + x];
        }
    }
	
	N.SetPixels(dec);
	
    return N;
}

public virtual SexyBitmap Rotate270()
{
    int resH = Height;
    int resW = Width;
    SexyBitmap N = Create(resH, resW);
    TextureColor[] res = GetPixels();
    TextureColor[] dec = N.GetPixels();
    for (int y = 0; y < resH; y++)
    {
        for (int x = 0; x < resW; x++)
        {
            dec[(resW - x - 1) * resH + y] = res[y * resW + x];
        }
    }
	
	N.SetPixels(dec);
	
    return N;
}

}