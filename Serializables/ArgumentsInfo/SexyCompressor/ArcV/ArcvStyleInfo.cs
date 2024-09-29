namespace ZCore.Serializables.ArgumentsInfo.SexyCompressor.ArcV
{
/// <summary> Groups options related to how to Apply Sryle to ARCV Data. </summary>

public class ArcvStyleInfo : ParamGroupInfo
{
/** <summary> Gets or Sets the Number of Bytes to Indent. </summary>
<returns> The Bytes Identation. </returns> */

public uint BytesIdentation{ get; set; }

/** <summary> Gets or Sets the Size Difference between Files inside the ARCV Stream. </summary>
<returns> The Size Diff. </returns> */

public uint SizeDiffBetweenFiles{ get; set; }

/** <summary> Gets or Sets the Byte to use for Padding the ARCV Stream. </summary>
<returns> The Padding Byte. </returns> */

public byte PaddingByte{ get; set; }

/// <summary> Creates a new Instance of the <c>ArcvStyleInfo</c>. </summary>

public ArcvStyleInfo()
{
BytesIdentation = 4;
SizeDiffBetweenFiles = 12;

PaddingByte = 0xAC;
}

}

}