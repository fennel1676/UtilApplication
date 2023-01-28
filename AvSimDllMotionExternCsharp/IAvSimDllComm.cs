using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvSimDllMotionExternCsharp
{
    public interface IAvSimDllComm
    {
        bool ComOpen(int nPort);

        bool ComOpen(string remoteIp, int nPort);

        bool ComClose();

        bool ComState();

        int ComRead(byte[] msg, int len);

        int ComRead(byte[] aMessage, int nLength, int nRecvPacketLength);

        int ComWrite(byte[] msg, int len);
    }

    public class Factory
    {
        public static IAvSimDllComm CreateAvSimDllCommSocket(eOS nType)
        {
            switch (nType)
            {
                case eOS.Android_Socket:    return new AvSimDllCommSocket_AndroidSocket();
                case eOS.Android_Serial:    return new AvSimDllCommSocket_AndroidSerial();
                case eOS.PC_Soket:          return new AvSimDllCommSocket_PcSocket();
                case eOS.PC_Serial:         return new AvSimDllCommSocket_PcSerial();
                case eOS.IOS:               return null;
                default: return new AvSimDllCommSocket_AndroidSocket();
            }
        }
    }
}
