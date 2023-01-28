#include "stdafx.h"
#include "Serial.h"

#pragma warning(disable: 4127)

#ifdef _DEBUG

#ifdef THIS_FILE
#undef THIS_FILE
#endif

static const char THIS_FILE[] = __FILE__;
#define new DEBUG_NEW

#endif

using  namespace SerialCommunication;

CSerial::CSerial()
	: m_lLastError(ERROR_SUCCESS)
	, m_hFile(0)
	, m_eEvent(EEventNone)
	, m_dwEventMask(0)
#ifndef SERIAL_NO_OVERLAPPED
	, m_hevtOverlapped(0)
#endif
{
}

CSerial::~CSerial()
{
	if (m_hFile)
	{
		_RPTF0(_CRT_WARN, "CSerial::~CSerial - Serial port not closed\n");
		Close();
	}
}

CSerial::EPort CSerial::CheckPort(LPCTSTR lpszDevice)
{
	HANDLE hFile = ::CreateFile(lpszDevice, GENERIC_READ | GENERIC_WRITE, 0, 0, OPEN_EXISTING, 0, 0);
	if (INVALID_HANDLE_VALUE == hFile)
	{
		switch (::GetLastError())
		{
		case ERROR_FILE_NOT_FOUND:			return EPortNotAvailable;
		case ERROR_ACCESS_DENIED:			return EPortInUse;
		default:							return EPortUnknownError;
		}
	}
	::CloseHandle(hFile);
	return EPortAvailable;
}

LONG CSerial::Open(LPCTSTR lpszDevice, DWORD dwInQueue, DWORD dwOutQueue, bool fOverlapped)
{
	m_lLastError = ERROR_SUCCESS;

	if (m_hFile)
	{
		m_lLastError = ERROR_ALREADY_INITIALIZED;
		_RPTF0(_CRT_WARN, "CSerial::Open - Port already opened\n");
		return m_lLastError;
	}

	m_hFile = ::CreateFile(lpszDevice, GENERIC_READ | GENERIC_WRITE, 0, 0, OPEN_EXISTING, fOverlapped ? FILE_FLAG_OVERLAPPED : 0, 0);
	if (INVALID_HANDLE_VALUE == m_hFile)
	{
		m_hFile = NULL;
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::Open - Unable to open port\n");
		return m_lLastError;
	}

#ifndef SERIAL_NO_OVERLAPPED
	_ASSERTE(NULL == m_hevtOverlapped);

	if (fOverlapped)
	{
		m_hevtOverlapped = ::CreateEvent(0, true, false, 0);
		if (NULL == m_hevtOverlapped)
		{
			m_lLastError = ::GetLastError();
			_RPTF0(_CRT_WARN, "CSerial::Open - Unable to create event\n");
			::CloseHandle(m_hFile);
			m_hFile = NULL;
			return m_lLastError;
		}
	}
#else
	_ASSERTE(!fOverlapped);
#endif

	if (dwInQueue || dwOutQueue)
	{
		_ASSERTE(dwInQueue >= 16);
		_ASSERTE(dwOutQueue >= 16);

		if (!::SetupComm(m_hFile, dwInQueue, dwOutQueue))
		{
			long lLastError = ::GetLastError();
			_RPTF0(_CRT_WARN, "CSerial::Open - Unable to setup the COM-port\n");
			Close();
			m_lLastError = lLastError;
			return m_lLastError;
		}
	}

	SetMask();
	SetupReadTimeouts(EReadTimeoutNonblocking);

	COMMCONFIG commConfig = { 0 };
	DWORD dwSize = sizeof(commConfig);
	commConfig.dwSize = dwSize;
	if (::GetDefaultCommConfig(lpszDevice, &commConfig, &dwSize))
	{
		if (!::SetCommConfig(m_hFile, &commConfig, dwSize))
			_RPTF0(_CRT_WARN, "CSerial::Open - Unable to set default communication configuration.\n");
	}
	else
		_RPTF0(_CRT_WARN, "CSerial::Open - Unable to obtain default communication configuration.\n");

	return m_lLastError;
}

LONG CSerial::Close(void)
{
	m_lLastError = ERROR_SUCCESS;
	if (NULL == m_hFile)
	{
		_RPTF0(_CRT_WARN, "CSerial::Close - Method called when device is not open\n");
		return m_lLastError;
	}

#ifndef SERIAL_NO_OVERLAPPED
	if (m_hevtOverlapped)
	{
		::CloseHandle(m_hevtOverlapped);
		m_hevtOverlapped = 0;
	}
#endif

	::CloseHandle(m_hFile);
	m_hFile = NULL;

	return m_lLastError;
}

