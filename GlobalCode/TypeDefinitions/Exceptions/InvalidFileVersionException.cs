using System;

// Exception thrown when Invalid ver is Read

public class InvalidFileVersionException<T>(T ver, string msg, params T[] valid) : 
NotSupportedException(msg ?? $"Invalid File Version (v{ver})\nAllowed: {string.Join(", ", valid)}" )
where T : struct, IComparable<T>

{
public T FileVersion{ get; } = ver;

public T[] AllowedVersions{ get; } = valid;

}