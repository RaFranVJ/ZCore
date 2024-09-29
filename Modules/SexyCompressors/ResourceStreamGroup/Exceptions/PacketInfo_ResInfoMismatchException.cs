using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Exceptions
{
// Exception thrown when flags does not Match in RSG and in Manifest

public class PacketInfo_ResInfoMismatchException(int entriesCount, int expected, string path) : 
Exception($"Invalid Number of ResEntries for \"{path}\".\nEntries: {entriesCount} (In Manifest) - Expected: {expected} (In ResGroup)")
{
}

}