LONG CSerial::Setup(EBaudrate eBaudrate, EDataBits eDataBits, EParity eParity, EStopBits eStopBits)
{
	m_lLastError = ERROR_SUCCESS;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::Setup - Device is not opened\n");
		return m_lLastError;
	}

	CDCB dcb;
	if (!::GetCommState(m_hFile, &dcb))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::Setup - Unable to obtain DCB information\n");
		return m_lLastError;
	}

	dcb.BaudRate = DWORD(eBaudrate);
	dcb.ByteSize = BYTE(eDataBits);
	dcb.Parity = BYTE(eParity);
	dcb.StopBits = BYTE(eStopBits);
	dcb.fParity = (eParity != EParNone);

	if (!::SetCommState(m_hFile, &dcb))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::Setup - Unable to set DCB information\n");
		return m_lLastError;
	}

	return m_lLastError;
}

LONG CSerial::SetEventChar(BYTE bEventChar, bool fAdjustMask)
{
	m_lLastError = ERROR_SUCCESS;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::SetEventChar - Device is not opened\n");
		return m_lLastError;
	}

	CDCB dcb;
	if (!::GetCommState(m_hFile, &dcb))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::SetEventChar - Unable to obtain DCB information\n");
		return m_lLastError;
	}

	dcb.EvtChar = char(bEventChar);

	if (fAdjustMask)
		SetMask(GetEventMask() | EEventRcvEv);

	if (!::SetCommState(m_hFile, &dcb))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::SetEventChar - Unable to set DCB information\n");
		return m_lLastError;
	}

	return m_lLastError;
}

LONG CSerial::SetMask(DWORD dwEventMask)
{
	m_lLastError = ERROR_SUCCESS;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::SetMask - Device is not opened\n");
		return m_lLastError;
	}

	if (!::SetCommMask(m_hFile, dwEventMask))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::SetMask - Unable to set event mask\n");
		return m_lLastError;
	}

	m_dwEventMask = dwEventMask;
	return m_lLastError;
}

LONG CSerial::WaitEvent(LPOVERLAPPED lpOverlapped, DWORD dwTimeout)
{
	CheckRequirements(lpOverlapped, dwTimeout);

	m_lLastError = ERROR_SUCCESS;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::WaitEvent - Device is not opened\n");
		return m_lLastError;
	}

#ifndef SERIAL_NO_OVERLAPPED
	if (!m_hevtOverlapped && (lpOverlapped || (dwTimeout != INFINITE)))
	{
		m_lLastError = ERROR_INVALID_FUNCTION;
		_RPTF0(_CRT_WARN, "CSerial::WaitEvent - Overlapped I/O is disabled, specified parameters are illegal.\n");
		return m_lLastError;
	}

	OVERLAPPED ovInternal;
	if (!lpOverlapped && m_hevtOverlapped)
	{
		memset(&ovInternal, 0, sizeof(ovInternal));
		ovInternal.hEvent = m_hevtOverlapped;
		lpOverlapped = &ovInternal;
	}

	_ASSERTE(!m_hevtOverlapped || HasOverlappedIoCompleted(lpOverlapped));

	if (!::WaitCommEvent(m_hFile, LPDWORD(&m_eEvent), lpOverlapped))
	{
		long lLastError = ::GetLastError();

		if (lLastError != ERROR_IO_PENDING)
		{
			m_lLastError = lLastError;
			_RPTF0(_CRT_WARN, "CSerial::WaitEvent - Unable to wait for COM event\n");
			return m_lLastError;
		}

		if (lpOverlapped == &ovInternal)
		{
			switch (::WaitForSingleObject(lpOverlapped->hEvent, dwTimeout))
			{
			case WAIT_OBJECT_0:
				break;

			case WAIT_TIMEOUT:
				CancelCommIo();
				m_lLastError = ERROR_TIMEOUT;
				return m_lLastError;

			default:
				m_lLastError = ::GetLastError();
				_RPTF0(_CRT_WARN, "CSerial::WaitEvent - Unable to wait until COM event has arrived\n");
				return m_lLastError;
			}
		}
	}
	else
	{
		if (lpOverlapped)
			::SetEvent(lpOverlapped->hEvent);
	}
