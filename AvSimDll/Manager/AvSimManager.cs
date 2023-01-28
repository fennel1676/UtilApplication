using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AvSimDll.Manager
{
    public partial class AvSimManager
    {
        private IntPtr m_pDlgManager = IntPtr.Zero;

        public int Initial(bool isUsedMultiManager = false)
        {
            if (isUsedMultiManager)
            {
                m_pDlgManager = AvSimManager__InitialEx();
                return (IntPtr.Zero == m_pDlgManager) ? 0 : 1;
            }
            else
                return AvSimManager__Initial();
        }

        public int Destroy()
        {
            if (IntPtr.Zero == m_pDlgManager)
                return AvSimManager__Destroy();
            else
                return AvSimManager__DestroyEx(m_pDlgManager);
        }

        public int Show()
        {
            if (IntPtr.Zero == m_pDlgManager)
                return AvSimManager__Show();
            else
                return AvSimManager__ShowEx(m_pDlgManager);
        }

        public int Hide()
        {
            if (IntPtr.Zero == m_pDlgManager)
                return AvSimManager__Hide();
            else
                return AvSimManager__HideEx(m_pDlgManager);
        }

        public int CheckLoadGame()
        {
            if (IntPtr.Zero == m_pDlgManager)
                return AvSimManager__CheckLoadGame();
            else
                return AvSimManager__CheckLoadGameEx(m_pDlgManager);
        }

        public int GameShow()
        {
            if (IntPtr.Zero == m_pDlgManager)
                return AvSimManager__GameShow();
            else
                return AvSimManager__GameShowEx(m_pDlgManager);
        }

        public int GameHide()
        {
            if (IntPtr.Zero == m_pDlgManager)
                return AvSimManager__GameHide();
            else
                return AvSimManager__GameHideEx(m_pDlgManager);
        }

        public int SetCommand<T>(T stData)
        {
            IntPtr pData = Marshal.AllocHGlobal(Marshal.SizeOf(stData));
            try
            {
                Marshal.StructureToPtr(stData, pData, false);
                if (IntPtr.Zero == m_pDlgManager)
                    return AvSimManager__SetCommand(pData, Marshal.SizeOf(stData));
                else
                    return AvSimManager__SetCommandEx(m_pDlgManager, pData, Marshal.SizeOf(stData));
            }
            finally
            {
                Marshal.FreeHGlobal(pData);
            }
        }

        public int GetEvent<T>(T stEvtData)
        {
            IntPtr pData = Marshal.AllocHGlobal(Marshal.SizeOf(stEvtData));
            try
            {
                Marshal.StructureToPtr(stEvtData, pData, false);
                if (IntPtr.Zero == m_pDlgManager)
                    return AvSimManager__GetEvent(pData);
                else
                    return AvSimManager__GetEventEx(m_pDlgManager, pData);
            }
            finally
            {
                Marshal.FreeHGlobal(pData);
            }
        }

        public int SetSimulatorMode<T>(T stEvtData)
        {
            IntPtr pData = Marshal.AllocHGlobal(Marshal.SizeOf(stEvtData));
            try
            {
                Marshal.StructureToPtr(stEvtData, pData, false);
                if (IntPtr.Zero == m_pDlgManager)
                    return AvSimManager__SetSimulatorMode(pData);
                else
                    return AvSimManager__SetSimulatorModeEx(m_pDlgManager, pData);
            }
            finally
            {
                Marshal.FreeHGlobal(pData);
            }
        }

        public int GetSimulatorMode<T>(T stEvtData)
        {
            IntPtr pData = Marshal.AllocHGlobal(Marshal.SizeOf(stEvtData));
            try
            {
                Marshal.StructureToPtr(stEvtData, pData, false);
                if (IntPtr.Zero == m_pDlgManager)
                    return AvSimManager__GetSimulatorMode(pData);
                else
                    return AvSimManager__GetSimulatorModeEx(m_pDlgManager, pData);
            }
            finally
            {
                Marshal.FreeHGlobal(pData);
            }
        }
    }
}
