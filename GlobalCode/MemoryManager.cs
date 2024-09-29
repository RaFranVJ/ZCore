using System;
using System.Diagnostics;

/// <summary> Initializes Handling Functions for the Memory Consumed by the Process of this Program. </summary>

public static class MemoryManager
{
/// <summary> Releases the memory Consumed by a Process that its being Executed by this Program. </summary>

public static void ReleaseMemory(Process targetProcess)
{
GC.Collect();
GC.WaitForPendingFinalizers();

if(LibInfo.OS.Platform != PlatformID.Win32NT)
throw new PlatformNotSupportedException();

TaskManager.ReleaseTaskResources(targetProcess.Handle);
}

}