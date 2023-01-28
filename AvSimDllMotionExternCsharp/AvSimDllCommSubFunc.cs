using System;

namespace AvSimDllMotionExternCsharp
{
    public class AvSimDllCommSubFunc
    {
        public static UInt16[] crc16_tbl = new UInt16[256]{
            0x0000, 0xc0c1, 0xc181, 0x0140, 0xc301, 0x03c0, 0x0280, 0xc241,     0xc601, 0x06c0, 0x0780, 0xc741, 0x0500, 0xc5c1, 0xc481, 0x0440,
            0xcc01, 0x0cc0, 0x0d80, 0xcd41, 0x0f00, 0xcfc1, 0xce81, 0x0e40,     0x0a00, 0xcac1, 0xcb81, 0x0b40, 0xc901, 0x09c0, 0x0880, 0xc841,
            0xd801, 0x18c0, 0x1980, 0xd941, 0x1b00, 0xdbc1, 0xda81, 0x1a40,     0x1e00, 0xdec1, 0xdf81, 0x1f40, 0xdd01, 0x1dc0, 0x1c80, 0xdc41,
            0x1400, 0xd4c1, 0xd581, 0x1540, 0xd701, 0x17c0, 0x1680, 0xd641,     0xd201, 0x12c0, 0x1380, 0xd341, 0x1100, 0xd1c1, 0xd081, 0x1040,
            0xf001, 0x30c0, 0x3180, 0xf141, 0x3300, 0xf3c1, 0xf281, 0x3240,     0x3600, 0xf6c1, 0xf781, 0x3740, 0xf501, 0x35c0, 0x3480, 0xf441,
            0x3c00, 0xfcc1, 0xfd81, 0x3d40, 0xff01, 0x3fc0, 0x3e80, 0xfe41,     0xfa01, 0x3ac0, 0x3b80, 0xfb41, 0x3900, 0xf9c1, 0xf881, 0x3840,
            0x2800, 0xe8c1, 0xe981, 0x2940, 0xeb01, 0x2bc0, 0x2a80, 0xea41,     0xee01, 0x2ec0, 0x2f80, 0xef41, 0x2d00, 0xedc1, 0xec81, 0x2c40,
            0xe401, 0x24c0, 0x2580, 0xe541, 0x2700, 0xe7c1, 0xe681, 0x2640,     0x2200, 0xe2c1, 0xe381, 0x2340, 0xe101, 0x21c0, 0x2080, 0xe041,
            0xa001, 0x60c0, 0x6180, 0xa141, 0x6300, 0xa3c1, 0xa281, 0x6240,     0x6600, 0xa6c1, 0xa781, 0x6740, 0xa501, 0x65c0, 0x6480, 0xa441,
            0x6c00, 0xacc1, 0xad81, 0x6d40, 0xaf01, 0x6fc0, 0x6e80, 0xae41,     0xaa01, 0x6ac0, 0x6b80, 0xab41, 0x6900, 0xa9c1, 0xa881, 0x6840,
            0x7800, 0xb8c1, 0xb981, 0x7940, 0xbb01, 0x7bc0, 0x7a80, 0xba41,     0xbe01, 0x7ec0, 0x7f80, 0xbf41, 0x7d00, 0xbdc1, 0xbc81, 0x7c40,
            0xb401, 0x74c0, 0x7580, 0xb541, 0x7700, 0xb7c1, 0xb681, 0x7640,     0x7200, 0xb2c1, 0xb381, 0x7340, 0xb101, 0x71c0, 0x7080, 0xb041,
            0x5000, 0x90c1, 0x9181, 0x5140, 0x9301, 0x53c0, 0x5280, 0x9241,     0x9601, 0x56c0, 0x5780, 0x9741, 0x5500, 0x95c1, 0x9481, 0x5440,
            0x9c01, 0x5cc0, 0x5d80, 0x9d41, 0x5f00, 0x9fc1, 0x9e81, 0x5e40,     0x5a00, 0x9ac1, 0x9b81, 0x5b40, 0x9901, 0x59c0, 0x5880, 0x9841,
            0x8801, 0x48c0, 0x4980, 0x8941, 0x4b00, 0x8bc1, 0x8a81, 0x4a40,     0x4e00, 0x8ec1, 0x8f81, 0x4f40, 0x8d01, 0x4dc0, 0x4c80, 0x8c41,
            0x4400, 0x84c1, 0x8581, 0x4540, 0x8701, 0x47c0, 0x4680, 0x8641,     0x8201, 0x42c0, 0x4380, 0x8341, 0x4100, 0x81c1, 0x8081, 0x4040
        };

        static byte[] tmp = new byte[512];

