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

#if VETIC_HAVE_SIMIFACE
using Sims3.SimIFace;

namespace veitcstd
{
    public static class vvec3
    {
        public static
              bool isnanorzero(Vector3 v)
        {
            float x, y, z;
            x = v.x;
            y = v.y;
            z = v.z;
            return (float.IsNaN(x) || float.IsNaN(y) || float.IsNaN(z)) || (x == 0 && y == 0 && z == 0);
        }
    }
}
#endif