#else
	if (!::WaitCommEvent(m_hFile, LPDWORD(&m_eEvent), 0))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::WaitEvent - Unable to wait for COM event\n");
		return m_lLastError;
	}
#endif

	return m_lLastError;
}

LONG CSerial::SetupHandshaking(EHandshake eHandshake)
{
	m_lLastError = ERROR_SUCCESS;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::SetupHandshaking - Device is not opened\n");
		return m_lLastError;
	}

	CDCB dcb;
	if (!::GetCommState(m_hFile, &dcb))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::SetupHandshaking - Unable to obtain DCB information\n");
		return m_lLastError;
	}

	switch (eHandshake)
	{
	case EHandshakeOff:
		dcb.fOutxCtsFlow = false;					// Disable CTS monitoring
		dcb.fOutxDsrFlow = false;					// Disable DSR monitoring
		dcb.fDtrControl = DTR_CONTROL_DISABLE;		// Disable DTR monitoring
		dcb.fOutX = false;							// Disable XON/XOFF for transmission
		dcb.fInX = false;							// Disable XON/XOFF for receiving
		dcb.fRtsControl = RTS_CONTROL_DISABLE;		// Disable RTS (Ready To Send)
		break;
	case EHandshakeHardware:
		dcb.fOutxCtsFlow = true;					// Enable CTS monitoring
		dcb.fOutxDsrFlow = true;					// Enable DSR monitoring
		dcb.fDtrControl = DTR_CONTROL_HANDSHAKE;	// Enable DTR handshaking
		dcb.fOutX = false;							// Disable XON/XOFF for transmission
		dcb.fInX = false;							// Disable XON/XOFF for receiving
		dcb.fRtsControl = RTS_CONTROL_HANDSHAKE;	// Enable RTS handshaking
		break;
	case EHandshakeSoftware:
		dcb.fOutxCtsFlow = false;					// Disable CTS (Clear To Send)
		dcb.fOutxDsrFlow = false;					// Disable DSR (Data Set Ready)
		dcb.fDtrControl = DTR_CONTROL_DISABLE;		// Disable DTR (Data Terminal Ready)
		dcb.fOutX = true;							// Enable XON/XOFF for transmission
		dcb.fInX = true;							// Enable XON/XOFF for receiving
		dcb.fRtsControl = RTS_CONTROL_DISABLE;		// Disable RTS (Ready To Send)
		break;
	default:
		_ASSERTE(false);
		m_lLastError = E_INVALIDARG;
		return m_lLastError;
	}

	if (!::SetCommState(m_hFile, &dcb))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::SetupHandshaking - Unable to set DCB information\n");
		return m_lLastError;
	}

	return m_lLastError;
}

LONG CSerial::SetupReadTimeouts(EReadTimeout eReadTimeout)
{
	m_lLastError = ERROR_SUCCESS;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::SetupReadTimeouts - Device is not opened\n");
		return m_lLastError;
	}

	COMMTIMEOUTS cto;
	if (!::GetCommTimeouts(m_hFile, &cto))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::SetupReadTimeouts - Unable to obtain timeout information\n");
		return m_lLastError;
	}

	switch (eReadTimeout)
	{
	case EReadTimeoutBlocking:
		cto.ReadIntervalTimeout = 0;
		cto.ReadTotalTimeoutConstant = 0;
		cto.ReadTotalTimeoutMultiplier = 0;
		break;
	case EReadTimeoutNonblocking:
		cto.ReadIntervalTimeout = MAXDWORD;
		cto.ReadTotalTimeoutConstant = 0;
		cto.ReadTotalTimeoutMultiplier = 0;
		break;
	default:
		_ASSERTE(false);
		m_lLastError = E_INVALIDARG;
		return m_lLastError;
	}

	if (!::SetCommTimeouts(m_hFile, &cto))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::SetupReadTimeouts - Unable to set timeout information\n");
		return m_lLastError;
	}

	return m_lLastError;
}

CSerial::EBaudrate CSerial::GetBaudrate(void)
{
	m_lLastError = ERROR_SUCCESS;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::GetBaudrate - Device is not opened\n");
		return EBaudUnknown;
	}

	CDCB dcb;
	if (!::GetCommState(m_hFile, &dcb))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::GetBaudrate - Unable to obtain DCB information\n");
		return EBaudUnknown;
	}

	return EBaudrate(dcb.BaudRate);
}

