#pragma once

#ifndef __SERIAL_H
#define __SERIAL_H
#ifndef SERIAL_DEFAULT_OVERLAPPED
#ifndef SERIAL_NO_OVERLAPPED
#define SERIAL_DEFAULT_OVERLAPPED	true
#else
#define SERIAL_DEFAULT_OVERLAPPED	false
#endif
#endif
#include <Windows.h>

namespace SerialCommunication
{
	class CSerial
	{
	public:
		typedef enum
		{
			EEventUnknown = -1,			// Unknown event
			EEventNone = 0,				// Event trigged without cause
			EEventBreak = EV_BREAK,		// A break was detected on input
			EEventCTS = EV_CTS,		// The CTS signal changed state
			EEventDSR = EV_DSR,		// The DSR signal changed state
			EEventError = EV_ERR,		// A line-status error occurred
			EEventRing = EV_RING,		// A ring indicator was detected
			EEventRLSD = EV_RLSD,		// The RLSD signal changed state
			EEventRecv = EV_RXCHAR,		// Data is received on input
			EEventRcvEv = EV_RXFLAG,		// Event character was received on input
			EEventSend = EV_TXEMPTY,	// Last character on output was sent
			EEventPrinterError = EV_PERR,		// Printer error occured
			EEventRx80Full = EV_RX80FULL,	// Receive buffer is 80 percent full
			EEventProviderEvt1 = EV_EVENT1,		// Provider specific event 1
			EEventProviderEvt2 = EV_EVENT2,		// Provider specific event 2
		}	EEvent;
		typedef enum
		{
			EBaudUnknown = -1,			// Unknown
			EBaud110 = CBR_110,		// 110 bits/sec
			EBaud300 = CBR_300,		// 300 bits/sec
			EBaud600 = CBR_600,		// 600 bits/sec
			EBaud1200 = CBR_1200,	// 1200 bits/sec
			EBaud2400 = CBR_2400,	// 2400 bits/sec
			EBaud4800 = CBR_4800,	// 4800 bits/sec
			EBaud9600 = CBR_9600,	// 9600 bits/sec
			EBaud14400 = CBR_14400,	// 14400 bits/sec
			EBaud19200 = CBR_19200,	// 19200 bits/sec (default)
			EBaud38400 = CBR_38400,	// 38400 bits/sec
			EBaud56000 = CBR_56000,	// 56000 bits/sec
			EBaud57600 = CBR_57600,	// 57600 bits/sec
			EBaud115200 = CBR_115200,	// 115200 bits/sec
			EBaud128000 = CBR_128000,	// 128000 bits/sec
			EBaud256000 = CBR_256000,	// 256000 bits/sec
		}	EBaudrate;
		typedef enum
		{
			EDataUnknown = -1,			// Unknown
			EData5 = 5,			// 5 bits per byte
			EData6 = 6,			// 6 bits per byte
			EData7 = 7,			// 7 bits per byte
			EData8 = 8			// 8 bits per byte (default)
		}	EDataBits;
		typedef enum
		{
			EParUnknown = -1,			// Unknown
			EParNone = NOPARITY,		// No parity (default)
			EParOdd = ODDPARITY,	// Odd parity
			EParEven = EVENPARITY,	// Even parity
			EParMark = MARKPARITY,	// Mark parity
			EParSpace = SPACEPARITY	// Space parity
		}	EParity;
		typedef enum
		{
			EStopUnknown = -1,			// Unknown
			EStop1 = ONESTOPBIT,	// 1 stopbit (default)
			EStop1_5 = ONE5STOPBITS,// 1.5 stopbit
			EStop2 = TWOSTOPBITS	// 2 stopbits
		}	EStopBits;
		typedef enum
		{
			EHandshakeUnknown = -1,	// Unknown
			EHandshakeOff = 0,	// No handshaking
			EHandshakeHardware = 1,	// Hardware handshaking (RTS/CTS)
			EHandshakeSoftware = 2	// Software handshaking (XON/XOFF)
		}	EHandshake;
		typedef enum
		{
			EReadTimeoutUnknown = -1,	// Unknown
			EReadTimeoutNonblocking = 0,	// Always return immediately
			EReadTimeoutBlocking = 1	// Block until everything is retrieved
		}	EReadTimeout;
		typedef enum
		{
			EErrorUnknown = 0,			// Unknown
			EErrorBreak = CE_BREAK,	// Break condition detected
			EErrorFrame = CE_FRAME,	// Framing error
			EErrorIOE = CE_IOE,		// I/O device error
			EErrorMode = CE_MODE,	// Unsupported mode
			EErrorOverrun = CE_OVERRUN,	// Character buffer overrun, next byte is lost
			EErrorRxOver = CE_RXOVER,	// Input buffer overflow, byte lost
			EErrorParity = CE_RXPARITY,// Input parity error
			EErrorTxFull = CE_TXFULL	// Output buffer full
		}	EError;
		typedef enum
		{
			EPortUnknownError = -1,		// Unknown error occurred
			EPortAvailable = 0,		// Port is available
			EPortNotAvailable = 1,		// Port is not present
			EPortInUse = 2		// Port is in use
		}	EPort;

