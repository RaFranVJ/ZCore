using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Exceptions
{
// Exception thrown when TextureId does not Match in RSG and in Manifest

public class PtxInfo_IdMismatchException(int id, int expected, string path) : 
Exception($"Invalid TextureID for \"{path}\".\nID: {id} (In Manifest) - Expected: {expected} (In ResGroup)")
{
}

}