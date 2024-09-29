using System;
using System.IO;

/// <summary> Allows Reading and Writing Bits (0 and 1) as Streams. </summary>

public class BitsStream : IDisposable
{
/// <summary> The Buffers of the <c>BitsStream</c>. </summary>

protected int buffers;

/// <summary> Specifies how the Stream should be Handled after being Opened. </summary>

public bool LeaveOpened = false;

/** <summary> Checks if the BaseStream is Actually a MemoryStream. </summary>

<returns> <b>true</b> if the BaseStream is a MemoryStream; otherwise, <b>false</b>. </returns> */

public bool IsMemoryStream => BaseStream is MemoryStream;

/** <summary> Gets of Sets the BaseStream of the <c>BitsStream</c>. </summary>

<returns> The Base Stream. </returns> */

public Stream BaseStream{ get; set; }

/** <summary> Gets or Sets the Length in Bytes of the <c>BitsStream</c>. </summary>

<returns> The Length of the Stream. </returns> */

public long Length{ get => BaseStream.Length; set => BaseStream.SetLength(value); }

/** <summary> Gets or Sets the Bits Position in the Stream. </summary>

<returns> The Bits Position. </returns> */

protected int BitsPosition{ get; set; }

/** <summary> Gets or Sets the Position of the <c>BitsStream</c>. </summary>

<returns> The Position of the Stream. </returns> */

public long Position{ get => BaseStream.Position; set => BaseStream.Position = value; }

/// <summary> Creates a new Instance of the BitsStream Class. </summary>

public BitsStream() : this(new MemoryStream() )
{
}

/** <summary> Creates a new Instance of the <c>BitsStream</c> Class with the given Stream. </summary>

<param name = "sourceStream"> The Stream where the Instance will be Created from. </param> */

public BitsStream(Stream sourceStream)
{
BaseStream = sourceStream;
BitsPosition = 0;
}

/** <summary> Creates a new Instance of the <c>BitsStream</c> Class with the given Buffers. </summary>

<param name = "sourceBuffers"> The Buffers where the Instance will be Created from. </param> */

public BitsStream(byte[] sourceBuffers) : this( new MemoryStream(sourceBuffers) )
{
}

/** <summary> Creates a new Instance of the <c>BitsStream</c> Class with the specific Location and opening Mode. </summary>

<param name = "targetPath"> The Path where the BitsStream will be Created. </param>
<param name = "openingMode"> The Opening Mode of the Stream. </param> */

public BitsStream(string targetPath, FileMode openingMode) : this( new FileStream(targetPath, openingMode) )
{
}

/// <summary> Closes the Stream and Releases all the Resources consumed by it. </summary>

public virtual void Close() => Dispose(true);

/** <summary> Creates a new <c>BitsStream</c> on the specific Location and with the specific Opening Mode. </summary>

<param name = "targetPath"> The Path where the Stream will be Created. </param>
<param name = "openingMode"> The Opening Mode of the Stream. </param>

<returns> The Stream that was Created. </returns> */

public static BitsStream Create(string targetPath, FileMode openingMode) => new(targetPath, openingMode);

/// <summary> Releases all the Resources consumed by the Stream. </summary>

public void Dispose() => Dispose(true);

/** <summary> Releases all the Resources consumed by the Stream. </summary>

<param name = "disposing"> Determines if all the Resources should be Discarded. </param> */

protected virtual void Dispose(bool disposing)
{

if(disposing)
{

if(LeaveOpened)
BaseStream.Flush();

else
BaseStream.Close();

}

}

/** <summary> Opens a <c>BitsStream</c> on the specific Location. </summary>

<param name = "targetPath"> The Path where the BitsStream to be Opened is Located. </param>

<returns> The BitsStream that was Opened. </returns> */

public static BitsStream Open(string targetPath) => new BitsStream(targetPath, FileMode.Open);

/** <summary> Reads a specific Number of Bits from a BitsStream. </summary>

<param name = "bitsCount"> The Number of Bits to Read. </param>

<returns> The Bits Read. </returns> */

public int ReadBits(int bitsCount)
{
int bitsRead = 0;

for(int i = bitsCount - 1; i >= 0; i--)
{
int singleBit = ReadOneBit();
bitsRead |= singleBit << i;
}

return bitsRead;
}

/** <summary> Reads one Bit from a <c>BitsStream</c>. </summary>

<returns> The Bit that was Read. </returns> */

public int ReadOneBit()
{

if(BitsPosition == 0)
{
buffers = BaseStream.ReadByte();

if(buffers == -1)
throw new IOException("Reached end of File");
}

BitsPosition = (BitsPosition + 7) % 8;
return (buffers >> BitsPosition) & 0b1;
}

public static implicit operator Stream(BitsStream a) => a.BaseStream;
}