	protected:
		class CDCB : public DCB
		{
		public:
			CDCB() { DCBlength = sizeof(DCB); }
		};

	protected:
		LONG	m_lLastError;		// Last serial error
		HANDLE	m_hFile;			// File handle
		EEvent	m_eEvent;			// Event type
		DWORD	m_dwEventMask;		// Event mask

#ifndef SERIAL_NO_OVERLAPPED
		HANDLE	m_hevtOverlapped;	// Event handle for internal overlapped operations
#endif

	public:
		CSerial();
		virtual ~CSerial();

	public:
		static EPort CheckPort(LPCTSTR lpszDevice);

		virtual LONG Open(LPCTSTR lpszDevice, DWORD dwInQueue = 0, DWORD dwOutQueue = 0, bool fOverlapped = SERIAL_DEFAULT_OVERLAPPED);
		virtual LONG Close(void);

		virtual LONG Setup(EBaudrate eBaudrate = EBaud9600, EDataBits eDataBits = EData8, EParity eParity = EParNone, EStopBits eStopBits = EStop1);
		virtual LONG SetEventChar(BYTE bEventChar, bool fAdjustMask = true);
		virtual LONG SetMask(DWORD dwMask = EEventBreak | EEventError | EEventRecv);
		virtual LONG WaitEvent(LPOVERLAPPED lpOverlapped = 0, DWORD dwTimeout = INFINITE);
		virtual LONG SetupHandshaking(EHandshake eHandshake);
		virtual LONG SetupReadTimeouts(EReadTimeout eReadTimeout);

		virtual EBaudrate  GetBaudrate(void);
		virtual EDataBits  GetDataBits(void);
		virtual EParity    GetParity(void);
		virtual EStopBits  GetStopBits(void);
		virtual EHandshake GetHandshaking(void);
		virtual DWORD      GetEventMask(void);
		virtual BYTE       GetEventChar(void);

		virtual LONG Write(const void* pData, size_t iLen, DWORD* pdwWritten = 0, LPOVERLAPPED lpOverlapped = 0, DWORD dwTimeout = INFINITE);
		virtual LONG Write(LPCSTR pString, DWORD* pdwWritten = 0, LPOVERLAPPED lpOverlapped = 0, DWORD dwTimeout = INFINITE);
		virtual LONG Read(void* pData, size_t iLen, DWORD* pdwRead = 0, LPOVERLAPPED lpOverlapped = 0, DWORD dwTimeout = INFINITE);

		LONG Break(void);
		EEvent GetEventType(void);
		EError GetError(void);
		HANDLE GetCommHandle(void) { return m_hFile; }
		bool IsOpen(void) const { return (m_hFile != 0); }
		LONG GetLastError(void) const { return m_lLastError; }
		bool GetCTS(void);
		bool GetDSR(void);
		bool GetRing(void);
		bool GetRLSD(void);

		LONG Purge(void);

	protected:
		void CheckRequirements(LPOVERLAPPED lpOverlapped, DWORD dwTimeout) const;
		BOOL CancelCommIo(void);

	};
}
#endif	// __SERIAL_H
