// Copyright © 2018-2022 Fullham Alfayet 
//
// veitcstd is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// veitcstd is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if VEITCSTD_64BIT_MODE
using size_t = System.UInt64;
using voidptr = System.UInt64;
using ivoidptr = System.Int64;
#else
using size_t = System.UInt32;
using voidptr = System.UInt32;
using ivoidptr = System.Int32;
#endif

namespace veitcstd
{
#if SIMS3VERSION_1672
    public static class vnativefunctions 
    {
        public struct filedata
        {
            public long len;
            public IntPtr data;
            public IntPtr handle;
        }

        [Flags]
        public enum mbtype : uint
        {
            MB_OK = 0x00000000u,
            MB_OKCANCEL = 0x00000001u,
            MB_ABORTRETRYIGNORE = 0x00000002u,
            MB_YESNOCANCEL = 0x00000003u,
            MB_YESNO = 0x00000004u,
            MB_RETRYCANCEL = 0x00000005u,

            MB_CANCELTRYCONTINUE = 0x00000006u,

            MB_ICONHAND = 0x00000010u,
            MB_ICONQUESTION = 0x00000020u,
            MB_ICONEXCLAMATION = 0x00000030u,
            MB_ICONASTERISK = 0x00000040u,

            MB_USERICON = 0x00000080u,

            MB_DEFBUTTON1 = 0x00000000u,
            MB_DEFBUTTON2 = 0x00000100u,
            MB_DEFBUTTON3 = 0x00000200u,

            MB_DEFBUTTON4 = 0x00000300u,

            MB_APPLMODAL = 0x00000000u,
            MB_SYSTEMMODAL = 0x00001000u,
            MB_TASKMODAL = 0x00002000u,

            MB_HELP = 0x00004000u, // Heup Button

            MB_NOFOCUS = 0x00008000u,
            MB_SETFOREGROUND = 0x00010000u,
            MB_DEFAULT_DESKTOP_ONLY = 0x00020000u,

            MB_TOPMOST = 0x00040000u,
            MB_RIGHT = 0x00080000u,
            MB_RTLREADING = 0x00100000u,

            MB_SERVICE_NOTIFICATION = 0x00200000u,

            MB_SERVICE_NOTIFICATION2 = 0x00040000u,

            MB_SERVICE_NOTIFICATION_NT3X = 0x00040000u,

            MB_TYPEMASK = 0x0000000Fu,
            MB_ICONMASK = 0x000000F0u,
            MB_DEFMASK = 0x00000F00u,
            MB_MODEMASK = 0x00003000u,
            MB_MISCMASK = 0x0000C000u
        }

        public enum mbresult : int
        {
            IDOK = 1,
            IDCANCEL = 2,
            IDABORT = 3,
            IDRETRY = 4,
            IDIGNORE = 5,
            IDYES = 6,
            IDNO = 7,
            IDCLOSE = 8,
            IDHELP = 9
        }

#if VEITC_AUTO_INIT_CCTOR
        static vnativefunctions()
        {
            try
            {
                vdelegate.CallvFunction(init_class);
            }
            catch (Exception ex)
            { libcs.assert(ex.ToString()); }
        }
#endif

        public static bool creaing = false;

        internal static bool cache_done_veitc_native_debug_text_to_debugger = false;
        internal static bool cache_done_veitc_native_load_library           = false;
        internal static bool cache_done_IsDebuggerAttached_internal         = false;
        internal static bool cache_done_IsDebuggerBreak_internal            = false;
        internal static bool cache_done_veitc_native_ptr_fs_zero            = false;
        internal static bool cache_done_veitc_native_message_box            = false;
        internal static bool cache_done_veitc_native_cpuid                  = false;
        internal static bool cache_done_veitc_native_file_create            = false;
        internal static bool cache_done_veitc_native_file_writetext         = false;
        internal static bool cache_done_veitc_native_file_write             = false;
        internal static bool cache_done_veitc_native_close_handle           = false;
        internal static bool cache_done_veitc_native_file_found             = false;
        internal static bool cache_done_veitc_native_obj_get_address        = false;

        private static bool done_init_class                                 = false;

        private const string native00 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM0";
        private const string native01 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM1";
        private const string native02 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM2";
        private const string native03 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM3";
        private const string native04 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM4";
        private const string native05 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM5";
        private const string native06 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM6";
        private const string native07 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM7";
        private const string native08 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM8";
        private const string native09 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM9";
        private const string native10 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM10";
        private const string native11 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM11";
        private const string native12 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM12";
        private const string native13 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM13";
        private const string native14 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM14";
        private const string native15 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM15";
        private const string native16 = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvM16";

