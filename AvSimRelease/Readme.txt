AvSimDll.dll �� C#���� ������ ���̺귯���� �̰� ���۷����� �߰�

using AvSimDll.MotionExternC;

�߰�

MotionControl Ŭ������ ����

Initial(); - AvSimDllMotionExternC_Environment.ini ���Ͽ� ��Ʈ�� �����ؼ� ����Ϸ��� �̰� ���
Initial(Port); - �� ���α׷����� ���� ��Ʈ�� ���� �ϸ� �̰� ���
Initial(IP, Port); - TCP ����� ������ ���̸� �̰� ���

DOF_and_Blower(Roll, Pitch, Yaw, Sway, Surge, Heave, MotionSpeed, Blower);
 - ���� �⺻���� ���� �޼ҵ� �������� �ʴ� DOF ���� ������ �˾Ƽ� ����

Destroy(); - ��Ʈ ��� �ִ°� ����