CSerial::EDataBits CSerial::GetDataBits(void)
{
	m_lLastError = ERROR_SUCCESS;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::GetDataBits - Device is not opened\n");
		return EDataUnknown;
	}

	CDCB dcb;
	if (!::GetCommState(m_hFile, &dcb))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::GetDataBits - Unable to obtain DCB information\n");
		return EDataUnknown;
	}

	return EDataBits(dcb.ByteSize);
}

CSerial::EParity CSerial::GetParity(void)
{
	m_lLastError = ERROR_SUCCESS;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::GetParity - Device is not opened\n");
		return EParUnknown;
	}

	CDCB dcb;
	if (!::GetCommState(m_hFile, &dcb))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::GetParity - Unable to obtain DCB information\n");
		return EParUnknown;
	}

	if (!dcb.fParity)
		return EParNone;

	return EParity(dcb.Parity);
}

CSerial::EStopBits CSerial::GetStopBits(void)
{
	m_lLastError = ERROR_SUCCESS;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::GetStopBits - Device is not opened\n");
		return EStopUnknown;
	}

	CDCB dcb;
	if (!::GetCommState(m_hFile, &dcb))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::GetStopBits - Unable to obtain DCB information\n");
		return EStopUnknown;
	}

	return EStopBits(dcb.StopBits);
}

DWORD CSerial::GetEventMask(void)
{
	m_lLastError = ERROR_SUCCESS;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::GetEventMask - Device is not opened\n");
		return 0;
	}

	return m_dwEventMask;
}

BYTE CSerial::GetEventChar(void)
{
	m_lLastError = ERROR_SUCCESS;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::GetEventChar - Device is not opened\n");
		return 0;
	}

	CDCB dcb;
	if (!::GetCommState(m_hFile, &dcb))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::GetEventChar - Unable to obtain DCB information\n");
		return 0;
	}

	return BYTE(dcb.EvtChar);
}

CSerial::EHandshake CSerial::GetHandshaking(void)
{
	m_lLastError = ERROR_SUCCESS;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::GetHandshaking - Device is not opened\n");
		return EHandshakeUnknown;
	}

	CDCB dcb;
	if (!::GetCommState(m_hFile, &dcb))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::GetHandshaking - Unable to obtain DCB information\n");
		return EHandshakeUnknown;
	}

	if ((dcb.fDtrControl == DTR_CONTROL_HANDSHAKE) && (dcb.fRtsControl == RTS_CONTROL_HANDSHAKE))
		return EHandshakeHardware;

	if (dcb.fOutX && dcb.fInX)
		return EHandshakeSoftware;

	return EHandshakeOff;
}

inline void CSerial::CheckRequirements(LPOVERLAPPED lpOverlapped, DWORD dwTimeout) const
{
#ifdef SERIAL_NO_OVERLAPPED
	if (lpOverlapped || (dwTimeout != INFINITE))
	{
		::MessageBox(0, _T("Overlapped I/O and time-outs are not supported, when overlapped I/O is disabled."), _T("Serial library"), MB_ICONERROR | MB_TASKMODAL);
		::DebugBreak();
		::ExitProcess(0xFFFFFFF);
	}
#endif

#ifdef SERIAL_NO_CANCELIO
	if ((dwTimeout != 0) && (dwTimeout != INFINITE))
	{
		::MessageBox(0, _T("Timeouts are not supported, when SERIAL_NO_CANCELIO is defined"), _T("Serial library"), MB_ICONERROR | MB_TASKMODAL);
		::DebugBreak();
		::ExitProcess(0xFFFFFFF);
	}
#endif	// SERIAL_NO_CANCELIO
	(void)dwTimeout;
	(void)lpOverlapped;
}

inline BOOL CSerial::CancelCommIo(void)
{
#ifdef SERIAL_NO_CANCELIO
	::DebugBreak();
	return FALSE;
#else
	return ::CancelIo(m_hFile);
#endif	// SERIAL_NO_CANCELIO
}

LONG CSerial::Write(const void* pData, size_t iLen, DWORD* pdwWritten, LPOVERLAPPED lpOverlapped, DWORD dwTimeout)
{
	CheckRequirements(lpOverlapped, dwTimeout);

	_ASSERTE(!lpOverlapped || pdwWritten);

	m_lLastError = ERROR_SUCCESS;

	DWORD dwWritten;
	if (0 == pdwWritten)
		pdwWritten = &dwWritten;

	*pdwWritten = 0;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::Write - Device is not opened\n");
		return m_lLastError;
	}

