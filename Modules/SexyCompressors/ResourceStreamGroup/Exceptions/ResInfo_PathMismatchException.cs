using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Exceptions
{
// Exception thrown when ver does not Match in RSG and in Manifest

public class ResInfo_PathMismatchException(string resPath, string expected, string rsgPath) : 
Exception($"Res Path: \"{resPath}\" (In Manifest) does not Match Expected: \"{expected}\" (In ResGroup)." +

$"Path to Rsg File: {rsgPath})")
{
}

}