// This is the main DLL file.

#include "stdafx.h"

#include "SerialCommunicationWrapper.h"

namespace SerialCommunicationWrapper
{
	WSerialCommunication::WSerialCommunication()
	{
		m_serial = new SerialCommunication::CSerial;
	}

	WSerialCommunication::~WSerialCommunication()
	{
		if (m_serial)
		{
			delete m_serial;
			m_serial = NULL;
		}
	}

	WSerialCommunication::!WSerialCommunication()
	{
		if (m_serial)
		{
			delete m_serial;
			m_serial = NULL;
		}
	}

	void WSerialCommunication::Free()
	{
		if (m_serial)
		{
			delete m_serial;
			m_serial = NULL;
		}
	}

	int WSerialCommunication::CheckPort(String^ strDevice)
	{		
		if (NULL == m_serial)
			return -1;
		
		wchar_t* szDevice = (wchar_t*)(void*)System::Runtime::InteropServices::Marshal::StringToCoTaskMemUni(strDevice);
		int nResult = m_serial->CheckPort(szDevice);
		System::Runtime::InteropServices::Marshal::FreeCoTaskMem((IntPtr)szDevice);
		
		return nResult;
	}

	LONG WSerialCommunication::Open(String^ strDevice, DWORD dwInQueue, DWORD dwOutQueue, bool fOverlapped)
	{
		if (NULL == m_serial)
			return -1;

		wchar_t* szDevice = (wchar_t*)(void*)System::Runtime::InteropServices::Marshal::StringToCoTaskMemUni(strDevice);
		long lResult = m_serial->Open(szDevice, dwInQueue, dwOutQueue, fOverlapped);
		System::Runtime::InteropServices::Marshal::FreeCoTaskMem((IntPtr)szDevice);
		return lResult;
	}

	LONG WSerialCommunication::Close()
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->Close();
	}

	LONG WSerialCommunication::Setup(int nBaudrate, int nDataBits, int nParity,  int nStopBits)
	{
		if (NULL == m_serial)
			return -1;
		
		return m_serial->Setup(	(SerialCommunication::CSerial::EBaudrate) nBaudrate,
								(SerialCommunication::CSerial::EDataBits) nDataBits,
								(SerialCommunication::CSerial::EParity) nParity,
								(SerialCommunication::CSerial::EStopBits) nStopBits);
	}

	LONG WSerialCommunication::SetEventChar(BYTE bEventChar, bool fAdjustMask)
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->SetEventChar(bEventChar, fAdjustMask);
	}

	LONG WSerialCommunication::SetMask(DWORD dwMask)
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->SetMask(dwMask);
	}

	LONG WSerialCommunication::WaitEvent(LPOVERLAPPED lpOverlapped, DWORD dwTimeout)
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->WaitEvent(lpOverlapped, dwTimeout);
	}

	LONG WSerialCommunication::SetupHandshaking(int nHandshake)
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->SetupHandshaking((SerialCommunication::CSerial::EHandshake) nHandshake);
	}

	LONG WSerialCommunication::SetupReadTimeouts(int nReadTimeout)
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->SetupReadTimeouts((SerialCommunication::CSerial::EReadTimeout)nReadTimeout);
	}

	int WSerialCommunication::GetBaudrate()
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->GetBaudrate();
	}

	int WSerialCommunication::GetDataBits()
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->GetDataBits();
	}

	int WSerialCommunication::GetParity()
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->GetParity();
	}

	int WSerialCommunication::GetStopBits()
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->GetStopBits();
	}

	int WSerialCommunication::GetHandshaking()
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->GetHandshaking();
	}

	DWORD WSerialCommunication::GetEventMask()
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->GetEventMask();
	}

	BYTE WSerialCommunication::GetEventChar()
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->GetEventChar();
	}

	LONG WSerialCommunication::Write(IntPtr pData, size_t iLen, DWORD* pdwWritten, LPOVERLAPPED lpOverlapped, DWORD dwTimeout)
	{
		if (NULL == m_serial)
			return -1;

		int nResult = m_serial->Write((void *)pData, iLen, pdwWritten, lpOverlapped, dwTimeout);
		return nResult;
	}

	//LONG WSerialCommunication::Write2(String^ strString, DWORD* pdwWritten, LPOVERLAPPED lpOverlapped, DWORD dwTimeout)
	//{
	//	if (NULL == m_serial)
	//		return -1;

	//	char* szString = (char*)(void*)System::Runtime::InteropServices::Marshal::StringToCoTaskMemAnsi(strString);
	//	LONG lResult = m_serial->Write(szString, pdwWritten, lpOverlapped, dwTimeout);
	//	System::Runtime::InteropServices::Marshal::FreeCoTaskMem((IntPtr)szString);

	//	return lResult;
	//}

	LONG WSerialCommunication::Read(void* pData, size_t iLen, DWORD* pdwRead, LPOVERLAPPED lpOverlapped, DWORD dwTimeout)
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->Read(pData, iLen, pdwRead, lpOverlapped, dwTimeout);
	}

	LONG WSerialCommunication::Break()
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->Break();
	}

	int WSerialCommunication::GetEventType()
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->GetEventType();
	}

	int WSerialCommunication::GetError()
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->GetError();
	}

	HANDLE WSerialCommunication::GetCommHandle()
	{
		if (NULL == m_serial)
			return NULL;

		return m_serial->GetCommHandle();
	}

	bool WSerialCommunication::IsOpen()
	{
		if (NULL == m_serial)
			return false;

		return m_serial->IsOpen();
	}

	LONG WSerialCommunication::GetLastError()
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->GetLastError();
	}

	bool WSerialCommunication::GetCTS()
	{
		if (NULL == m_serial)
			return false;

		return m_serial->GetCTS();
	}

	bool WSerialCommunication::GetDSR()
	{
		if (NULL == m_serial)
			return false;

		return m_serial->GetDSR();
	}

	bool WSerialCommunication::GetRing()
	{
		if (NULL == m_serial)
			return false;

		return m_serial->GetRing();
	}

	bool WSerialCommunication::GetRLSD()
	{
		if (NULL == m_serial)
			return false;

		return m_serial->GetRLSD();
	}

	LONG WSerialCommunication::Purge()
	{
		if (NULL == m_serial)
			return -1;

		return m_serial->Purge();
	}
}

