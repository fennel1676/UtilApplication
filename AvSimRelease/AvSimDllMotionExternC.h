
/////////////////////////////////////////////////////////////////////////////////////////////
#define		Dllexport			__declspec(dllexport)
#define		Dllimport			__declspec(dllimport)
#define		DllExportExternC	extern "C" __declspec(dllexport)
#define		DllImportExternC	extern "C" __declspec(dllimport)
/////////////////////////////////////////////////////////////////////////////////////////////
// AvSim DOF Scale Data Define
#define		AV_SIM__ACTION_VALUE_MIN				    0		// AvSim Scale 최소치
#define		AV_SIM__ACTION_VALUE_CENTER				10000		// AvSim Scale 중간치
#define		AV_SIM__ACTION_VALUE_MAX				20000		// AvSim Scale 최대치
/////////////////////////////////////////////////////////////////////////////////////////////


/////////////////////////////////////////////////////////////////////////////////////////////
// AvSim Circling Scale Data Define
#define		AV_SIM__ACTION_CIRCLING_VALUE_MIN							0		// AvSim Scale 최소치
#define		AV_SIM__ACTION_CIRCLING_VALUE_CENTER					18000		// AvSim Scale 중간치
#define		AV_SIM__ACTION_CIRCLING_VALUE_MAX						36000		// AvSim Scale 최대치
/////////////////////////////////////////////////////////////////////////////////////////////

#define   MOTOR_DRIVE_POLING_CHANNEL_MAX				9 

#define AV_SIM__BLOW_SCALE_CALCULATE_ZERO			0
#define AV_SIM__BLOW_SCALE_CALCULATE_SPAN			100

#define AV_SIM__MOTION_SPEED_SCALE_CALCULATE_ZERO			1
#define AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SPAN			255
#define AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE			30

#define AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_ZERO			3000
#define AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SPAN			0
#define AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SAFE			200

#define AV_SIM__CIRCLING_MODE_DEFAULT			0
#define AV_SIM__CIRCLING_MODE_STOP   			1
#define AV_SIM__CIRCLING_MODE_CW				2
#define AV_SIM__CIRCLING_MODE_CCW				3

#define AV_SIM__RESPONSE_DISABLE			0
#define AV_SIM__RESPONSE_ENABLE				1

/////////////////////////////////////////////////////////////////////////////
#ifndef __AVSIM_DLL_MOTION_EXTERN_C_H__			// 라이브러리 만드는곳에서 함수선언
/////////////////////////////////////////////////////////////////////////////
	extern	int	Communication_MakeCmd(int nMainCmd, int nSubCmd, unsigned char chResponseType, int nDataLength, unsigned char* pDataBuf, unsigned char* pComDataFrame);
/////////////////////////////////////////////////////////////////////////////
#else						// 라이브러리를 사용하는 곳에서의 함수선언
/////////////////////////////////////////////////////////////////////////////
	int			Communication_MakeCmd(int nMainCmd, int nSubCmd, unsigned char chResponseType, int nDataLength, unsigned char* pDataBuf, unsigned char* pComDataFrame);
