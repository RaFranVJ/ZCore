using System;
using System.Runtime.InteropServices;

/// <summary> Handles the Drawing of Bitmaps on Different Platforms. </summary>

public static class ImageHelper
{
/// <summary> Registers the Bitmap implementation to use based on the Current Env. </summary>

public static void RegistPlatform()
{

if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) )
SexyBitmap.Regist<GDIBitmap>();
    
else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX) )
SexyBitmap.Regist<ImageBitmap>();
    
else
SexyBitmap.Regist<SkiaBitmap>(); 

}

}