using System.Collections.Generic;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.PopCapTexture
{
/// <summary> Represents some Params for a PopCapTexture (PTX) inside a RSB Stream </summary>

public class PtxParamsForRsb : PtxParams
{
/** <summary> Gets a List of Valids Sizes for PTX Info inside the RSB Stream. </summary>
<returns> The ValidSizes </returns> */

public static readonly List<uint> ValidInfoSizes = new() {16, 20, 24};

/** <summary> Gets or Sets Index of the Texture inside the RSB Stream. </summary>
<returns> The TextureIndex </returns> */

public int TextureIndex{ get; set; }

/** <summary> Gets or Sets the TextureWidth. </summary>
<returns> The TextureWidth </returns> */

public int TextureWidth{ get; set; }

/** <summary> Gets or Sets the TextureHeight. </summary>
<returns> The TextureHeight </returns> */

public int TextureHeight{ get; set; }

/// <summary> Creates a new Instance of the <c>PtxParams</c> (RSB Variation) </summary>

public PtxParamsForRsb()
{
}

/// <summary> Creates a new Instance of the <c>PtxParams</c> (RSB Variation) </summary>

public PtxParamsForRsb(int width, int height, uint format)
{
TextureWidth = width;
TextureHeight = height;

TextureFormat = format;
}

/// <summary> Creates a new Instance of the <c>PtxParams</c> (RSB Variation) </summary>

public PtxParamsForRsb(int index, int width, int height, int pitch, uint format)
{
TextureIndex = index;

TextureWidth = width;
TextureHeight = height;

TexturePitch = pitch;
TextureFormat = format;
}

// Read PTX Info

public static List<PtxParamsForRsb> Read(BinaryStream sourceStream, Endian endian, uint offset,
uint infoLength, uint ptxCount, bool checkInfoLength = true)
{
sourceStream.Position = offset;

List<PtxParamsForRsb> ptxInfo = new();

if(!ValidInfoSizes.Contains(infoLength) && checkInfoLength)
throw new PtxInfo_InvalidLengthException(infoLength, ValidInfoSizes);

for(int i = 0; i < ptxCount; i++)
{
int width = sourceStream.ReadInt(endian);
int height = sourceStream.ReadInt(endian);

int pitch = sourceStream.ReadInt(endian);
uint format = sourceStream.ReadUInt(endian);

PtxParamsForRsb imgParams = new(i, width, height, pitch, format);

if(infoLength >= 20)
{
imgParams.AlphaSize = sourceStream.ReadInt(endian);

imgParams.AlphaChannel = infoLength == 24 ? (PtxAlphaChannel)sourceStream.ReadUInt(endian) : 
(imgParams.AlphaSize == 0 ? PtxAlphaChannel.Default : PtxAlphaChannel.A_Palette);

}

ptxInfo.Add(imgParams);
}

uint endOffset = infoLength * ptxCount + offset;
RsbHelper.CheckEndOffset(sourceStream, endOffset);

return ptxInfo;
}

}

}