/////////////////////////////////////////////////////////////////////////////
#endif						// 라이브러리선언 끝
/////////////////////////////////////////////////////////////////////////////
#pragma pack( push, 1 )
	enum eOutput
	{
		eRoll,
		ePitch,
		eYaw,
		eSway,
		eSurge,
		eHeave,
		eRollEx,
		ePitchEx,
		eYawEx,
		eMax,
	};

	enum eAxisOutput
	{
		eAxis1,
		eAxis2,
		eAxis3,
		eAxis4,
		eAxis5,
		eAxis6,
		eAxisEx7,
		eAxisEx8,
		eAxisEx9,
		eAxisMax,
	};
	
	typedef struct __motorErrorData
	{
		unsigned char mMainErrCode;
		unsigned char mSubErrCode;
	}MOTOR_ERROR_DATA, *LPMOTOR_ERROR_DATA;

	typedef struct __motionData
	{
		unsigned int mRoll;
		unsigned int mPitch;
		unsigned int mYaw;
		unsigned int mSway;
		unsigned int mSurge;
		unsigned int mHeave;
		unsigned int mMotionSpeed;
		unsigned int mBlower;

		__motionData()
		{
			Init();
		}

		void Init()
		{
			mRoll			= AV_SIM__ACTION_VALUE_CENTER;
			mPitch			= AV_SIM__ACTION_VALUE_CENTER;
			mYaw			= AV_SIM__ACTION_VALUE_CENTER;
			mSway			= AV_SIM__ACTION_VALUE_CENTER;
			mSurge			= AV_SIM__ACTION_VALUE_CENTER;
			mHeave			= AV_SIM__ACTION_VALUE_CENTER;
			mMotionSpeed	= AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SAFE; //RPM 스피드 적용 (기존 스피드값 30 정도)
			mBlower			= AV_SIM__BLOW_SCALE_CALCULATE_ZERO;
		}
	}MOTION_DATA, *LPMOTION_DATA;

	typedef struct __motionExtendData
	{
		MOTION_DATA  mMotionData;
		unsigned int mRolling;
		unsigned int mRollingSpeed;
		unsigned int mRollingMode;
		unsigned int mPitching;
		unsigned int mPitchingSpeed;
		unsigned int mPitchingMode;
		unsigned int mYawing;
		unsigned int mYawingSpeed;
		unsigned int mYawingMode;

		__motionExtendData()
		{
			Init();
		}

		void Init()
		{
			mMotionData.Init();
			mRolling		= AV_SIM__ACTION_CIRCLING_VALUE_CENTER;
			mRollingSpeed	= AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SAFE;//RPM 스피드 적용 (기존 스피드값 30 정도)
			mRollingMode	= AV_SIM__CIRCLING_MODE_DEFAULT;
			mPitching		= AV_SIM__ACTION_CIRCLING_VALUE_CENTER;
			mPitchingSpeed	= AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SAFE;//RPM 스피드 적용 (기존 스피드값 30 정도)
			mPitchingMode	= AV_SIM__CIRCLING_MODE_DEFAULT;
			mYawing			= AV_SIM__ACTION_CIRCLING_VALUE_CENTER;
			mYawingSpeed	= AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SAFE;//RPM 스피드 적용 (기존 스피드값 30 정도)
			mYawingMode		= AV_SIM__CIRCLING_MODE_DEFAULT;
		}

	}MOTION_EXTEND_DATA, *LPMOTION_EXTEND_DATA;

	typedef struct __equipmentData
	{
		unsigned int mDO;
		unsigned int mDI;
		unsigned int mSrcAxisPos[eAxisOutput::eAxisMax];
		unsigned int mDstAxisPos[eAxisOutput::eAxisMax];
		unsigned int mEcdAxisPos[eAxisOutput::eAxisMax];	

	}EQUIPMENT_DATA, *LPEQUIPMENT_DATA;

	typedef struct __equipmentExtendData
	{
		unsigned int mDO;
		unsigned int mDI;
		unsigned int mAxisPos[eAxisOutput::eAxisMax];
		MOTOR_ERROR_DATA mAxisAlarm[eAxisOutput::eAxisMax + 1]; //마지막 alarm 값을 읽어오기 위함
	}EQUIPMENT_EXTEND_DATA, *LPEQUIPMENT_EXTEND_DATA;


	typedef struct __axisData
	{
		unsigned int mAxis;
		unsigned int mAxisSpeed;
	}AXIS_DATA, *LPAXIS_DATA;



	typedef struct __motionAxisData
	{
		AXIS_DATA mAxisData[eAxisOutput::eAxisEx9];
		unsigned int mBlower;

		__motionAxisData()
		{
			Init();
		}

		void Init()
		{
			mAxisData[eAxis1].mAxis = AV_SIM__ACTION_VALUE_CENTER;
			mAxisData[eAxis1].mAxisSpeed = AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE;
			mAxisData[eAxis2].mAxis = AV_SIM__ACTION_VALUE_CENTER;
			mAxisData[eAxis2].mAxisSpeed = AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE;
			mAxisData[eAxis3].mAxis = AV_SIM__ACTION_VALUE_CENTER;
			mAxisData[eAxis3].mAxisSpeed = AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE;
			mAxisData[eAxis4].mAxis = AV_SIM__ACTION_VALUE_CENTER;
			mAxisData[eAxis4].mAxisSpeed = AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE;
			mAxisData[eAxis5].mAxis = AV_SIM__ACTION_VALUE_CENTER;
			mAxisData[eAxis5].mAxisSpeed = AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE;
			mAxisData[eAxis6].mAxis = AV_SIM__ACTION_VALUE_CENTER;
			mAxisData[eAxis6].mAxisSpeed = AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE;
			mAxisData[eAxisEx7].mAxis = AV_SIM__ACTION_CIRCLING_VALUE_CENTER;
			mAxisData[eAxisEx7].mAxisSpeed = AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE;
			mAxisData[eAxisEx8].mAxis = AV_SIM__ACTION_CIRCLING_VALUE_CENTER;
			mAxisData[eAxisEx8].mAxisSpeed = AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE;
			mBlower = AV_SIM__BLOW_SCALE_CALCULATE_ZERO;
		}
	}MOTION_AXIS_DATA, *LPMOTION_AXIS_DATA;
	
