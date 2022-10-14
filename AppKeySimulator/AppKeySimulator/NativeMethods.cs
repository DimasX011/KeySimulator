using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;


namespace GtaAppKeySimulator
{
    public class NativeMethods
    {

        public static void AppClick(string app, string key)
        {
            while (IsWindow(FindWindow(null, app)) == false)
            {
                Thread.Sleep(1);
            }
            IntPtr calcWindow = FindWindow(null, $"{app}");
            if (SetForegroundWindow(calcWindow))
                //ModifiedKeyStroke(Key.Tab);
                    SendKeys.SendWait($"{key}");
        }


        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void ModifiedKeyStroke(Key key, ModifierKeys modifiers = ModifierKeys.None)
        {
             NativeMethods.INPUT BuildINPUT(Key k, NativeMethods.KeyboardFlags flags) => new NativeMethods.INPUT
            {
                type = NativeMethods.InputType.Keyboard,
                union = new NativeMethods.InputUnion { keyboard = new NativeMethods.KEYBDINPUT { virtualKey = (ushort)KeyInterop.VirtualKeyFromKey(k), scanCode = 0, flags = flags, timeStamp = 0, extraInfo = IntPtr.Zero } }
            };
            List<Key> keys = new List<Key>();
            if (modifiers.HasFlag(ModifierKeys.Control)) keys.Add(Key.LeftCtrl);
            if (modifiers.HasFlag(ModifierKeys.Alt)) keys.Add(Key.LeftAlt);
            if (modifiers.HasFlag(ModifierKeys.Shift)) keys.Add(Key.LeftShift);
            if (modifiers.HasFlag(ModifierKeys.Windows)) keys.Add(Key.LWin);
            keys.Add(key);
            NativeMethods.INPUT[] inputs = new NativeMethods.INPUT[keys.Count * 2];
            for (int i = 0; i < keys.Count; i++)
            {
                inputs[i] = BuildINPUT(keys[i], NativeMethods.KeyboardFlags.None);
                inputs[(i + 1)] = BuildINPUT(keys[i], NativeMethods.KeyboardFlags.KeyUp);
            }
            _ = NativeMethods.SendInput(inputs.Length, inputs, NativeMethods.SizeOfInput);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string className, string windowName);


        [Flags]
        public enum KeyboardFlags : uint
        {
            None = 0,

            /// <summary>
            /// KEYEVENTF_EXTENDEDKEY = 0x0001 (If specified, the scan code was preceded by a prefix byte that has the value 0xE0 (224).)
            /// </summary>
            ExtendedKey = 1,

            /// <summary>
            /// KEYEVENTF_KEYUP = 0x0002 (If specified, the key is being released. If not specified, the key is being pressed.)
            /// </summary>
            KeyUp = 2,

            /// <summary>
            /// KEYEVENTF_UNICODE = 0x0004 (If specified, wScan identifies the key and wVk is ignored.)
            /// </summary>
            Unicode = 4,

            /// <summary>
            /// KEYEVENTF_SCANCODE = 0x0008 (Windows 2000/XP: If specified, the system synthesizes a VK_PACKET keystroke. The wVk parameter must be zero. This flag can only be combined with the KEYEVENTF_KEYUP flag. For more information, see the Remarks section.)
            /// </summary>
            ScanCode = 8,
        }

        [Flags]
        public enum MouseFlags : uint
        {
            /// <summary>
            /// Specifies that movement occurred.
            /// </summary>
            Move = 0x0001,

            /// <summary>
            /// Specifies that the left button was pressed.
            /// </summary>
            LeftDown = 0x0002,

            /// <summary>
            /// Specifies that the left button was released.
            /// </summary>
            LeftUp = 0x0004,

            /// <summary>
            /// Specifies that the right button was pressed.
            /// </summary>
            RightDown = 0x0008,

            /// <summary>
            /// Specifies that the right button was released.
            /// </summary>
            RightUp = 0x0010,

            /// <summary>
            /// Specifies that the middle button was pressed.
            /// </summary>
            MiddleDown = 0x0020,

            /// <summary>
            /// Specifies that the middle button was released.
            /// </summary>
            MiddleUp = 0x0040,

            /// <summary>
            /// Windows 2000/XP: Specifies that an X button was pressed.
            /// </summary>
            XDown = 0x0080,

            /// <summary>
            /// Windows 2000/XP: Specifies that an X button was released.
            /// </summary>
            XUp = 0x0100,

            /// <summary>
            /// Windows NT/2000/XP: Specifies that the wheel was moved, if the mouse has a wheel. The amount of movement is specified in mouseData. 
            /// </summary>
            VerticalWheel = 0x0800,

            /// <summary>
            /// Specifies that the wheel was moved horizontally, if the mouse has a wheel. The amount of movement is specified in mouseData. Windows 2000/XP:  Not supported.
            /// </summary>
            HorizontalWheel = 0x1000,

            /// <summary>
            /// Windows 2000/XP: Maps coordinates to the entire desktop. Must be used with MOUSEEVENTF_ABSOLUTE.
            /// </summary>
            VirtualDesk = 0x4000,

            /// <summary>
            /// Specifies that the dx and dy members contain normalized absolute coordinates. If the flag is not set, dxand dy contain relative data (the change in position since the last reported position). This flag can be set, or not set, regardless of what kind of mouse or other pointing device, if any, is connected to the system. For further information about relative mouse motion, see the following Remarks section.
            /// </summary>
            Absolute = 0x8000,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort virtualKey;
            public ushort scanCode;
            public KeyboardFlags flags;
            public uint timeStamp;
            public IntPtr extraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int deltaX;
            public int deltaY;
            public uint mouseData;
            public MouseFlags flags;
            public uint time;
            public IntPtr extraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public uint message;
            public ushort wParamL;
            public ushort wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mouse;
            [FieldOffset(0)]
            public KEYBDINPUT keyboard;
            [FieldOffset(0)]
            public HARDWAREINPUT hardware;
        }
        public enum InputType : int
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }
        public struct INPUT
        {
            public InputType type;
            public InputUnion union;
        }

        public static int SizeOfInput { get; } = Marshal.SizeOf(typeof(INPUT));

        [DllImport("user32.dll")]
        public static extern uint SendInput(int nInputs, INPUT[] pInputs, int cbSize);
    }
}
