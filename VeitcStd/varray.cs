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

namespace veitcstd
{
    public static class varray<T>
    {
        public static void arraycpy(Array source_array, int source_index, Array destination_array, int destination_index, int length) // fast code.  EAMono Interpreter only
        {
            if (source_array == null)
            {
                throw new ArgumentNullException("source_array == null");
            }
            if (destination_array == null)
            {
                throw new ArgumentNullException("destination_array == null");
            }
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length has to be >= 0.");
            }
            if (source_index < 0)
            {
                throw new ArgumentOutOfRangeException("source_index has to be >= 0.");
            }
            if (destination_index < 0)
            {
                throw new ArgumentOutOfRangeException("destination_index has to be >= 0.");
            }
            if (Array.FastCopy(source_array, source_index, destination_array, destination_index, length))
            {
                return; // ok
            }
            throw new ArgumentException("FastCopy() failed.");
        }
        public static int arrayindexof(T[] array, T item) // fast code.  EAMono Interpreter only
        {
            if (array == null)
            {
                throw new ArgumentNullException("array == null");
            }
            for (int i = 0, max_l = array.Length; i < max_l; i++)
            {
                //if (object.ReferenceEquals(array[i], item)) 
                if ((object)array[i] == (object)item)
                    return i;
            }
            return -1;
        }
        public static int arrayindexof(T[] array, T item, int length) // fast code.  EAMono Interpreter only
        {
            if (array == null)
            {
                throw new ArgumentNullException("array == null");
            }
            if (length < 0) { throw new ArgumentOutOfRangeException("length has to be >= 0."); }
            for (int i = 0; i < length; i++)
            {
                //if (object.ReferenceEquals(array[i], item)) 
                if ((object)array[i] == (object)item)
                    return i;
            }
            return -1;
        }

        public static readonly T[] EmptyArray = new T[0]; 
    }
}