#ifndef SERIAL_NO_OVERLAPPED
	if (!m_hevtOverlapped &&
		(lpOverlapped || (dwTimeout != INFINITE)))
	{
		m_lLastError = ERROR_INVALID_FUNCTION;
		_RPTF0(_CRT_WARN, "CSerial::Write - Overlapped I/O is disabled, specified parameters are illegal.\n");
		return m_lLastError;
	}

	OVERLAPPED ovInternal;
	if (!lpOverlapped && m_hevtOverlapped)
	{
		memset(&ovInternal, 0, sizeof(ovInternal));
		ovInternal.hEvent = m_hevtOverlapped;
		lpOverlapped = &ovInternal;
	}

	_ASSERTE(!m_hevtOverlapped || HasOverlappedIoCompleted(lpOverlapped));

	if (!::WriteFile(m_hFile, pData, iLen, pdwWritten, lpOverlapped))
	{
		long lLastError = ::GetLastError();

		if (ERROR_IO_PENDING != lLastError)
		{
			m_lLastError = lLastError;
			_RPTF0(_CRT_WARN, "CSerial::Write - Unable to write the data\n");
			return m_lLastError;
		}

		if (lpOverlapped == &ovInternal)
		{
			switch (::WaitForSingleObject(lpOverlapped->hEvent, dwTimeout))
			{
			case WAIT_OBJECT_0:
				if (!::GetOverlappedResult(m_hFile, lpOverlapped, pdwWritten, FALSE))
				{
					m_lLastError = ::GetLastError();
					_RPTF0(_CRT_WARN, "CSerial::Write - Overlapped completed without result\n");
					return m_lLastError;
				}
				break;
			case WAIT_TIMEOUT:
				CancelCommIo();
				m_lLastError = ERROR_TIMEOUT;
				return m_lLastError;
			default:
				m_lLastError = ::GetLastError();
				_RPTF0(_CRT_WARN, "CSerial::Write - Unable to wait until data has been sent\n");
				return m_lLastError;
			}
		}
	}
	else
	{
		if (lpOverlapped)
			::SetEvent(lpOverlapped->hEvent);
	}
#else
	if (!::WriteFile(m_hFile, pData, iLen, pdwWritten, 0))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::Write - Unable to write the data\n");
		return m_lLastError;
	}
#endif

	return m_lLastError;
}

LONG CSerial::Write(LPCSTR pString, DWORD* pdwWritten, LPOVERLAPPED lpOverlapped, DWORD dwTimeout)
{
	CheckRequirements(lpOverlapped, dwTimeout);
	return Write(pString, strlen(pString), pdwWritten, lpOverlapped, dwTimeout);
}

LONG CSerial::Read(void* pData, size_t iLen, DWORD* pdwRead, LPOVERLAPPED lpOverlapped, DWORD dwTimeout)
{
	CheckRequirements(lpOverlapped, dwTimeout);

	_ASSERTE(!lpOverlapped || pdwRead);

	m_lLastError = ERROR_SUCCESS;

	DWORD dwRead;
	if (0 == pdwRead)
		pdwRead = &dwRead;

	*pdwRead = 0;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::Read - Device is not opened\n");
		return m_lLastError;
	}

#ifdef _DEBUG
	memset(pData, 0xDC, iLen);
#endif

#ifndef SERIAL_NO_OVERLAPPED
	if (!m_hevtOverlapped &&
		(lpOverlapped || (dwTimeout != INFINITE)))
	{
		m_lLastError = ERROR_INVALID_FUNCTION;
		_RPTF0(_CRT_WARN, "CSerial::Read - Overlapped I/O is disabled, specified parameters are illegal.\n");
		return m_lLastError;
	}

	OVERLAPPED ovInternal;
	if (NULL == lpOverlapped)
	{
		memset(&ovInternal, 0, sizeof(ovInternal));
		ovInternal.hEvent = m_hevtOverlapped;
		lpOverlapped = &ovInternal;
	}

	_ASSERTE(!m_hevtOverlapped || HasOverlappedIoCompleted(lpOverlapped));

	if (!::ReadFile(m_hFile, pData, iLen, pdwRead, lpOverlapped))
	{
		long lLastError = ::GetLastError();
		if (ERROR_IO_PENDING != lLastError)
		{
			m_lLastError = lLastError;
			_RPTF0(_CRT_WARN, "CSerial::Read - Unable to read the data\n");
			return m_lLastError;
		}

		if (lpOverlapped == &ovInternal)
		{
			switch (::WaitForSingleObject(lpOverlapped->hEvent, dwTimeout))
			{
			case WAIT_OBJECT_0:
				if (!::GetOverlappedResult(m_hFile, lpOverlapped, pdwRead, FALSE))
				{
					m_lLastError = ::GetLastError();
					_RPTF0(_CRT_WARN, "CSerial::Read - Overlapped completed without result\n");
					return m_lLastError;
				}
				break;
			case WAIT_TIMEOUT:
				CancelCommIo();
				m_lLastError = ERROR_TIMEOUT;
				return m_lLastError;
			default:
				m_lLastError = ::GetLastError();
				_RPTF0(_CRT_WARN, "CSerial::Read - Unable to wait until data has been read\n");
				return m_lLastError;
			}
		}
	}
	else
	{
		if (lpOverlapped)
			::SetEvent(lpOverlapped->hEvent);
	}
