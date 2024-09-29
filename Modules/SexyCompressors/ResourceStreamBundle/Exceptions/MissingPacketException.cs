using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions
{
// Exception thrown when a Rsb Packet is Missing

public class MissingPacketException(string packetName) : 
Exception($"Missing Packet: \"{packetName}\"")
{
}

}