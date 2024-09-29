public struct PvrTcPacket
{
public ulong PvrTcWord;

public PvrTcPacket()
{
}

public PvrTcPacket(ulong PvrTcWord)
{
this.PvrTcWord = PvrTcWord;
}

public uint ModulationData
{

get => (uint)(PvrTcWord & 0xFFFFFFFF);
set => PvrTcWord |= value;

}

public bool UsePunchThroughAlpha
{

get => ((PvrTcWord >> 32) & 0b1) == 1;
set => PvrTcWord |= (value ? 1ul : 0ul) << 32;

}

			public int ColorA
            {
				get => (int)((PvrTcWord >> 33) & 0b11111111111111);
				set => PvrTcWord |= ((ulong)(value & 0b11111111111111)) << 33;
			}

			public bool ColorAIsOpaque
            {
				get => ((PvrTcWord >> 47) & 0b1) == 1;
				set => PvrTcWord |= (value ? 1ul : 0ul) << 47;
			}

			public int ColorB
            {
				get => (int)((PvrTcWord >> 48) & 0b111111111111111);
				set => PvrTcWord |= ((ulong)(value & 0b111111111111111)) << 48;
			}

			public bool ColorBIsOpaque
            {
				get => (PvrTcWord >> 63) == 1;
				set => PvrTcWord |= (value ? 1ul : 0ul) << 63;
			}

			public PvrColor_RGBA GetColorA_RGBA()
			{
				int ColorA = this.ColorA;
				
				if (ColorAIsOpaque)
				{
					int r = ColorA >> 9;
					int g = (ColorA >> 4) & 0x1F;
					int b = ColorA & 0xF;
					return new( (byte)((r << 3) | (r >> 2)), (byte)((g << 3) | (g >> 2)), (byte)((b << 4) | b));
				}
				
				else
				{
					int a = (ColorA >> 11) & 0x7;
					int r = (ColorA >> 7) & 0xF;
					int g = (ColorA >> 3) & 0xF;
					int b = ColorA & 0x7;
					return new( (byte)((r << 4) | r), (byte)((g << 4) | g), (byte)((b << 5) | (b << 2) | (b >> 1)), (byte)((a << 5) | (a << 2) | (a >> 1)));
				}
			}

			public PvrColor_RGBA GetColorB_RGBA()
			{
				int ColorB = this.ColorB;
				
				if (ColorBIsOpaque)
				{
					int r = ColorB >> 10;
					int g = (ColorB >> 5) & 0x1F;
					int b = ColorB & 0x1F;
					return new( (byte)((r << 3) | (r >> 2)), (byte)((g << 3) | (g >> 2)), (byte)((b << 3) | (b >> 2)));
				}
				else
				{
					int a = (ColorB >> 12) & 0x7;
					int r = (ColorB >> 8) & 0xF;
					int g = (ColorB >> 4) & 0xF;
					int b = ColorB & 0xF;
					return new( (byte)((r << 4) | r), (byte)((g << 4) | g), (byte)((b << 4) | b), (byte)((a << 5) | (a << 2) | (a >> 1)));
				}
			}

			public PvrColor_RGB GetColorA_RGB()
			{
				int ColorA = this.ColorA;
				
				if (ColorAIsOpaque)
				{
					int r = ColorA >> 9;
					int g = (ColorA >> 4) & 0x1F;
					int b = ColorA & 0xF;
					return new( (byte)((r << 3) | (r >> 2)), (byte)((g << 3) | (g >> 2)), (byte)((b << 4) | b));
				}
				
				else
				{
					int r = (ColorA >> 7) & 0xF;
					int g = (ColorA >> 3) & 0xF;
					int b = ColorA & 0x7;
					return new( (byte)((r << 4) | r), (byte)((g << 4) | g), (byte)((b << 5) | (b << 2) | (b >> 1)));
				}
			}

			public PvrColor_RGB GetColorB_RGB()
			{
				int ColorB = this.ColorB;
				
				if (ColorBIsOpaque)
				{
					int r = ColorB >> 10;
					int g = (ColorB >> 5) & 0x1F;
					int b = ColorB & 0x1F;
					return new( (byte)((r << 3) | (r >> 2)), (byte)((g << 3) | (g >> 2)), (byte)((b << 3) | (b >> 2)));
				}
				else
				{
					int r = (ColorB >> 8) & 0xF;
					int g = (ColorB >> 4) & 0xF;
					int b = ColorB & 0xF;
					return new( (byte)((r << 4) | r), (byte)((g << 4) | g), (byte)((b << 4) | b));
				}
			}

			public void SetColorA_RGBA(PvrColor color)
            {
				int a = color.Alpha >> 5;
				
				if (a == 0x7)
                {
					int r = color.Red >> 3;
					int g = color.Green >> 3;
					int b = color.Blue >> 4;
					ColorA = r << 9 | g << 4 | b;
					ColorAIsOpaque = true;
				}
				
                else
                {
					int r = color.Red >> 4;
					int g = color.Green >> 4;
					int b = color.Blue >> 5;
					ColorA = a << 11 | r << 7 | g << 3 | b;
					ColorAIsOpaque = false;
				}
            }

			public void SetColorB_RGBA(PvrColor color)
			{
				int a = color.Alpha >> 5;
				
				if (a == 0x7)
				{
					int r = color.Red >> 3;
					int g = color.Green >> 3;
					int b = color.Blue >> 3;
					ColorB = r << 10 | g << 5 | b;
					ColorBIsOpaque = true;
				}
				
				else
				{
					int r = color.Red >> 4;
					int g = color.Green >> 4;
					int b = color.Blue >> 4;
					ColorB = a << 12 | r << 8 | g << 4 | b;
					ColorBIsOpaque = false;
				}
			}

			public void SetColorA_RGB(PvrColor color)
			{
                int r = color.Red >> 3;
                int g = color.Green >> 3;
                int b = color.Blue >> 4;
                ColorA = r << 9 | g << 4 | b;
                ColorAIsOpaque = true;
            }

			public void SetColorB_RGB(PvrColor color)
			{
                int r = color.Red >> 3;
                int g = color.Green >> 3;
                int b = color.Blue >> 3;
                ColorB = r << 10 | g << 5 | b;
                ColorBIsOpaque = true;
            }

			public static readonly byte[][] BILINEAR_FACTORS = {
				new byte[]{ 4, 4, 4, 4 },
				new byte[]{ 2, 6, 2, 6 },
				new byte[]{ 8, 0, 8, 0 },
				new byte[]{ 6, 2, 6, 2 },
				new byte[]{ 2, 2, 6, 6 },
				new byte[]{ 1, 3, 3, 9 },
				new byte[]{ 4, 0, 12, 0 },
				new byte[]{ 3, 1, 9, 3 },
				new byte[]{ 8, 8, 0, 0 },
				new byte[]{ 4, 12, 0, 0 },
				new byte[]{ 16, 0, 0, 0 },
				new byte[]{ 12, 4, 0, 0 },
				new byte[]{ 6, 6, 2, 2 },
				new byte[]{ 3, 9, 1, 3 },
				new byte[]{ 12, 0, 4, 0 },
				new byte[]{ 9, 3, 3, 1 }, 
			};

			public static readonly byte[] WEIGHTS = {
				 8, 0, 8, 0, 5, 3, 5, 3, 3, 5, 3, 5, 0, 8, 0, 8, 8, 0, 8, 0, 4, 4, 4, 4, 4, 4, 0, 0, 0, 8, 0, 8
			};
		}