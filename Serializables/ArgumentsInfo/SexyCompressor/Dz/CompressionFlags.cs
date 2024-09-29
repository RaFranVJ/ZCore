namespace ZCore.Serializables.ArgumentsInfo.SexyCompressor.Dz
{
/// <summary> Determines how to Compress Resources inside a DZ Stream

public enum CompressionFlags : ushort
{
/// <summary> Default Compression (Uses Read-Only Streams). </summary>
Default,

/// <summary> The File is a chunk which will be Appended to each Other by the DZ Encoder. </summary>
CommonBuffer,

/// <summary> The File will be Compressed by using the DZ algorithm. </summary>
Dz = 4,

/// <summary> The File will be Compressed by using the ZLIB algorithm. </summary>
ZLib = 8,

/// <summary> The File will be Compressed by using the BZip2 algorithm. </summary>
BZip2 = 16,

/// <summary> The Chunck is a MP3 File </summary>
Mp3 = 32,

/// <summary> The Chunck is a JPEG Image.  </summary>
Jpeg = 64,

/// <summary> The File has only Zero Bytes. </summary
ZeroBytes = 128,

/// <summary> The File is Read-Only, used for Copies. </summary>
ReadOnly = 256,

/// <summary> The File will be Compressed by using the LZMA algorithm. </summary>
Lzma = 512,

/// <summary> The File should be Randomly accessed by the DZ Decoder. </summary>
RandomAccess = 1024
}

}