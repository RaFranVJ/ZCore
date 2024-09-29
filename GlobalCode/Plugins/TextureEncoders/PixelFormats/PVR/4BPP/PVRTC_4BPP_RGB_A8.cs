using System;

public static class PVRTC_4BPP_RGB_A8
{
        public static SexyBitmap Read(BinaryStream bs, int width, int height)
        {
            bool resizeNeeded = false;
			
            int newWidth = width;
            int newHeight = height;

            if (newWidth < 8)
            {
                newWidth = 8;
                resizeNeeded = true;
            }
            if (newHeight < 8)
            {
                newHeight = 8;
                resizeNeeded = true;
            }
            if ((newWidth & (newWidth - 1)) != 0)
            {
                newWidth = 1 << ((int)Math.Ceiling(Math.Log2(newWidth)));
                resizeNeeded = true;
            }
            if ((newHeight & (newHeight - 1)) != 0)
            {
                newHeight = 1 << ((int)Math.Ceiling(Math.Log2(newHeight)));
                resizeNeeded = true;
            }
            if (newWidth != newHeight)
            {
                newWidth = newHeight = Math.Max(newWidth, newHeight);
                resizeNeeded = true;
            }

            byte[] packets = new byte[(newWidth * newWidth) >> 1];
            bs.Read(packets, 0, packets.Length);

            SexyBitmap image = SexyBitmap.CreateNew(newWidth, newHeight);
            TextureColor[] pixels = image.GetPixels();

            PVRTCDecode.DecompressPVRTC(packets, pixels, (uint)newWidth, (uint)newHeight, 4);
            image.SetPixels(pixels);

            if (resizeNeeded)
            {
                SexyBitmap resizedImage = image.Cut(0, 0, width, height);
                image.Dispose();
                image = resizedImage;
            }

            int imageSize = image.Square;
			
            for (int i = 0; i < imageSize; i++)
            {
                pixels[i].Alpha = bs.ReadByte();
            }

            return image;
        }

        public static int Write(BinaryStream bs, SexyBitmap image, Endian endian = default)
        {
            TextureColor[] pixels = image.GetPixels();
            int imageSize = image.Square;

            // Determine new dimensions and handle resizing if necessary
            int newWidth = image.Width;
            int newHeight = image.Height;
            bool resizeNeeded = false;

            if (newWidth < 8)
            {
                newWidth = 8;
                resizeNeeded = true;
            }
            if (newHeight < 8)
            {
                newHeight = 8;
                resizeNeeded = true;
            }
            if ((newWidth & (newWidth - 1)) != 0)
            {
                newWidth = 1 << ((int)Math.Ceiling(Math.Log2(newWidth)));
                resizeNeeded = true;
            }
            if ((newHeight & (newHeight - 1)) != 0)
            {
                newHeight = 1 << ((int)Math.Ceiling(Math.Log2(newHeight)));
                resizeNeeded = true;
            }
            if (newWidth != newHeight)
            {
                newWidth = newHeight = Math.Max(newWidth, newHeight);
                resizeNeeded = true;
            }

            // Resize image if needed
            if (resizeNeeded)
            {
                SexyBitmap resizedImage = SexyBitmap.CreateNew(newWidth, newHeight);
                image.MoveTo(resizedImage, 0, 0);
                image.Dispose();
                image = resizedImage;
            }

            PvrTcPacket[] packets = PVRTCEncode.EncodeRGB4Bpp(pixels, newWidth);
			
            foreach (var packet in packets)
            {
                bs.WriteULong(packet.PvrTcWord, endian);
            }

            // Write alpha channel values
            for (int i = 0; i < imageSize; i++)
            {
                bs.WriteByte(pixels[i].Alpha);
            }

            return newWidth << 2;
        }
		
}