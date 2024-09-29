using System;

public static class PVRTC_4BPP_RGB
    {
        public static SexyBitmap Read(BinaryStream bs, int width, int height)
        {
            bool needsResize = false;
            int newWidth = width;
            int newHeight = height;

            if (newWidth < 8)
            {
                newWidth = 8;
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

            PVRTCDecode.DecompressPVRTC(packets, pixels, (uint)newWidth, (uint)newHeight, 4);
			image.SetPixels(pixels);

            if (needsResize)
            {
                SexyBitmap croppedImage = image.Cut(0, 0, width, height);
                image.Dispose();
				
                return croppedImage;
            }

            return image;
        }

        public static int Write(BinaryStream bs, SexyBitmap image, Endian endian = default)
        {
            int originalWidth = image.Width;
            bool needsResize = false;
            int newWidth = originalWidth;
            int newHeight = image.Height;

            if (newWidth < 8)
            {
                newWidth = 8;
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

            if (needsResize)
            {
                SexyBitmap resizedImage = SexyBitmap.CreateNew(newWidth, newHeight);
                image.MoveTo(resizedImage, 0, 0);
                image.Dispose();
                image = resizedImage;
            }

            TextureColor[] pixels = image.GetPixels();
            PvrTcPacket[] words = PVRTCEncode.EncodeRGB4Bpp(pixels, newWidth);

            foreach (var word in words)
            {
                bs.WriteULong(word.PvrTcWord, endian);
            }

            return newWidth >> 1;
        }
    }