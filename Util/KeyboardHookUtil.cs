using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKI.Util
{
    public class KeyboardHookUtil
    {
        private List<Keys> HookedKeys = new List<Keys>();
        private IntPtr hhook = IntPtr.Zero;
        private AKI.Win32.API.User32.keyboardHookProc callbackkeyboardHookProc = null;
        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;

        public bool Hook()
        {
            IntPtr hInstance = AKI.Win32.API.Kernel32.LoadLibrary("User32");
            callbackkeyboardHookProc = new AKI.Win32.API.User32.keyboardHookProc(hookProc);
            hhook = AKI.Win32.API.User32.SetWindowsHookEx((int)AKI.Win32.WH.KEYBOARD_LL, callbackkeyboardHookProc, hInstance, 0);
            return IntPtr.Zero != hhook ? true : false;
        }

        public bool Unhook()
        {
            if (callbackkeyboardHookProc == null)
                return false;
            bool ok = AKI.Win32.API.User32.UnhookWindowsHookEx(hhook);
            callbackkeyboardHookProc = null;
            return AKI.Win32.API.User32.UnhookWindowsHookEx(hhook);
        }

        public void AddKey(Keys cKey)
        {
            if (!HookedKeys.Contains(cKey))
                HookedKeys.Add(cKey);
        }

        public void RemoveKey(Keys cKey)
        {
            HookedKeys.Remove(cKey);
        }

        public void ClearKey()
        {
            HookedKeys.Clear();
        }

        public int hookProc(int code, int wParam, ref AKI.Win32.API.User32.keyboardHookStruct lParam)
        {
            if (code >= 0)
            {
                Keys key = (Keys)lParam.vkCode;
                if (HookedKeys.Contains(key))
                {
                    KeyEventArgs kea = new KeyEventArgs(key);
                    if ((wParam == AKI.Win32.WM.KEYDOWN || wParam == AKI.Win32.WM.SYSKEYDOWN) && (KeyDown != null))
                    {
                        KeyDown(this, kea);
                    }
                    else if ((wParam == AKI.Win32.WM.KEYUP || wParam == AKI.Win32.WM.SYSKEYUP) && (KeyUp != null))
                    {
                        KeyUp(this, kea);
                    }
                }
            }
            return AKI.Win32.API.User32.CallNextHookEx(hhook, code, wParam, ref lParam);
        }
    }
}
