namespace ZCore.Modules.TextureDrawer.Parsers.SexyTexture.Definitions
{
/// <summary> Determines how to Compress SexyTex Files (Depends on TextureFormat selected by User)

public enum CompressionFlags : uint
{
/// <summary> SexyTextures won't use Compression. </summary>
NoCompression,

/// <summary> SexyTextures will be Compressed by using the ZLIB algorithm. </summary>
ZLib
}

}