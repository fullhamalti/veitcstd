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
    // TODO: add support Directory class
#if SIMS3VERSION_1672
    public static class vio
    {
        public class FILE
        {
            // TODO: add support
            public ulong len;
            public IntPtr data;
            //

            public string path;
            public IntPtr handle;

            public void Close()
            {
                if (IsClosed())
                    return;

                vnativefunctions.CloseHandle(handle);
                handle = libcs.tointptr(0xFFFFFFFF);
            }

            public bool IsClosed()
            {
                return !libcs.is_valid_handle(handle);
            }

            public bool WriteStr(string text)
            {
                if (IsClosed())
                    throw new ObjectDisposedException("FILE");
                return vnativefunctions.WriteTextToFileW(handle, text) == 1;
            }

            public bool WritePtr(IntPtr data, size_t count)
            {
                if (IsClosed())
                    throw new ObjectDisposedException("FILE");
                return vnativefunctions.WriteFile(handle, data, (ivoidptr)count) == 1;
            }

            // TODO: add support Read, Seak, LastWriteTime, Length, Flush
        }
        public static vio.FILE CreateFile(string path)
        {
            if (path == null)
                return null;

            var fileHandle = vnativefunctions.CreateFile(path);
            if (!libcs.is_valid_handle(fileHandle))
                return null;
            var file = new FILE();
            file.data = default(IntPtr);
            file.handle = fileHandle;
            file.len = ulong.MaxValue;
            file.path = path;
            return file;
        }
        // TODO: add support DeleteFile, Move, CopyTo, Exists, GetFiles
    }
#endif
}