#else
	if (!::ReadFile(m_hFile, pData, iLen, pdwRead, 0))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::Read - Unable to read the data\n");
		return m_lLastError;
	}
#endif

	return m_lLastError;
}

LONG CSerial::Break(void)
{
	m_lLastError = ERROR_SUCCESS;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::Break - Device is not opened\n");
		return m_lLastError;
	}

	::SetCommBreak(m_hFile);
	::Sleep(100);
	::ClearCommBreak(m_hFile);

	return m_lLastError;
}

CSerial::EEvent CSerial::GetEventType(void)
{
#ifdef _DEBUG
	if ((m_eEvent & m_dwEventMask) == 0)
		_RPTF2(_CRT_WARN, "CSerial::GetEventType - Event %08Xh not within mask %08Xh.\n", m_eEvent, m_dwEventMask);
#endif

	EEvent eEvent = EEvent(m_eEvent & m_dwEventMask);
	m_eEvent = EEventNone;

	return eEvent;
}

CSerial::EError CSerial::GetError(void)
{
	m_lLastError = ERROR_SUCCESS;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::GetError - Device is not opened\n");
		return EErrorUnknown;
	}

	DWORD dwErrors = 0;
	if (!::ClearCommError(m_hFile, &dwErrors, 0))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::GetError - Unable to obtain COM status\n");
		return EErrorUnknown;
	}

	return EError(dwErrors);
}

bool CSerial::GetCTS(void)
{
	m_lLastError = ERROR_SUCCESS;

	DWORD dwModemStat = 0;
	if (!::GetCommModemStatus(m_hFile, &dwModemStat))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::GetCTS - Unable to obtain the modem status\n");
		return false;
	}

	return (dwModemStat & MS_CTS_ON) != 0;
}

bool CSerial::GetDSR(void)
{
	m_lLastError = ERROR_SUCCESS;

	DWORD dwModemStat = 0;
	if (!::GetCommModemStatus(m_hFile, &dwModemStat))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::GetDSR - Unable to obtain the modem status\n");
		return false;
	}

	return (dwModemStat & MS_DSR_ON) != 0;
}

bool CSerial::GetRing(void)
{
	m_lLastError = ERROR_SUCCESS;

	DWORD dwModemStat = 0;
	if (!::GetCommModemStatus(m_hFile, &dwModemStat))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::GetRing - Unable to obtain the modem status");
		return false;
	}

	return (dwModemStat & MS_RING_ON) != 0;
}

bool CSerial::GetRLSD(void)
{
	m_lLastError = ERROR_SUCCESS;

	DWORD dwModemStat = 0;
	if (!::GetCommModemStatus(m_hFile, &dwModemStat))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::GetRLSD - Unable to obtain the modem status");
		return false;
	}

	return (dwModemStat & MS_RLSD_ON) != 0;
}

LONG CSerial::Purge()
{
	m_lLastError = ERROR_SUCCESS;

	if (NULL == m_hFile)
	{
		m_lLastError = ERROR_INVALID_HANDLE;
		_RPTF0(_CRT_WARN, "CSerial::Purge - Device is not opened\n");
		return m_lLastError;
	}

	if (!::PurgeComm(m_hFile, PURGE_TXCLEAR | PURGE_RXCLEAR))
	{
		m_lLastError = ::GetLastError();
		_RPTF0(_CRT_WARN, "CSerial::Purge - Overlapped completed without result\n");
	}

	return m_lLastError;
}