        /////////////////////////////////////////////////////////////////////////////////////////////
        //	FUNCTION		: SpaceCut
        //	DESCRIPT		: 문자열에서 스페이스를 제거함
        //	RETERN			: 
        //	ARGUMENT		:
        //		buf			: 공백을 제거할 문자열 포인터
        //	EXAMPLE			: 
        //	UPDATE			: 2005/04/11, by RKN
        /////////////////////////////////////////////////////////////////////////////////////////////
        public void SpaceCut(byte[] buf)
        {
            byte[] p, q;

            // 앞 Space제거
            p = q = buf;
            int nIndex = 0;
            while (0x00 != p[nIndex])
            {
                if (p[nIndex] == ' ' || p[nIndex] == '\t')
                    nIndex++;
                else
                    break;
            }

            nIndex = 0;
            while (0x00 != p[nIndex])
            {
                q[nIndex] = p[nIndex];
                nIndex++;
            }

            q[nIndex] = 0x00;

            // 뒤 Space제거
            p = q = buf;
            nIndex = 0;
            while (0x00 != p[nIndex])
            {
                nIndex++;
            }
            nIndex--;

            while (nIndex >= 0)
            {
                if (p[nIndex] == ' ' || p[nIndex] == '\t')
                {
                    p[nIndex] = 0x00;
                    nIndex--;
                }
                else
                    break;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        //	FUNCTION		: StrUpr
        //	DESCRIPT		: 소문자 문자열을 대문자로 치환
        //	RETERN			: 치환된 문자열의 포인터 
        //	ARGUMENT		:
        //		ptr			: 치환할 문자열의 포인터
        //	EXAMPLE			: 
        //	UPDATE			: 2005/04/11, by RKN
        /////////////////////////////////////////////////////////////////////////////////////////////
        public byte[] StrUpr(byte[] ptr)
        {
            byte[] p, q;

            q = ptr;
            p = tmp;

            int nIndex = 0;
            while (0x00 != q[nIndex])
            {
                p[nIndex] = (byte)char.ToUpper((char)q[nIndex]);
            }

            p[nIndex] = 0x00;

            return tmp;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        //	FUNCTION		: skip_crlf
        //	DESCRIPT		: ';' 또는 엔터 등의 문자열을 만나게 되면 스킵하는 함수
        //	RETERN			: 
        //	ARGUMENT		:
        //		str			: 비교할 문자열 포인터
        //	EXAMPLE			: 
        //	UPDATE			: 2005/04/11, by RKN
        /////////////////////////////////////////////////////////////////////////////////////////////
        public void skip_crlf(byte[] str)
        {
            int nIndex = 0;
            while (0x00 != str[nIndex])
            {
                if (str[nIndex] == 0x0d || str[nIndex] == 0x0a)
                    str[nIndex] = 0x00;

                nIndex++;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        //	FUNCTION		: MakeCRC16
        //	DESCRIPT		: 위의 테이블을 참조하여 CRC를 계산하는 함수(계산식보다 테이블로 하기때문에 시간이 적게 걸림.)
        //	RETERN			: 2Byte CRC 값
        //	ARGUMENT		:
        //		stxt		: 계산할 데이터의 시작 포인터
        //		ssiz		: 계산할 데이터의 길이
        //	EXAMPLE			: MakeCRC16(pData, 10)
        /////////////////////////////////////////////////////////////////////////////////////////////
        public static UInt16 MakeCRC16(byte[] stxt, int ssiz)
        {
            int i;
            UInt16 iax, ibx, idx, ial, idh;

            iax = (UInt16)0xFFFF;
            UInt16 value = 0;
            for (i = 0; i < ssiz; i++)
            {
                if (i == ssiz - 1)
                    value = stxt[i];
                else
                    value = BitConverter.ToUInt16(stxt, i);

                idx = (UInt16)((value & (UInt16)0x00FF) ^ iax);
                ibx = (UInt16)(idx & (UInt16)0x00FF);
                iax = crc16_tbl[ibx];
                ial = (UInt16)(iax & (UInt16)0x00FF);
                idh = (UInt16)((idx >> 8) & (UInt16)0x00FF);
                ial = (UInt16)((ial ^ idh) & (UInt16)0x00FF);
                iax = (UInt16)((iax & (UInt16)0xFF00) | ial);
            }
	        return (UInt16)iax;
        }

        public static UInt16 MakeCRC16(byte[] stxt, int index, int ssiz)
        {
            int i;
            UInt16 iax, ibx, idx, ial, idh;

            iax = (UInt16)0xFFFF;
            UInt16 value = 0;
            for (i = index; i < ssiz; i++)
            {
                if (i == ssiz - 1)
                    value = stxt[i];
                else
                    value = BitConverter.ToUInt16(stxt, i);

                idx = (UInt16)((value & (UInt16)0x00FF) ^ iax);
                ibx = (UInt16)(idx & (UInt16)0x00FF);
                iax = crc16_tbl[ibx];
                ial = (UInt16)(iax & (UInt16)0x00FF);
                idh = (UInt16)((idx >> 8) & (UInt16)0x00FF);
                ial = (UInt16)((ial ^ idh) & (UInt16)0x00FF);
                iax = (UInt16)((iax & (UInt16)0xFF00) | ial);
            }
            return (UInt16)iax;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        //	FUNCTION		: TickCheck()
        //	DESCRIPT		: 현재 마지막시간으로 부터의 인자 시간이 경과되었는지 확인하는 함수
        //	RETERN			: 경과 결과(FALSE:미경과, TRUE:경과)
        //	ARGUMENT		:
        //		dPreiode	: 경과 확인할 시간값(1000=1초)
        //		dLastTime	: 마지막시간(long timer = GetCurrentTime(); )
        //	EXAMPLE			: TickCheck(체크할경과시간, 마지막시간값)
        /////////////////////////////////////////////////////////////////////////////////////////////
        public static bool TickCheck(long dPeriode, long dLastTime)
        {
            long dDef, dCurTime;
            dCurTime = DateTime.Now.Ticks;
            if (dLastTime > dCurTime)
                dDef = (0xffffffffL - dLastTime) + dCurTime;
            else
                dDef = dCurTime - dLastTime;
            return (dDef >= dPeriode * 10000);            
        }
    }
}
