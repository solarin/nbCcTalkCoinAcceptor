using System;
namespace dk.CctalkLib.Checksumms.Helpers
{
    class CRC8 : CRC
    {
        public CRC8(InitialCrcValue val)
            : base(val)
        {
        }

        public byte ComputeChecksum(byte[] buffer, byte start, byte len)
        {
            UInt16 cksum = 0;
            byte res = 0;
            byte sum = 0, i = 0;

            for (i = start; i < start + len; i++)
            {
                sum += buffer[i];
            }

            cksum = (UInt16)((UInt16)256 - (UInt16)sum);

            res = (byte)cksum;

            return res;
        }

        public override byte[] ComputeChecksumBytes(byte[] arr)
        {
            return new byte[] { ComputeChecksum(arr, 0, (byte)arr.Length) };
        }

        protected override uint ComputeChecksum(byte[] bytes)
        {
            return (uint)ComputeChecksum(bytes, 0, (byte)bytes.Length);
        }

    }
}
