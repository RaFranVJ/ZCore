using System;
using System.Collections.Generic;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions
{
// Exception thrown when Length for PtxInfo read differs from Expected

public class PtxInfo_InvalidLengthException(uint infoLength, List<uint> validSizes) 
: Exception(string.Format(errorMsg, infoLength, string.Join(", ", validSizes) ) )
{
public uint LengthFound{ get; } = infoLength;

// Error MSG (should load it from LocStrings)

private const string errorMsg = "Invalid Size for PTX Info: {0}" +

"Valid sizes are: {1}";
}

}