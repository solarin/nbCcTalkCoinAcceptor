namespace dk.CctalkLib.Checksumms.Helpers
{
    public enum CRCType
    {
        None = 0, 
        CRC8 = 99,
        CRC16 = 100,
        CRC32 = 101,
        CRC16CCITT = 102
    }

    public enum InitialCrcValue 
    { 
        Zeros = 0x0000, 
        NonZero1 = 0xffff, 
        NonZero2 = 0x1D0F 
    }
    
    public interface ICRC
    {

        byte[] ComputeChecksumBytes(byte[] arr);
        //ushort ComputeChecksum(byte[] bytes);
    }
    
}
