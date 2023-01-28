AvSimDll.dll 이 C#으로 매핑한 라이브러리로 이걸 레퍼런스로 추가

using AvSimDll.MotionExternC;

추가

MotionControl 클래스로 제어

Initial(); - AvSimDllMotionExternC_Environment.ini 파일에 포트값 지정해서 사용하려면 이걸 사용
Initial(Port); - 각 프로그램에서 따로 포트값 관리 하면 이걸 사용
Initial(IP, Port); - TCP 통신이 가능한 모델이면 이걸 사용

DOF_and_Blower(Roll, Pitch, Yaw, Sway, Surge, Heave, MotionSpeed, Blower);
 - 가장 기본적인 동작 메소드 지원하지 않는 DOF 값이 있으면 알아서 무시

Destroy(); - 포트 잡고 있는거 해제