        public unsafe static void init_class()
        {
#if !SIMS3VERSION_1672
#error Game versions not supported. Only Patch 1.67.2
#else
            if (done_init_class)
                return;
            creaing = true;
            done_init_class = true;

            InvalidFileNameChars = new char[] // len: 41
            {
            	'"',
            	'<',
            	'>',
            	'|',
            	'\0',
            	'\u0001',
            	'\u0002',
            	'\u0003',
            	'\u0004',
            	'\u0005',
            	'\u0006',
            	'\a',
            	'\b',
            	'\t',
            	'\n',
            	'\v',
            	'\f',
            	'\r',
            	'\u000e',
            	'\u000f',
            	'\u0010',
            	'\u0011',
            	'\u0012',
            	'\u0013',
            	'\u0014',
            	'\u0015',
            	'\u0016',
            	'\u0017',
            	'\u0018',
            	'\u0019',
            	'\u001a',
            	'\u001b',
            	'\u001c',
            	'\u001d',
            	'\u001e',
            	'\u001f',
            	':',
            	'*',
            	'?',
            	'\\',
            	'/'
            };

            var type = typeof(vnativefunctions);
            var m01 = (MonoMethod)vmethod.GetMethodNameFastS(type,"veitc_native_load_library");

            uint func_address = 0;
            int tem = 0;

            if (m01 == null)
            {
                //libcs.breakf();
                libcs.assert("type.GetMethod(\"veitc_native_load_library\") failed");
                done_init_class = false;
                goto skip2;
            }

            tem = native00.Length + native00.Length
                - 0x32;

            var native00_address = (uint)native00.obj_address().value;
            func_address = native00_address + 0x14;


            for (int i = 0x10; i < tem; i++)
            {
                *(uint*)(native00_address + i) = 0xCCCCCCCC;
            }

            native00.obj_address(); // check SIGSEGV 

            *(uint*)(func_address)        =  libcs.swap32(0x908B5C24u);
            *(uint*)(func_address + 0x4)  =  libcs.swap32(0x0483C308u);
            *(uint*)(func_address + 0x8)  =  libcs.swap32(0x53FF1580u);
            *(uint*)(func_address + 0xC)  =  libcs.swap32(0x62F90090u);
            *(uint*)(func_address + 0x10) =  libcs.swap32(0xC3909090u);

            cache_done_veitc_native_load_library =
                vmethod.SetNativeFunctionAdderss(m01.mhandle, libcs.tointptr(func_address));

            ////////////////////////////////////////////////////////////////////////////////////////////////////////

            skip2:
            var m02 = (MonoMethod)vmethod.GetMethodNameFastS(type, "veitc_native_debug_text_to_debugger");
            if (m02 == null)
            {
                libcs.assert("type.GetMethod(\"veitc_native_debug_text_to_debugger\") failed");
                done_init_class = false;
                goto skip3;
            }

            tem = native01.Length + native01.Length
                - 0x32;

            var native01_address = (uint)native01.obj_address().value;
            func_address = native01_address + 0x14;

            for (int i = 0x10; i < tem; i++)
            {
                *(uint*)(native01_address + i) = 0xCCCCCCCC;
            }

            native01.obj_address(); // check SIGSEGV 

            *(uint*)(func_address)        = libcs.swap32(0x608B4C24u);
            *(uint*)(func_address + 0x4)  = libcs.swap32(0x2483C108u);
            *(uint*)(func_address + 0x8)  = libcs.swap32(0x51FF1514u);
            *(uint*)(func_address + 0xC)  = libcs.swap32(0x63F90061u);
            *(uint*)(func_address + 0x10) = libcs.swap32(0xC3909090u);

            cache_done_veitc_native_debug_text_to_debugger = 
                vmethod.SetNativeFunctionAdderss(m02.mhandle, libcs.tointptr(func_address));

            ////////////////////////////////////////////////////////////////////////////////////////////////////////

            skip3:
            var m03 = (MonoMethod)vmethod.GetMethodNameFastS(type, "IsDebuggerAttached_internal");
            if (m03 == null)
            {
                libcs.assert("type.GetMethod(\"IsDebuggerAttached_internal\") failed");
                goto skip4;
            }

            tem = native02.Length + native02.Length
                - 0x32;

            var native02_address = (uint)native02.obj_address().value;
            func_address = native02_address + 0x14;

            for (int i = 0x10; i < tem; i++)
            {
                *(uint*)(native02_address + i) = 0xCCCCCCCC;
            }

            native02.obj_address(); // check SIGSEGV 

            *(uint*)(func_address)       = libcs.swap32(0x64A13000u);
            *(uint*)(func_address + 0x4) = libcs.swap32(0x00000FB6u);
            *(uint*)(func_address + 0x8) = libcs.swap32(0x4002C390u);

            cache_done_IsDebuggerAttached_internal =
                vmethod.SetNativeFunctionAdderss(m03.mhandle, libcs.tointptr(func_address));

            ////////////////////////////////////////////////////////////////////////////////

            skip4:
            var m04 = (MonoMethod)vmethod.GetMethodNameFastS(type, "DebuggerBreak");
            if (m04 == null)
            {
                libcs.assert("type.GetMethod(\"DebuggerBreak\") failed");
                goto skip5;
            }

            tem = native03.Length + native03.Length
                - 0x32;

            var native03_address = (uint)native03.obj_address().value;
            func_address = native03_address + 0x14;

            for (int i = 0x10; i < tem; i++)
            {
                *(uint*)(native03_address + i) = 0xCCCCCCCC;
            }

            native03.obj_address(); // check SIGSEGV 

            *(uint*)(func_address) = libcs.swap32(0xCCC3CCCCu);

            cache_done_IsDebuggerBreak_internal =
                vmethod.SetNativeFunctionAdderss(m04.mhandle, libcs.tointptr(func_address));

            ////////////////////////////////////////////////////////////////////////////////

            skip5:
            var m05 = (MonoMethod)vmethod.GetMethodNameFastS(type, "veitc_native_ptr_fs_zero");
            if (m05 == null)
            {
                libcs.assert("type.GetMethod(\"veitc_native_ptr_fs_zero\") failed");
                goto skip6;
            }

            tem = native04.Length + native04.Length
                - 0x32;

            var native04_address = (uint)native04.obj_address().value;
            func_address = native04_address + 0x14;

            for (int i = 0x10; i < tem; i++)
            {
                *(uint*)(native04_address + i) = 0xCCCCCCCC;
            }

            native04.obj_address(); // check SIGSEGV 

            *(uint*)(func_address)       = libcs.swap32(0x8B5C2404u);
            *(uint*)(func_address + 0x4) = libcs.swap32(0x648B0331u);
            *(uint*)(func_address + 0x8) = libcs.swap32(0xDBC3CCCCu);

            cache_done_veitc_native_ptr_fs_zero =
                vmethod.SetNativeFunctionAdderss(m05.mhandle, libcs.tointptr(func_address));

            ////////////////////////////////////////////////////////////////////////////////

            skip6:
            var m06 = (MonoMethod)vmethod.GetMethodNameFastS(type, "veitc_native_message_box");
            if (m06 == null)
            {
                libcs.assert("type.GetMethod(\"veitc_native_message_box\") failed");
                goto skip7;
            }

            tem = native05.Length + native05.Length
                - 0x32;

            var native05_address = (uint)native05.obj_address().value;
            func_address = native05_address + 0x14;

            for (int i = 0x10; i < tem; i++)
            {
                *(uint*)(native05_address + i) = 0xCCCCCCCC;
            }

            native05.obj_address(); // check SIGSEGV 

            byte[] nativefunc_messagebox = {
            	0x8B, 0x5C, 0x24, 0x08, 0x83, 0xC3, 0x08, 0x89, 0x5C, 0x24, 0x08, 0x8B,
            	0x5C, 0x24, 0x0C, 0x83, 0xC3, 0x08, 0x89, 0x5C, 0x24, 0x0C, 0x83, 0xEC,
            	0x08, 0x8B, 0x5C, 0x24, 0x08, 0x89, 0x5C, 0x24, 0x04, 0xFF, 0x74, 0x24,
            	0x18, 0xFF, 0x74, 0x24, 0x18, 0xFF, 0x74, 0x24, 0x18, 0xFF, 0x74, 0x24,
            	0x18, 0xFF, 0x15, 0x00, 0x68, 0xF9, 0x00, 0x83, 0xC4, 0x08, 0xC3, 0xCC
            };

            for (int i = 0; i < nativefunc_messagebox.Length; i++)
            {
                *(byte*)(func_address + i) = nativefunc_messagebox[i];
            }

            cache_done_veitc_native_message_box =
                vmethod.SetNativeFunctionAdderss(m06.mhandle, libcs.tointptr(func_address));

            ////////////////////////////////////////////////////////////////////////////////

            skip7:
            var m07 = (MonoMethod)vmethod.GetMethodNameFastS(type, "veitc_native_cpuid");
            if (m07 == null)
            {
                libcs.assert("type.GetMethod(\"veitc_native_cpuid\") failed");
                goto skip8;
            }

            tem = native06.Length + native06.Length
                - 0x32;

            var native06_address = (uint)native06.obj_address().value;
            func_address = native06_address + 0x14;

            for (int i = 0x10; i < tem; i++)
            {
                *(uint*)(native06_address + i) = 0xCCCCCCCC;
            }

            native06.obj_address(); // check SIGSEGV 

            byte[] nativefunc_cpuid = {
	            0x60, 0x8B, 0x44, 0x24, 0x24, 0x8B, 0x5C, 0x24, 0x28, 0x8B, 0x4C, 0x24,
	            0x2C, 0x8B, 0x54, 0x24, 0x30, 0x8B, 0x00, 0x8B, 0x1B, 0x8B, 0x09, 0x8B,
	            0x12, 0x0F, 0xA2, 0x55, 0x89, 0xE5, 0x83, 0xEC, 0x10, 0x89, 0x04, 0x24,
	            0x89, 0x5C, 0x24, 0x04, 0x89, 0x4C, 0x24, 0x08, 0x89, 0x54, 0x24, 0x0C,
	            0x8B, 0x44, 0x24, 0x38, 0x8B, 0x5C, 0x24, 0x3C, 0x8B, 0x4C, 0x24, 0x40,
	            0x8B, 0x54, 0x24, 0x44, 0x8B, 0x3C, 0x24, 0x89, 0x38, 0x8B, 0x7C, 0x24,
	            0x04, 0x89, 0x3B, 0x8B, 0x7C, 0x24, 0x08, 0x89, 0x39, 0x8B, 0x7C, 0x24,
	            0x0C, 0x89, 0x3A, 0x83, 0xC4, 0x10, 0x5D, 0x61, 0xC3
            };

            for (int i = 0; i < nativefunc_cpuid.Length; i++)
            {
                *(byte*)(func_address + i) = nativefunc_cpuid[i];
            }

            cache_done_veitc_native_cpuid =
                vmethod.SetNativeFunctionAdderss(m07.mhandle, libcs.tointptr(func_address));

            ////////////////////////////////////////////////////////////////////////////////

            skip8:
            var m08 = (MonoMethod)vmethod.GetMethodNameFastS(type, "veitc_native_file_create");
            if (m08 == null)
            {
                libcs.assert("type.GetMethod(\"veitc_native_file_create\") failed");
                goto skip9;
            }

            tem = native07.Length + native07.Length
                - 0x32;

            var native07_address = (uint)native07.obj_address().value;
            func_address = native07_address + 0x14;

            for (int i = 0x10; i < tem; i++)
            {
                *(uint*)(native07_address + i) = 0xCCCCCCCC;
            }

            native07.obj_address(); // check SIGSEGV 

            byte[] createfile_func = {
            	0x8B, 0x5C, 0x24, 0x04, 0x83, 0xC3, 0x08, 0x90, 0x90, 0x90, 0x90, 0x90,
            	0x90, 0x90, 0x31, 0xC0, 0x50, 0x68, 0x00, 0x00, 0x00, 0x80, 0x6A, 0x02,
            	0x50, 0x6A, 0x03, 0x68, 0x00, 0x00, 0x00, 0x40, 0x53, 0x90, 0x90, 0x90,
            	0x90, 0xFF, 0x15, 0x54, 0x63, 0xF9, 0x00, 0x31, 0xDB, 0xC3, 0x90, 0x90,
            	0x90, 0x90, 0x90, 0x90
            };

            for (int i = 0; i < createfile_func.Length; i++)
            {
                *(byte*)(func_address + i) = createfile_func[i];
            }

            cache_done_veitc_native_file_create =
                vmethod.SetNativeFunctionAdderss(m08.mhandle, libcs.tointptr(func_address));

            ////////////////////////////////////////////////////////////////////////////////

            skip9:
            var m09 = (MonoMethod)vmethod.GetMethodNameFastS(type, "veitc_native_file_writetext");
            if (m09 == null)
            {
                libcs.assert("type.GetMethod(\"veitc_native_file_writetext\") failed");
                goto skip10;
            }

            tem = native08.Length + native08.Length
                - 0x32;

            var native08_address = (uint)native08.obj_address().value;
            func_address = native08_address + 0x14;

            for (int i = 0x10; i < tem; i++)
            {
                *(uint*)(native08_address + i) = 0xCCCCCCCC;
            }

            native08.obj_address(); // check SIGSEGV 

            byte[] writetext_func = {
	            0x8B, 0x44, 0x24, 0x08, 0x83, 0xC0, 0x08, 0x8B, 0x5C, 0x24, 0x08, 0x83,
	            0xC3, 0x04, 0x8B, 0x1B, 0x01, 0xDB, 0x90, 0x90, 0x90, 0x31, 0xD2, 0x52,
	            0x52, 0x53, 0x50, 0xFF, 0x74, 0x24, 0x14, 0xFF, 0x15, 0x94, 0x63, 0xF9,
	            0x00, 0xC3
            };

            for (int i = 0; i < writetext_func.Length; i++)
            {
                *(byte*)(func_address + i) = writetext_func[i];
            }

            cache_done_veitc_native_file_writetext =
                vmethod.SetNativeFunctionAdderss(m09.mhandle, libcs.tointptr(func_address));

            ////////////////////////////////////////////////////////////////////////////////

            skip10:
            var m10 = (MonoMethod)vmethod.GetMethodNameFastS(type, "veitc_native_file_write");
            if (m10 == null)
            {
                libcs.assert("type.GetMethod(\"veitc_native_file_write\") failed");
                goto skip11;
            }

            tem = native09.Length + native09.Length
                - 0x32;

            var native09_address = (uint)native09.obj_address().value;
            func_address = native09_address + 0x14;

            for (int i = 0x10; i < tem; i++)
            {
                *(uint*)(native09_address + i) = 0xCCCCCCCC;
            }

            native09.obj_address(); // check SIGSEGV 

            byte[] write_func = {
	            0x31, 0xDB, 0x53, 0x53, 0xFF, 0x74, 0x24, 0x14, 0xFF, 0x74, 0x24, 0x14,
	            0xFF, 0x74, 0x24, 0x14, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90,
	            0x90, 0x90, 0x90, 0x90, 0x90, 0xFF, 0x15, 0x94, 0x63, 0xF9, 0x00, 0xC3
            };

            for (int i = 0; i < write_func.Length; i++)
            {
                *(byte*)(func_address + i) = write_func[i];
            }

            cache_done_veitc_native_file_write =
                vmethod.SetNativeFunctionAdderss(m10.mhandle, libcs.tointptr(func_address));

            ////////////////////////////////////////////////////////////////////////////////

            skip11:
            var m11 = (MonoMethod)vmethod.GetMethodNameFastS(type, "veitc_native_close_handle");
            if (m11 == null)
            {
                libcs.assert("type.GetMethod(\"veitc_native_close_handle\") failed");
                goto skip12;
            }

            tem = native10.Length + native10.Length
                - 0x32;

            var native10_address = (uint)native10.obj_address().value;
            func_address = native10_address + 0x14;

            for (int i = 0x10; i < tem; i++)
            {
                *(uint*)(native10_address + i) = 0xCCCCCCCC;
            }

            native10.obj_address(); // check SIGSEGV 

            byte[] fileclose_func = {
	            0xFF, 0x74, 0x24, 0x04, 0x90, 0xFF, 0x15, 0x9C, 0x63, 0xF9, 0x00, 0xC3
            };

            for (int i = 0; i < fileclose_func.Length; i++)
            {
                *(byte*)(func_address + i) = fileclose_func[i];
            }

            cache_done_veitc_native_close_handle =
                vmethod.SetNativeFunctionAdderss(m11.mhandle, libcs.tointptr(func_address));

            ////////////////////////////////////////////////////////////////////////////////

            skip12:
            var m12 = (MonoMethod)vmethod.GetMethodNameFastS(type, "veitc_native_obj_get_address");
            if (m12 == null)
            {
                libcs.assert("type.GetMethod(\"veitc_native_obj_get_address\") failed");
                throw new NotSupportedException();
            }

            tem = native11.Length + native11.Length
                - 0x32;

            var native11_address = (uint)native11.obj_address().value;
            func_address = native11_address + 0x14;

            for (int i = 0x10; i < tem; i++)
            {
                *(uint*)(native11_address + i) = 0xCCCCCCCC;
            }

            native11.obj_address(); // check SIGSEGV 

            byte[] get_address_func = {
	            0x8B, 0x44, 0x24, 0x04, 0xC3
            };

            for (int i = 0; i < get_address_func.Length; i++)
            {
                *(byte*)(func_address + i) = get_address_func[i];
            }

            cache_done_veitc_native_obj_get_address =
                vmethod.SetNativeFunctionAdderss(m12.mhandle, libcs.tointptr(func_address));

            if (m01 != null)
                OutputDebugString("veitc_native_load_library: func_address: 0x" + vmethod.GetNativeFunctionAdderss(m01.mhandle).ToString("X"));
            if (m02 != null)
                OutputDebugString("veitc_native_debug_text_to_debugger: func_address: 0x" + vmethod.GetNativeFunctionAdderss(m02.mhandle).ToString("X"));
            if (m03 != null)
                OutputDebugString("IsDebuggerAttached_internal: func_address: 0x" + vmethod.GetNativeFunctionAdderss(m03.mhandle).ToString("X"));
            if (m04 != null)
                OutputDebugString("DebuggerBreak: func_address: 0x" + vmethod.GetNativeFunctionAdderss(m04.mhandle).ToString("X"));
            if (m05 != null)
                OutputDebugString("veitc_native_ptr_fs_zero: func_address: 0x" + vmethod.GetNativeFunctionAdderss(m05.mhandle).ToString("X"));
            if (m06 != null)
                OutputDebugString("veitc_native_message_box: func_address: 0x" + vmethod.GetNativeFunctionAdderss(m06.mhandle).ToString("X"));
            if (m07 != null)
                OutputDebugString("veitc_native_cpuid: func_address: 0x" + vmethod.GetNativeFunctionAdderss(m07.mhandle).ToString("X"));
            if (m08 != null)
                OutputDebugString("veitc_native_file_create: func_address: 0x" + vmethod.GetNativeFunctionAdderss(m08.mhandle).ToString("X"));
            if (m09 != null)
                OutputDebugString("veitc_native_file_writetext: func_address: 0x" + vmethod.GetNativeFunctionAdderss(m09.mhandle).ToString("X"));
            if (m10 != null)
                OutputDebugString("veitc_native_file_write: func_address: 0x" + vmethod.GetNativeFunctionAdderss(m10.mhandle).ToString("X"));
            if (m11 != null)
                OutputDebugString("veitc_native_close_handle: func_address: 0x" + vmethod.GetNativeFunctionAdderss(m11.mhandle).ToString("X"));
            if (m12 != null)
                OutputDebugString("veitc_native_obj_get_address: func_address: 0x" + vmethod.GetNativeFunctionAdderss(m12.mhandle).ToString("X"));
            creaing = false;

            // Successful Exploit

#endif // SIMS3VERSION_1672
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool veitc_native_custom_test(); // DEBUG Only
      /*{
          Code for x86:
            mov al,1
            ret  
       
          C++ code:
            return true;
        }*/

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool veitc_native_custom_test_C(); // DEBUG Only
      /*{
          Code for x86:
            mov al,1
            ret  
       
          C++ code:
            return true;
        }*/

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern IntPtr veitc_native_load_library(string pathOrDllFileName);
      /*{
          Code for x86:
           mov ebx,[esp+4]
           add ebx,8
           push ebx
           call [0x00F96280] // LoadLibraryW
           ret 
       
          C++ code:
           return LoadLibraryW(pathOrDllFileName);
        }*/

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void veitc_native_debug_text_to_debugger(string str);
      /*{
          Code for x86:
            pushad 
            mov ecx,[esp+24]
            add ecx,8
            push ecx
            call [0x00F96314] // OutputDebugStringW
            popad 
            ret  
       
          C++ code:
            OutputDebugStringW(str);
            return;
        }*/

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern int IsDebuggerAttached_internal();
      /*{
          Code for x86:
            mov eax,dword ptr fs:[30]
            movzx eax,byte ptr ds:[eax+2]
            ret 
        }*/

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void DebuggerBreak();
      /*{
          Code for x86:
            int3
            ret 
        }*/

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern IntPtr veitc_native_ptr_fs_zero(voidptr offset);
      /*{
           Code for x86:
            mov ebx,[esp+4]
            mov eax,dword ptr fs:[ebx]
            xor ebx,ebx
            ret 
        }*/

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern mbresult veitc_native_message_box(voidptr hWnd, string text, string caption, mbtype uType);
      /*{
          Code for x86:
            mov ebx,[esp+8]
            add ebx,8
            mov [esp+8],ebx
            mov ebx,[esp+C]
            add ebx,8
            mov [esp+C],ebx
            sub esp,8
            mov ebx,[esp+8]
            mov [esp+4],ebx
            push [esp+18]
            push [esp+18]
            push [esp+18]
            push [esp+18]
            call [0x00F96800] // MessageBoxW
            add esp,8
            ret 
       
          C++ code:
            return MessageBoxW(hWnd, text, caption, uType);
        }*/

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void veitc_native_cpuid(ref voidptr eax, ref voidptr ebx, ref voidptr ecx, ref voidptr edx);
      /*{
          Code for x86:
            pushad 
            mov eax,[esp+24]
            mov ebx,[esp+28]
            mov ecx,[esp+2C]
            mov edx,[esp+30]
            mov eax,[eax]
            mov ebx,[ebx]
            mov ecx,[ecx]
            mov edx,[edx]
            cpuid 
            push ebp
            mov ebp,esp
            sub esp,10
            mov [esp],eax
            mov [esp+4],ebx
            mov [esp+8],ecx
            mov [esp+C],edx
            mov eax,[esp+38]
            mov ebx,[esp+3C]
            mov ecx,[esp+40]
            mov edx,[esp+44]
            mov edi,[esp]
            mov [eax],edi
            mov edi,[esp+4]
            mov [ebx],edi
            mov edi,[esp+8]
            mov [ecx],edi
            mov edi,[esp+C]
            mov [edx],edi
            add esp,10
            pop ebp
            popad 
            ret 
       
          C++ code:
            _cpuid_(&eax, &ebx, &ecx, &edx);
            return;
        }*/

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern IntPtr veitc_native_file_create(string path);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern object veitc_native_object_get_type(object obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void veitc_native_close_handle(IntPtr handle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int veitc_native_file_write(IntPtr handle, IntPtr data, int count);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int veitc_native_file_writetext(IntPtr handle, string text);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int veitc_native_force_gamecrash_noexhandle();
        /*{
          Code for x86:
          #if !intCode
            mov eax,esp
            mov esp,0x000300FF
            jmp esp
          #else
            int 0x29
          #endif
            ret 
        }*/

        public static int veitc_native_force_gamecrash_noexhandle_nonative()
        {
            /* Do Nothing */ libcs.unused(); return 0;
        }


        public unsafe static void veitc_native_force_gamecrash_noexhandle_init(bool eaCustomMono, bool intCode)
        {
            const string data = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌveitc_native_force_gamecrash_noexhandle";
            var dataCode = ((uint)data.obj_address().value + 0x8); 

            byte[] x86_code;
            if (intCode)
            {
                x86_code = new byte[] {
                    0xCD, 0x29,                     // int 0x29
                    0xC3                            // ret
                };
            }
            else
            {
                x86_code = new byte[] {
	                0x89, 0xE0,                     // mov eax,esp
                    0xBC, 0xFF, 0x00, 0x03, 0x00,   // mov esp,0x000300FF
                    0xFF, 0xE4,                     // jmp esp
                    0xC3                            // ret
                };
            }

            for (int i = 0; i < x86_code.Length; i++)
            {
                *(byte*)(dataCode + i) = x86_code[i];
            }

            if (eaCustomMono)
            {
                // TODO
                return;
            }

            var m01 = (MonoMethod)vmethod.GetGoodMethods(typeof(vnativefunctions), null, "veitc_native_force_gamecrash_noexhandle", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (m01 == null)
            {
                libcs.assert("GetMethod veitc_native_force_gamecrash_noexhandle failed");
                return;
            }
            if (vmethod.SetNativeFunctionAdderss(m01.mhandle, libcs.tointptr(dataCode)))
            {
                veitc_native_force_gamecrash_noexhandle();
                return;
            }
            libcs.assert("custom_native_function veitc_native_force_gamecrash_noexhandle failed");
        }

        public static bool IsDebuggerAttached()
        {
            if (cache_done_IsDebuggerAttached_internal && IsDebuggerAttached_internal() == 1)
                return true;
            return false;
        }

        public static bool booOutputDebugString = true;

        [System.Diagnostics.Conditional("OUTPUT_DEBUG_ENABLED")]
        public static void OutputDebugString(string str) { 
            if (!cache_done_veitc_native_debug_text_to_debugger)
                return;

            if (str == null)
                return;

            if (str.Length == 0)
                return;


            str = str.Replace((char)0x0D, (char)0x0A);
            veitc_native_debug_text_to_debugger(str);
        }

        [System.Diagnostics.Conditional("OUTPUT_DEBUG_ENABLED")]
        public static void SafeOutputDebugString(string str)
        {
            if (!cache_done_veitc_native_debug_text_to_debugger)
                return;

            if (str == null)
                str = "veitcstd: message is null";

            if (str.Length == 0)
                str = "veitcstd: no message";

            str = str.Replace((char)0x0D, (char)0x0A);

            veitc_native_debug_text_to_debugger(str);
        }

        public static IntPtr LoadDLLNativeLibrary(string pathOrDllFileName)
        {
           
            if (!cache_done_veitc_native_load_library)
                return default(IntPtr);

            if (pathOrDllFileName == null || pathOrDllFileName.Length == 0)
                throw new ArgumentNullException("pathOrDllFileName");

            return veitc_native_load_library(pathOrDllFileName);
        }

        public unsafe static bool RunExeUnProtectNativeLibrary()
        {
            // TODO add C++ library
            if (LoadDLLNativeLibrary("VeitcNative_ExeUnProtect.dll").value != null)
            {
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern int veitc_native_un_pro(voidptr k32, voidptr vpro);

        public unsafe static int RunExeUnProtect()
        {
            const string nativeunpro = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvunproM";
            const string textVirtualPro = "ÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌÌvtectNMNMNMNRunExeU";

            var virtualPro = ((uint)textVirtualPro.obj_address().value             + 0x8);
            var ke32       = ((uint)"kernel32.dll\0\0\0\0\0\0".obj_address().value + 0x8);

            // "VirtualProtect" UTF-8
            byte[] rawtext = {
	            0x56, 0x69, 0x72, 0x74, 0x75, 0x61,
                0x6C, 0x50, 0x72, 0x6F, 0x74, 0x65,
	            0x63, 0x74, 0x00, 0x00, 0x00, 0x00
            };

            for (int i = 0; i < rawtext.Length; i++)
            {
                *(byte*)(virtualPro + i) = rawtext[i];
            }

            var m01 = (MonoMethod)vmethod.GetMethodNameFastS(typeof(vnativefunctions)
                ,"veitc_native_un_pro");

            uint func_address = 0;
            int tem = 0;

            if (m01 == null)
            {
                return 0;
            }

            tem = nativeunpro.Length + nativeunpro.Length
                - 0x32;

            var nativeunpro_address = (uint)nativeunpro.obj_address().value;
            func_address = nativeunpro_address + 0x14;

            for (int i = 0x10; i < tem; i++)
            {
                *(uint*)(nativeunpro_address + i) = 0xCCCCCCCC;
            }

            /*
           code for x86:
            push ebp
            mov ebp,esp
            sub esp,18
            push 0
            call [GetModuleHandleW]
            mov [ebp-18],eax
            mov eax,[ebp-18]
            mov ecx,[ebp-18]
            add ecx,[eax+3C]
            mov [ebp-14],ecx
            mov edx,[ebp-14]
            mov eax,[edx+50]
            mov [ebp-10],eax
            mov ecx,[ebp+C]
            push ecx
            mov edx,[ebp+8]
            push edx
            call [GetModuleHandleW]
            push eax
            call [GetProcAddress]
            mov [ebp-8],eax
            mov eax,[ebp-8]
            mov [ebp-4],eax
            lea ecx,[ebp-C]
            push ecx
            push 40
            mov edx,[ebp-10]
            push edx
            mov eax,[ebp-18]
            push eax
            call [ebp-4] // call VirtualProtect
            add esp,10
            mov eax,[ebp-C]
            mov esp,ebp
            pop ebp
            ret 
            
            code for C++
            /*
            int veitc_native_un_pro(void* k32, void* vpro)
            {
            	auto hExecutableInstance = (size_t)GetModuleHandle(NULL);
            	IMAGE_NT_HEADERS* ntHeader = (IMAGE_NT_HEADERS*)(hExecutableInstance + ((IMAGE_DOS_HEADER*)hExecutableInstance)->e_lfanew);
            	SIZE_T size = ntHeader->OptionalHeader.SizeOfImage;
            	DWORD oldProtect;
            
            	void* funcptr = GetProcAddress((HMODULE)GetModuleHandleW((LPCWSTR)k32), (LPCSTR)vpro);
            
            	void* (*funcVirtualProtect)(void*, void*, void*, void*) = (void*(*)(void*, void*, void*, void*))funcptr;
            
            	funcVirtualProtect((void*)hExecutableInstance, (void*)size, (void*)0x40, &oldProtect);
            	
            	return oldProtect;
            }
             */

            byte[] x86_code = {
	          0x55, 0x8B, 0xEC, 0x83, 0xEC, 0x18, 0x6A, 0x00, 0xFF, 0x15, 0x74, 0x62,
	          0xF9, 0x00, 0x89, 0x45, 0xE8, 0x8B, 0x45, 0xE8, 0x8B, 0x4D, 0xE8, 0x03,
	          0x48, 0x3C, 0x89, 0x4D, 0xEC, 0x8B, 0x55, 0xEC, 0x8B, 0x42, 0x50, 0x89,
	          0x45, 0xF0, 0x8B, 0x4D, 0x0C, 0x51, 0x8B, 0x55, 0x08, 0x52, 0xFF, 0x15,
	          0x74, 0x62, 0xF9, 0x00, 0x50, 0xFF, 0x15, 0x04, 0x61, 0xF9, 0x00, 0x89,
	          0x45, 0xF8, 0x8B, 0x45, 0xF8, 0x89, 0x45, 0xFC, 0x8D, 0x4D, 0xF4, 0x51,
	          0x6A, 0x40, 0x8B, 0x55, 0xF0, 0x52, 0x8B, 0x45, 0xE8, 0x50, 0xFF, 0x55,
	          0xFC, 0x83, 0xC4, 0x10, 0x8B, 0x45, 0xF4, 0x8B, 0xE5, 0x5D, 0xC3
            };

            for (int i = 0; i < x86_code.Length; i++)
            {
                *(byte*)(func_address + i) = x86_code[i];
            }

            if (!vmethod.SetNativeFunctionAdderss(m01.mhandle, libcs.tointptr(func_address)))
                return 0;

            return veitc_native_un_pro(ke32, virtualPro);
        }

        public static mbresult MessageBox(uint hWnd, string text, string caption, mbtype uType)
        {
            if (!cache_done_veitc_native_message_box)
                return (mbresult)0;
#if VETIC_USE_WIN32
            if (text == null)
                throw new ArgumentNullException("text");
            if (caption == null)
                throw new ArgumentNullException("caption");

            return veitc_native_message_box(hWnd, text, caption, uType);
#else
            return (mbresult)0;
#endif
        }

        public unsafe static int WriteTextToFileA(IntPtr handle, string text) // UTF-8 or ANSI
        {
            if (!cache_done_veitc_native_file_write || text == null || !libcs.is_valid_handle(handle))
                return 0;
            if (text.Length == 0)
                return 1;

            var tttl = text.Length;
            var ttta = Marshal.StringToHGlobalAnsi(text);

            return veitc_native_file_write(handle, ttta, tttl);
            //Marshal.FreeHGlobal(ttta); // random game crashed
        }

        public unsafe static int WriteTextToFileW(IntPtr handle, string text) // UTF-16
        {
            if (!cache_done_veitc_native_file_writetext || text == null || !libcs.is_valid_handle(handle))
                return 0;
            if (text.Length == 0)
                return 1;
            return veitc_native_file_writetext(handle, text);
        }

        public unsafe static int WriteFile(IntPtr handle, IntPtr data, int count)
        {
            if (!cache_done_veitc_native_file_write || data.value == null || !libcs.is_valid_handle(handle))
                return 0;
            if (count == 0)
                return 1;
            return veitc_native_file_write(handle, data, count);
        }

        public unsafe static void CloseHandle(IntPtr handle)
        {
            if (libcs.is_valid_handle(handle))
            {
                if (!cache_done_veitc_native_close_handle)
                    return;
                veitc_native_close_handle(handle);
            }
        }

        [Obsolete("TODO")]
        public unsafe static void* NonMonoObjectAsPointer(object obj)
        {
            //if (obj == null)
                return (void*)0;
            //return (void*)veitc_native_get_classobject_for_nonptr<IntPtr>(obj).value;
        }

        public unsafe static void* MonoObjectAsPointer(object obj)
        {
            if (obj == null)
                return (void*)0;
            return (void*)obj.obj_address().value;
        }

        public unsafe static object[] GetNewPtrAllocStr(int len)
        {
            if (len < 0)
                throw new OutOfMemoryException("len < 0");

            string eee = string.InternalAllocateStr(len);
            uint tt = (uint)(eee.obj_address().value) + (uint)RuntimeHelpers.OffsetToStringData;
            IntPtr yyy = libcs.tointptr(tt);
            int le = eee.Length * 2;

            return new object[] { eee, yyy, le };
        }

        public unsafe static bool StrIsFirstZeroCharMax(string s, size_t max)
        {
            if (s == null)
                return false;
            fixed (char* ptr = s)
            {
                for (size_t i = 0; i < s.Length && i < max; i++)
                {
                    if (*(short*)(ptr + i) == 0)
                        return true;
                }
            }
            return false;
        }

        public unsafe static IntPtr StringToCoTaskMemAnsi(string s)
        {
            int len2 = s.Length;
            int len = len2 + 1;

            IntPtr intPtr = Marshal.AllocCoTaskMem(len);
            uint t = (uint)intPtr.value;

            fixed (char* ptr = s)
            {
                for (int i = 0; i < len2; i++)
                {
                    *(short*)(t + i) = *(short*)(ptr + i);
                }
            }
#if DEBUG
            OutputDebugString("StringToCoTaskMemAnsi: 0x" + t.ToString("X"));
#endif
            return intPtr;
        }

        private static char[] InvalidFileNameChars = null;

        public static bool CheckInvalidPathChars(string path)
        {
            if (path == null || path.Length == 0 || InvalidFileNameChars == null)
            {
                return false;
            }
            if (path.IndexOfAny(InvalidFileNameChars) > 0)
            {
                return false;
            }
            return true;
        }

        public unsafe static object[] StringToScriptMemAnsi(string s)
        {
            int slength = s.Length;
            if (slength == 0)
                return null;
            int len = slength + 1;

            object[] objlist = GetNewPtrAllocStr(len);
            IntPtr intPtr = (IntPtr)objlist[1];

            uint t = (uint)intPtr.value;

            fixed (char* ptr = s)
            {
                for (int i = 0; i < slength; i++)
                {
                    *(short*)(t + i) = *(short*)(ptr + i);
                }
            }
#if DEBUG
            OutputDebugString("StringToScriptMemAnsi: 0x" + t.ToString("X"));
#endif
            return objlist;
        }

        public unsafe static uint MonoObjectAsPointerU(object obj) //void* ptr = (void*)obj; don't compile
        {
            if (obj == null)
                return 0;
            return (uint)obj.obj_address().value;
        }

        public unsafe static ulong MonoObjectAsPointerU64(object obj) //void* ptr = (void*)obj; don't compile
        {
            if (obj == null)
                return 0;
            return (ulong)obj.obj_address().value;
        }

        // You need create a directory path!
        // if directory not exists return 0xFFFFFFFF
        public unsafe static IntPtr CreateFile(string path)
        {
            if (!cache_done_veitc_native_file_create || path == null || path.Length == 0)
                return libcs.tointptr(0xFFFFFFFF);
            return veitc_native_file_create(path);
        }




        /* TODO: Add Native Function

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void veitc_native_rdtsc(out voidptr eax, out voidptr edx);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern IntPtr veitc_native_call_function(IntPtr funcAddress);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void veitc_native_exit_game(voidptr exitcode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern IntPtr veitc_native_obj_get_address(object obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern uint veitc_native_obj_get_address_nonptr(object obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern ulong veitc_native_obj_get_address_nonptr_64bit(object obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern object veitc_native_get_object_for_ptr(IntPtr obj_address);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern object veitc_native_get_object_for_nonptr(uint obj_address);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern object veitc_native_get_object_for_nonptr_64bit(ulong obj_address);


        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern T veitc_native_get_classobject_for_ptr<T>(IntPtr obj_address);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern T veitc_native_get_classobject_for_nonptr<T>(object obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int veitc_native_check_yielding_context();

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern IntPtr veitc_native_create_thread(IntPtr _StartAddress, IntPtr _ArgList, uint _StackSize);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void veitc_native_destroy_thread(IntPtr threadHandle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool veitc_native_file_found(string path);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool veitc_native_file_delete(string path);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool veitc_native_file_rename(string path, string fileName);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool veitc_native_file_copy(string filePath, string toFilePath);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool veitc_native_file_read(string path, IntPtr outData, int startIndex, int maxSize);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern string veitc_native_file_readtext(string path);
        */
    }
#endif
}
