using System;
using System.Collections.Generic;
using dk.CctalkLib.Checksumms.Helpers;
using dk.CctalkLib.Messages;
using System.Diagnostics;

namespace dk.CctalkLib.Checksumms
{
    class CRCChecksum : ICctalkChecksum
    {
        #region ICcTalkChecksum
        private Dictionary<CRCType, ICRC> cr;

        public CRCType CRCType
        {
            get;
            set;
        }

        public CRCChecksum()
        {
            cr = new Dictionary<CRCType, ICRC>();
            CRCType = CRCType.None;
        }

        ICRC getChecker()
        {
            if (CRCType == Helpers.CRCType.None) return null;
            if (cr.ContainsKey(CRCType)) return cr[this.CRCType];
            ICRC toret = null;
            switch (CRCType)
            {
                case Helpers.CRCType.CRC8:
                    toret = new CRC8(InitialCrcValue.Zeros);
                    break;
                //case Helpers.CRCType.CRC16:
                //    toret = new CRC16(InitialCrcValue.Zeros);
                //    break;
                case Helpers.CRCType.CRC16CCITT:
                    toret = new CRC16CCITT(InitialCrcValue.Zeros);
                    break;
                //case Helpers.CRCType.CRC32:
                //    toret = new CRC32(InitialCrcValue.Zeros);
                //    break;
            }
            if (toret == null) return null;
            cr.Add(CRCType, toret);
            return toret;
        }

        public Byte[] Execute(Byte[] source)
        {
            ICRC check = getChecker();
            byte[] d = check.ComputeChecksumBytes(source);
            return d;
        }

        public void CalcAndApply(Byte[] messageInBytes)
        {
            switch (CRCType)
            {
                case Helpers.CRCType.CRC8:
                    CalcAndApply8(messageInBytes);
                    break;

                case Helpers.CRCType.CRC16CCITT:
                    CalcAndApply16citt(messageInBytes);
                    break;

                default:
                    throw new NotSupportedException(CRCType.ToString());
            }
        }

        public bool Check(byte[] messageInBytes, int offset, int length)
        {
            switch (CRCType)
            {
                case Helpers.CRCType.CRC8:
                    return Check8(messageInBytes, offset, length);

                case Helpers.CRCType.CRC16CCITT:
                    return Check16citt(messageInBytes, offset, length);

                default:
                    throw new NotSupportedException(CRCType.ToString());
            }
        }

        #endregion

        #region 8-bits handlers

        private void CalcAndApply8(Byte[] messageInBytes)
        {
            if (messageInBytes == null) throw new ArgumentNullException("messageInBytes");
            ICRC checker = getChecker();
            if (checker == null) throw new NotSupportedException(CRCType.ToString());

            var checksumPlace = messageInBytes.Length - 1;

            if (messageInBytes[checksumPlace] != 0)
                throw new ArgumentException("Checksumm alredy set");

            byte retByte = checker.ComputeChecksumBytes(messageInBytes)[0];
            messageInBytes[checksumPlace] = retByte;
        }

        private bool Check8(byte[] messageInBytes, int offset, int length)
        {
            ICRC checker = getChecker();
            if (checker == null) throw new NotSupportedException(CRCType.ToString());
            return checker.ComputeChecksumBytes(messageInBytes)[0] == 0;
        }

        #endregion

        #region 16-bits citt handlers

        private void CalcAndApply16citt(Byte[] messageInBytes)
        {
            var tmp = getPayloadFromMsg16bitcitt(messageInBytes);
            var cs = Execute(tmp);
            messageInBytes[messageInBytes.Length - 1] = cs[0]; //MSB
            messageInBytes[CctalkMessage.PosSourceAddr] = cs[1]; // LSB
        }

        private bool Check16citt(byte[] buffer, int offset, int length)
        {
            ICRC checker = getChecker();
            if (checker == null) throw new NotSupportedException(CRCType.ToString());

            var tmp = getPayloadFromMsg16bitcitt(buffer);
            var cs = checker.ComputeChecksumBytes(tmp);

            int datalen = buffer[CctalkMessage.PosDataLen];

            byte LSB = buffer[CctalkMessage.PosSourceAddr];
            byte MSB = buffer[datalen + 4];

            if (cs[0] != MSB || cs[1] != LSB)
            {
                Debug.WriteLine(string.Format("Bad Cksum {0}{1} (calculated {2}{3})",
                       String.Format("{0:x2}", MSB),
                       String.Format("{0:x2}", LSB),
                       String.Format("{0:x2}", cs[0]),
                       String.Format("{0:x2}", cs[1])));
                return false;
            }
            return true;
        }

        byte[] getPayloadFromMsg16bitcitt(byte[] buffer)
        {
            var tmp = new byte[buffer.Length - 2];
            tmp[0] = buffer[0];
            tmp[1] = buffer[1];

            // copies the payload from messageInBytes to a new array that will be checksummed
            // also trims the last checksum byte
            Array.Copy(buffer, 3, tmp, 2, buffer.Length - 4); //TODO: bug possible. not checked -- i think this is correct, solariN

            return tmp;
        }
        #endregion
    }

}
