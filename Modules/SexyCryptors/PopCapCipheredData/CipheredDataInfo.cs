namespace ZCore.Modules.SexyCryptors.PopCapCipheredData
{
/// <summary> Stores Info about the Original Size of Ciphered Files. </summary>

public class CipheredDataInfo : MetaModel<CipheredDataInfo>
{
/** <summary> Gets or Sets the Size of a File before Encryption. </summary>
<returns> The Size before Encryption. </returns> */

public long SizeBeforeEncryption{ get; set; }

/// <summary> Creates a new Instance of the <c>CipheredDataInfo</c>. </summary>

public CipheredDataInfo()
{
}

/** <summary> Creates a new Instance of the <c>CipheredDataInfo</c> with the specific Size. </summary>
<param name = "SizeBeforeEncryption"> The Size this Instance should hold as a <b>SizeBeforeEncryption</b>. </param> */

public CipheredDataInfo(long plainSize)
{
SizeBeforeEncryption = plainSize;
}

/** <summary> Reads the Info of a Ciphered File. </summary>

<param name = "targetStream"> The Stream to be Read. </param>

<returns> The Info Read. </returns> */

public static CipheredDataInfo ReadBin(BinaryStream sourceStream, Endian endian) => new( sourceStream.ReadLong(endian) );

/** <summary> Writes the Info of a Ciphered File. </summary>
<param name = "targetStream"> The Stream where the Info will be Written. </param> */

public void WriteBin(BinaryStream targetStream, Endian endian) => targetStream.WriteLong(SizeBeforeEncryption, endian);
}

}