#pragma pack(pop) 


	/////////////////////////////////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////////////////////
	// Initial
	DllExportExternC	BOOL			MotionControl__Initial();
	DllExportExternC	BOOL			MotionControl__Initial_Port(int nPort, int nTimeout);
	DllExportExternC	BOOL			MotionControl__Initial_Sock(const char* szRemoteIP, int nPort, int nTimeout);
	// Destroy
	DllExportExternC	BOOL			MotionControl__Destroy();
	DllExportExternC	BOOL			MotionControl__Destroy_Port(int nPort);
	DllExportExternC	BOOL			MotionControl__Destroy_Sock();
	// State Check
	DllExportExternC	BOOL			MotionControl__State();
	DllExportExternC	BOOL			MotionControl__State_Port(int nPort);
	DllExportExternC	BOOL			MotionControl__State_Sock();
	// Ping Test
	DllExportExternC	BOOL			MotionControl__Ping();
	DllExportExternC	BOOL			MotionControl__Ping_Port(int nPort, bool bResp = true);
	DllExportExternC	BOOL			MotionControl__Ping_Sock(bool bResp = true);
	// Motion Control DO 
	//	nDO : bit flag
	DllExportExternC	BOOL			MotionControl__DO(int nDO);
	DllExportExternC	BOOL			MotionControl__DO_Port(int nPort, int nDO);
	DllExportExternC	BOOL			MotionControl__DO_Sock(int nDO);
	// Motion Control DI (Read)
	//	return : Digital Input bit flag
	DllExportExternC	int				MotionControl__GetDI();
	DllExportExternC	int				MotionControl__GetDI_Port(int nPort);
	DllExportExternC	int				MotionControl__GetDI_Sock();
	// Motion Control DO (Read)
	//	return : Digital Output bit flag
	DllExportExternC	int				MotionControl__GetDO();
	DllExportExternC	int				MotionControl__GetDO_Port(int nPort);
	DllExportExternC	int				MotionControl__GetDO_Sock();
	// Motion Control DOF and Blower
	//	nRoll ~ nHeave : 0~20000
	//	nSpeed : 1(max) ~ 255(min)
	//	nBlower : 0(min) ~ 100(max)
	DllExportExternC	BOOL			MotionControl__DOF_and_Blower(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower);
	DllExportExternC	BOOL			MotionControl__DOF_and_Blower_Port(int nPort, int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower);
	DllExportExternC	BOOL			MotionControl__DOF_and_Blower_Sock(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower);
	// Motion Control Axis and Blower
	//	nAxisPos1 ~ nAxisPos6 : 0~20000
	//	nAxisPos7 ~ nAxisPos8 : 0~36000
	//	nAxisSpeed1 ~ nAxisSpeed8 : 1(max) ~ 255(min)
	//	nBlower : 0(min) ~ 100(max)
	DllExportExternC	BOOL			MotionControl__Axis_and_Blower(MOTION_AXIS_DATA stAxisData);
	DllExportExternC	BOOL			MotionControl__Axis_and_Blower_Port(int nPort, MOTION_AXIS_DATA stAxisData);
	DllExportExternC	BOOL			MotionControl__Axis_and_Blower_Sock(MOTION_AXIS_DATA stAxisData);

	/////////////////////////////////////////////////////////
	//CloudGate 전용 (Horse)
	//Motion Control Axis1, Axis2 Control & Data Obtain & Axes Position Obtain
	DllExportExternC	BOOL			MotionControl__Axis_and_DI_DataObtain(unsigned int* pAxis1Dir, unsigned int* pAxis1Pos, unsigned int* pAxis1Speed, unsigned int* pAxis2Dir, unsigned int* pAxis2Pos, unsigned int* pAxis2Speed, unsigned int* pnDI, unsigned int arrSrcPos[8], unsigned int arrDstPos[8], unsigned int arrEcdPos[8], bool bResp = true);
	DllExportExternC	BOOL			MotionControl__Axis_and_DI_DataObtain_Port(int nPort, unsigned int* pAxis1Dir, unsigned int* pAxis1Pos, unsigned int* pAxis1Speed, unsigned int* pAxis2Dir, unsigned int* pAxis2Pos, unsigned int* pAxis2Speed, unsigned int* pnDI, unsigned int arrSrcPos[8], unsigned int arrDstPos[8], unsigned int arrEcdPos[8], bool bResp = true);
	DllExportExternC	BOOL			MotionControl__Axis_and_DI_DataObtain_Sock(unsigned int* pAxis1Dir, unsigned int* pAxis1Pos, unsigned int* pAxis1Speed, unsigned int* pAxis2Dir, unsigned int* pAxis2Pos, unsigned int* pAxis2Speed, unsigned int* pnDI, unsigned int arrSrcPos[8], unsigned int arrDstPos[8], unsigned int arrEcdPos[8], bool bResp = true);
	//RPM
	DllExportExternC	BOOL			MotionControlV2__Axis_and_DI_DataObtain(unsigned int* pAxis1Dir, unsigned int* pAxis1Pos, unsigned int* pAxis1Speed, unsigned int* pAxis2Dir, unsigned int* pAxis2Pos, unsigned int* pAxis2Speed, unsigned int* pnDI, unsigned int arrSrcPos[8], unsigned int arrDstPos[8], unsigned int arrEcdPos[8], bool bResp = true);
	DllExportExternC	BOOL			MotionControlV2__Axis_and_DI_DataObtain_Port(int nPort, unsigned int* pAxis1Dir, unsigned int* pAxis1Pos, unsigned int* pAxis1Speed, unsigned int* pAxis2Dir, unsigned int* pAxis2Pos, unsigned int* pAxis2Speed, unsigned int* pnDI, unsigned int arrSrcPos[8], unsigned int arrDstPos[8], unsigned int arrEcdPos[8], bool bResp = true);
	DllExportExternC	BOOL			MotionControlV2__Axis_and_DI_DataObtain_Sock(unsigned int* pAxis1Dir, unsigned int* pAxis1Pos, unsigned int* pAxis1Speed, unsigned int* pAxis2Dir, unsigned int* pAxis2Pos, unsigned int* pAxis2Speed, unsigned int* pnDI, unsigned int arrSrcPos[8], unsigned int arrDstPos[8], unsigned int arrEcdPos[8], bool bResp = true);
	//
	//////////////////////////////////////////////////////////
	// Motion Control - Get Data(DO, DI, Source Postion, 1 ~ 9 Axis Alarm)
	DllExportExternC	BOOL			MotionControl__EQExtendData(LPEQUIPMENT_EXTEND_DATA pEQData, bool bResp = true);
	DllExportExternC	BOOL			MotionControl__EQExtendData_Port(int nPort, LPEQUIPMENT_EXTEND_DATA pEQData, bool bResp = true);
	DllExportExternC	BOOL			MotionControl__EQExtendData_Sock(LPEQUIPMENT_EXTEND_DATA pEQData, bool bResp = true);
	// MotionControlV2__DOF_and_Blower
	//	nRoll ~ nHeave : 0~20000
	//	nSpeed(RPM : 3000(max) ~ 0(min)
	//	nBlower : 0(min) ~ 100(max)
	DllExportExternC	BOOL			MotionControlV2__DOF_and_Blower(MOTION_DATA stMotionData);
	DllExportExternC	BOOL			MotionControlV2__DOF_and_Blower_Port(int nPort, MOTION_DATA stMotionData);
	DllExportExternC	BOOL			MotionControlV2__DOF_and_Blower_Sock(MOTION_DATA stMotionData);


	/// MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis
	//	nRoll ~ nHeave : 0~20000
	//	nSpeed(RPM : 3000(max) ~ 0(min)
	//	nBlower : 0(min) ~ 100(max)
	//  Rolling ~ Yawing : 0 ~ 36000
	//	nDO : bit flag
	//	nDI : bit flag
	DllExportExternC	BOOL			MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis(MOTION_EXTEND_DATA stMotionData, LPEQUIPMENT_DATA pEQData, bool bResp = true);
	DllExportExternC	BOOL			MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_Port(int nPort, MOTION_EXTEND_DATA stMotionData, LPEQUIPMENT_DATA pEQData, bool bResp = true);
	DllExportExternC	BOOL			MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_Sock(MOTION_EXTEND_DATA stMotionData, LPEQUIPMENT_DATA pEQData, bool bResp = true);
	// MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm
	//	nRoll ~ nHeave : 0~20000
	//	nSpeed(RPM : 3000(max) ~ 0(min)
	//	nBlower : 0(min) ~ 100(max)
	//  Rolling ~ Yawing : 0 ~ 36000
	//	nDO : bit flag
	//	nDI : bit flag
	DllExportExternC	BOOL			MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(MOTION_EXTEND_DATA stMotionData, LPEQUIPMENT_EXTEND_DATA pEQData, bool bResp = true);
	DllExportExternC	BOOL			MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm_Port(int nPort, MOTION_EXTEND_DATA stMotionData, LPEQUIPMENT_EXTEND_DATA pEQData, bool bResp = true);
	DllExportExternC	BOOL			MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm_Sock(MOTION_EXTEND_DATA stMotionData, LPEQUIPMENT_EXTEND_DATA pEQData, bool bResp = true);

	////////////////////////////////////////////////////////////////////////////////////////
	// Socket Parameter Setting Function
	///////////////////////////////////////////////////////////////////////////////////////
	DllExportExternC	BOOL			ReadTcpSocketMode(int* pData);
	DllExportExternC	BOOL			ReadTcpSocketMode_Port(int nPort, int* pData);
	DllExportExternC	BOOL			ReadTcpSocketMode_Sock(int* pData);

	DllExportExternC	BOOL			WriteTcpSocketMode(int nData);
	DllExportExternC	BOOL			WriteTcpSocketMode_Port(int nPort, int nData);
	DllExportExternC	BOOL			WriteTcpSocketMode_Sock(int nData);

	DllExportExternC	BOOL			ReadWifiMode(int* pData);
	DllExportExternC	BOOL			ReadWifiMode_Port(int nPort, int* pData);
	DllExportExternC	BOOL			ReadWifiMode_Sock(int* pData);

	DllExportExternC	BOOL			WriteWifiMode(int nData);
	DllExportExternC	BOOL			WriteWifiMode_Port(int nPort, int nData);
	DllExportExternC	BOOL			WriteWifiMode_Sock(int nData);
	
	DllExportExternC	BOOL			ReadWifiChannel(int* pData);
	DllExportExternC	BOOL			ReadWifiChannel_Port(int nPort, int* pData);
	DllExportExternC	BOOL			ReadWifiChannel_Sock(int* pData);

	DllExportExternC	BOOL			WriteWifiChannel(int nData);
	DllExportExternC	BOOL			WriteWifiChannel_Port(int nPort, int nData);
	DllExportExternC	BOOL			WriteWifiChannel_Sock(int nData);

	DllExportExternC	BOOL			ReadBluetoothDeviceId(char* szData);
	DllExportExternC	BOOL			ReadBluetoothDeviceId_Port(int nPort, char* szData);
	DllExportExternC	BOOL			ReadBluetoothDeviceId_Sock(char* szData);

	DllExportExternC	BOOL			WriteBluetoothDeviceId(const char* szData);
	DllExportExternC	BOOL			WriteBluetoothDeviceId_Port(int nPort, const char* szData);
	DllExportExternC	BOOL			WriteBluetoothDeviceId_Sock(const char* szData);

	DllExportExternC	BOOL			ReadBluetoothDevicePassword(char* szData);
	DllExportExternC	BOOL			ReadBluetoothDevicePassword_Port(int nPort, char* szData);
	DllExportExternC	BOOL			ReadBluetoothDevicePassword_Sock(char* szData);

	DllExportExternC	BOOL			WriteBluetoothDevicePassword(const char* szData);
	DllExportExternC	BOOL			WriteBluetoothDevicePassword_Port(int nPort, const char* szData);
	DllExportExternC	BOOL			WriteBluetoothDevicePassword_Sock(const char* szData);

	DllExportExternC	BOOL			ReadSocketServerIP(char* szData);
	DllExportExternC	BOOL			ReadSocketServerIP_Port(int nPort, char* szData);
	DllExportExternC	BOOL			ReadSocketServerIP_Sock(char* szData);

	DllExportExternC	BOOL			WriteSocketServerIP(const char* szData);
	DllExportExternC	BOOL			WriteSocketServerIP_Port(int nPort, const char* szData);
	DllExportExternC	BOOL			WriteSocketServerIP_Sock(const char* szData);

	DllExportExternC	BOOL			ReadSocketServerSubnetMask(char* szData);
	DllExportExternC	BOOL			ReadSocketServerSubnetMask_Port(int nPort, char* szData);
	DllExportExternC	BOOL			ReadSocketServerSubnetMask_Sock(char* szData);

	DllExportExternC	BOOL			WriteSocketServerSubnetMask(const char* szData);
	DllExportExternC	BOOL			WriteSocketServerSubnetMask_Port(int nPort, const char* szData);
	DllExportExternC	BOOL			WriteSocketServerSubnetMask_Sock(const char* szData);

	DllExportExternC	BOOL			ReadSocketServerGateway(char* szData);
	DllExportExternC	BOOL			ReadSocketServerGateway_Port(int nPort, char* szData);
	DllExportExternC	BOOL			ReadSocketServerGateway_Sock(char* szData);

	DllExportExternC	BOOL			WriteSocketServerGateway(const char* szData);
	DllExportExternC	BOOL			WriteSocketServerGateway_Port(int nPort, const char* szData);
	DllExportExternC	BOOL			WriteSocketServerGateway_Sock(const char* szData);

	DllExportExternC	BOOL			ReadSocketServerDNS(char* szData);
	DllExportExternC	BOOL			ReadSocketServerDNS_Port(int nPort, char* szData);
	DllExportExternC	BOOL			ReadSocketServerDNS_Sock(char* szData);

	DllExportExternC	BOOL			WriteSocketServerDNS(const char* szData);
	DllExportExternC	BOOL			WriteSocketServerDNS_Port(int nPort, const char* szData);
	DllExportExternC	BOOL			WriteSocketServerDNS_Sock(const char* szData);

	DllExportExternC	BOOL			ReadSocketServerPort(int* pData);
	DllExportExternC	BOOL			ReadSocketServerPort_Port(int nPort, int* pData);
	DllExportExternC	BOOL			ReadSocketServerPort_Sock(int* pData);

	DllExportExternC	BOOL			WriteSocketServerPort(int nData);
	DllExportExternC	BOOL			WriteSocketServerPort_Port(int nPort, int nData);
	DllExportExternC	BOOL			WriteSocketServerPort_Sock(int nData);
	
	DllExportExternC	BOOL			ReadSocketClientIP(char* szData);
	DllExportExternC	BOOL			ReadSocketClientIP_Port(int nPort, char* szData);
	DllExportExternC	BOOL			ReadSocketClientIP_Sock(char* szData);

	DllExportExternC	BOOL			WriteSocketClientIP(const char* szData);
	DllExportExternC	BOOL			WriteSocketClientIP_Port(int nPort, const char* szData);
	DllExportExternC	BOOL			WriteSocketClientIP_Sock(const char* szData);

	DllExportExternC	BOOL			ReadSocketClientSubnetMask(char* szData);
	DllExportExternC	BOOL			ReadSocketClientSubnetMask_Port(int nPort, char* szData);
	DllExportExternC	BOOL			ReadSocketClientSubnetMask_Sock(char* szData);

	DllExportExternC	BOOL			WriteSocketClientSubnetMask(const char* szData);
	DllExportExternC	BOOL			WriteSocketClientSubnetMask_Port(int nPort, const char* szData);
	DllExportExternC	BOOL			WriteSocketClientSubnetMask_Sock(const char* szData);

	DllExportExternC	BOOL			ReadSocketClientGateway(char* szData);
	DllExportExternC	BOOL			ReadSocketClientGateway_Port(int nPort, char* szData);
	DllExportExternC	BOOL			ReadSocketClientGateway_Sock(char* szData);

	DllExportExternC	BOOL			WriteSocketClientGateway(const char* szData);
	DllExportExternC	BOOL			WriteSocketClientGateway_Port(int nPort, const char* szData);
	DllExportExternC	BOOL			WriteSocketClientGateway_Sock(const char* szData);

	DllExportExternC	BOOL			ReadSocketClientDNS(char* szData);
	DllExportExternC	BOOL			ReadSocketClientDNS_Port(int nPort, char* szData);
	DllExportExternC	BOOL			ReadSocketClientDNS_Sock(char* szData);

	DllExportExternC	BOOL			WriteSocketClientDNS(const char* szData);
	DllExportExternC	BOOL			WriteSocketClientDNS_Port(int nPort, const char* szData);
	DllExportExternC	BOOL			WriteSocketClientDNS_Sock(const char* szData);


	///////////////////////////////////////////////////////////////////////////////////////