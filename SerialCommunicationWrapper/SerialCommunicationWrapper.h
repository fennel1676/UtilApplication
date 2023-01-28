// SerialCommunicationWrapper.h

#pragma once
#include <Windows.h>
#include <stdio.h>
#include <vcclr.h>
#include "include\Serial.h"

using namespace System;

namespace SerialCommunicationWrapper
{	
	public ref class WSerialCommunication
	{
	private:
		SerialCommunication::CSerial *m_serial;
		HINSTANCE m_hUnmanagedLib;

	public:
		WSerialCommunication();		//	Constructor
		~WSerialCommunication();	//	Destructor
		!WSerialCommunication();	//	Destructor
		void Free();

	public:
		int CheckPort(String^ strDevice);
		LONG Open(String^ strDevice, DWORD dwInQueue, DWORD dwOutQueue, bool fOverlapped);
		LONG Close(void);

		LONG Setup(	int nBaudrate, int nDataBits, int nParity, int nStopBits);
		LONG SetEventChar(BYTE bEventChar, bool fAdjustMask);
		LONG SetMask(DWORD dwMask);
		LONG WaitEvent(LPOVERLAPPED lpOverlapped, DWORD dwTimeout);
		LONG SetupHandshaking(int nHandshake);
		LONG SetupReadTimeouts(int nReadTimeout);

		int GetBaudrate();
		int GetDataBits();
		int GetParity();
		int GetStopBits();
		int GetHandshaking();
		DWORD GetEventMask();
		BYTE GetEventChar();

		LONG Write(IntPtr aData, size_t iLen, DWORD* pdwWritten, LPOVERLAPPED lpOverlapped, DWORD dwTimeout);
		//LONG Write2(String^ pString, DWORD* pdwWritten, LPOVERLAPPED lpOverlapped, DWORD dwTimeout);
		LONG Read(void* pData, size_t iLen, DWORD* pdwRead, LPOVERLAPPED lpOverlapped, DWORD dwTimeout);

		LONG Break();
		int GetEventType();
		int GetError();
		HANDLE GetCommHandle();
		bool IsOpen();
		LONG GetLastError();
		bool GetCTS();
		bool GetDSR();
		bool GetRing();
		bool GetRLSD();

		LONG Purge();
	};
}
