using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Communication.ICMP
{
    public class IcmpBase
    {
        private byte m_bType;
        private byte m_bCode;
        private UInt16 m_usChecksum;
        private int m_nMessageSize;
        private byte[] m_abMessage = new byte[1024];

        public byte Type { get { return m_bType; } set { m_bType = value; } }
        public byte Code { get { return m_bCode; } set { m_bCode = value; } }
        public UInt16 Checksum { get { return m_usChecksum; } set { m_usChecksum = value; } }
        public int MessageSize { get { return m_nMessageSize; } set { m_nMessageSize = value; } }
        public byte[] Message { get { return m_abMessage; } set { m_abMessage = value; } }

        public IcmpBase()
        {
        }

        public IcmpBase(byte[] abData, int nSize)
        {
            m_bType = abData[20];
            m_bCode = abData[21];
            m_usChecksum = BitConverter.ToUInt16(abData, 22);
            m_nMessageSize = nSize - 24;
            Buffer.BlockCopy(abData, 24, m_abMessage, 0, m_nMessageSize);
        }

        public byte[] getBytes()
        {
            byte[] abData = new byte[m_nMessageSize + 9];
            Buffer.BlockCopy(BitConverter.GetBytes(m_bType), 0, abData, 0, 1);
            Buffer.BlockCopy(BitConverter.GetBytes(m_bCode), 0, abData, 1, 1);
            Buffer.BlockCopy(BitConverter.GetBytes(m_usChecksum), 0, abData, 2, 2);
            Buffer.BlockCopy(m_abMessage, 0, abData, 4, m_nMessageSize);
            return abData;
        }

        public UInt16 getChecksum()
        {
            UInt32 unChcksm = 0;
            byte[] abData = getBytes();
            int packetsize = m_nMessageSize + 8;
            int index = 0;

            while (index < packetsize)
            {
                unChcksm += Convert.ToUInt32(BitConverter.ToUInt16(abData, index));
                index += 2;
            }
            unChcksm = (unChcksm >> 16) + (unChcksm & 0xffff);
            unChcksm += (unChcksm >> 16);
            return (UInt16)(~unChcksm);
        }
    }
}
