using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AvSimDll.Manager
{
    public partial class AvSimManager
    {
        [DllImport("AVSimDllManager.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int AvSimManager__Initial();

        [DllImport("AVSimDllManager.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static IntPtr AvSimManager__InitialEx();

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__Destroy();

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__DestroyEx(IntPtr pDlgManager);

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__Show();

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__ShowEx(IntPtr pDlgManager);

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__Hide();

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__HideEx(IntPtr pDlgManager);
        
        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__CheckLoadGame();

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__CheckLoadGameEx(IntPtr pDlgManager);

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__GameShow();

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__GameShowEx(IntPtr pDlgManager);

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__GameHide();

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__GameHideEx(IntPtr pDlgManager);

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__SetCommand(IntPtr pCmdData, int nSize);

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__SetCommandEx(IntPtr pDlgManager, IntPtr pCmdData, int nSize);

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__GetEvent(IntPtr pEvtData);

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__GetEventEx(IntPtr pDlgManager, IntPtr pEvtData);

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__SetSimulatorMode(IntPtr pEvtData);

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__SetSimulatorModeEx(IntPtr pDlgManager, IntPtr pEvtData);

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__GetSimulatorMode(IntPtr pEvtData);

        [DllImport("AVSimDllManager.dll")]
        private extern static int AvSimManager__GetSimulatorModeEx(IntPtr pDlgManager, IntPtr pEvtData);
    }
}
