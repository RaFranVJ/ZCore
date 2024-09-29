using System;

public static class PVRTC_2BPP_RGBA
{
	
        public static SexyBitmap Read(BinaryStream bs, int width, int height)
        {
            bool needsResize = false;
			
            int newWidth = width;
            int newHeight = height;

            if (newWidth < 16)
            {
                newWidth = 16;
                needsResize = true;
            }
            if (newHeight < 8)
            {
                newHeight = 8;
                needsResize = true;
            }
            if ((newWidth & (newWidth - 1)) != 0)
            {
                newWidth = 0b10 << ((int)Math.Floor(Math.Log2(newWidth)));
                needsResize = true;
            }
            if ((newHeight & (newHeight - 1)) != 0)
            {
                newHeight = 0b10 << ((int)Math.Floor(Math.Log2(newHeight)));
                needsResize = true;
            }
            if (newWidth != newHeight)
            {
                newWidth = newHeight = Math.Max(newWidth, newHeight);
                needsResize = true;
            }

            byte[] packets = new byte[(newWidth * newWidth) >> 1];
            bs.Read(packets, 0, packets.Length);

            SexyBitmap image = SexyBitmap.CreateNew(newWidth, newHeight);
            TextureColor[] pixels = image.GetPixels();

            PVRTCDecode.DecompressPVRTC(packets, pixels, (uint)newWidth, (uint)newHeight, 2);
			image.SetPixels(pixels);

            if (needsResize)
            {
                SexyBitmap croppedImage = image.Cut(0, 0, width, height);
                image.Dispose();
				
                return croppedImage;
            }

            return image;
        }

        public static int Write(BinaryStream bs, SexyBitmap image) => throw new NotImplementedException();
    }