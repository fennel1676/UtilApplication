using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvSimDevKit
{
    public enum eSimulatorMode
    {
        None = 0,				                // Version Mode - NONE
        Axis6_6Dof_Triangle =3,				    // Version Mode - 6DOF Triangle 
        Axis6_6Dof_Icarus_Sky = 10,				// Version Mode - 6DOF Icarus Sky
        Axis3_3Dof_Ski_Jump = 12,				// Version Mode - 3DOF Ski Jump
        Axis2_2Dof_Cube = 13,				    // Version Mode - 2DOF Cube
        Axis6_6Dof_Mine = 14,				    // Version Mode - 6DOF Mine
        Axis3_3Dof_Skonec_4D = 16,				// Version Mode - 3DOF Skonec 4D
        Axis2_2Dof_Parachute_Main = 17,			// Version Mode - 2DOF Parachute Main Ctrl
        Axis3_3Dof_Parachute_Main = 18,			// Version Mode - 3DOF Parachute Main Ctrl
        Axis1_1Dof_Parachute_Sub = 19,			// Version Mode - 1DOF Parachute Sub Ctrl
        Axis1_1Dof_Parachute_Sub_Initial = 20,	// Version Mode - 1DOF Parachute Sub Ctrl - Initial Mode
        Axis6_6Dof_Mine_Vive = 21,				// Version Mode - 6DOF Mine - Vive
        Axis0_0Dof_Walking_System = 22,			// Version Mode - 0DOF Walking System
        Axis0_0Dof_Walking_System_Initial = 23,	// Version Mode - 0DOF Walking System - Initial Mode
        Axis1_1Dof_Walking_System = 24,			// Version Mode - 1DOF Walking System
        Axis1_1Dof_Walking_System_Initial = 25,	// Version Mode - 1DOF Walking System - Iniital Mode
        Axis3_3Dof_Skonec_4D_Yaw = 26,			// Version Mode - 3DOF Skonec 4D - Yaw version
        Axis2_2Dof_Seesaw = 27,				    // Version Mode - 2DOF Seesaw
        Axis0_0Dof_Elliptial = 28,				// Version Mode - 0DOF Elliptical
        Axis7_7Dof_Drum_1P = 29,				// Version Mode - 6DOF Drum 1Person
        Axis7_7Dof_Drum_2P = 30,				// Version Mode - 6DOF Drum 2Person
        Axis3_3Dof_Ski_Junp_VER2 = 31,			// Version Mode - 3DOF Ski Jump - Version 2
        Axis3_3Dof_Sofa_2P = 32,				// Version Mode - 3DOF Sofa 2P
    }
}
