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
using System.Collections.Generic;
using System.Diagnostics;

namespace veitcstd
{
    public class vstacktrace {
    
        private static StackFrame sftemp = null;

        public StackFrame[] Frames = null;

        public bool DebugInfo = false;

        public int FrameCountFast = 0;

        public int FrameCount
        {
            get
            {
                if (Frames != null)
                {
                    return Frames.Length;
                }
                return 0;
            }
        }

        public StackFrame GetFrame(int index)
        {
            if (Frames == null || index < 0 || index >= FrameCount)
            {
                return null;
            }
            return Frames[index];
        }

        public StackFrame GetFrameFast(int index)
        {
            if (Frames == null || index < 0 || index >= FrameCountFast)
            {
                return null;
            }
            return Frames[index];
        }

        public string GetNameMethod(int index) {
            var frame = GetFrameFast(index);
            if (frame == null) 
                return null;
            var methodInfo = frame.GetMethod();
            if (methodInfo == null)
                return null;
            return methodInfo.DeclaringType.FullName + "." + methodInfo.Name + "()";
        }

        public vstacktrace() { }

        public vstacktrace(bool needFileInfo)
        {
            needFileInfo = false; // EAMono does not support StackFrame needFileInfo
            InitFrames(0, needFileInfo);
        }

        public vstacktrace(int skipFrames)
        {
            InitFrames(skipFrames, false);
        }

        public vstacktrace(int skipFrames, bool needFileInfo)
        {
            needFileInfo = false;
            InitFrames(skipFrames, needFileInfo);
        }
        // unprotected mono mscorlib 
        public static void InitFrame(StackFrame frame, int skipFrames, bool needFileInfo) // fast code.  EAMono Interpreter only
        {
            if (frame == null)
                throw new NullReferenceException();
            StackFrame.get_frame_info(skipFrames + 2, needFileInfo, out frame.methodBase, out frame.ilOffset, out frame.nativeOffset, out frame.fileName, out frame.lineNumber, out frame.columnNumber);
        }
        public static void InitFrameLast(StackFrame frame, int skipFrames, bool needFileInfo) // fast code.  EAMono Interpreter only
        {
            if (frame == null)
                throw new NullReferenceException();
            StackFrame.get_frame_info(skipFrames, needFileInfo, out frame.methodBase, out frame.ilOffset, out frame.nativeOffset, out frame.fileName, out frame.lineNumber, out frame.columnNumber);
        }
        private static StackFrame s_stack_frame_ = null;
        public static bool FastIsCallStackMethed(System.Reflection.MethodBase methed, bool last) // fast code.  EAMono Interpreter only
        {
            if (methed == null)
                throw new ArgumentNullException();
            if (s_stack_frame_ == null)
            {
                s_stack_frame_ = new StackFrame();
            }
            if (last)
            {
                for (int stack = 64 - 1; stack >= 0; stack--)
                {
                    InitFrameLast(s_stack_frame_, stack, false);
                   // var mb = s_stack_frame_.methodBase;
                    if (s_stack_frame_.methodBase == methed)
                        return true;
                }
            }
            else
            {
                for (int stack = 1; stack < 64; stack++)
                {
                    // var stackFrame = new StackFrame(i, false);
                    InitFrame(s_stack_frame_, stack, false);
                    var mb = s_stack_frame_.methodBase;
                    if (mb == null)
                        break;
                    if (mb == methed)
                        return true;

                }
            }
            return false;
        }
        public static bool IsCallingMyMethedLite(string text, bool co, int skip)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            StackFrame sf = sftemp;//new StackFrame(skip);
            if (sf == null)
                sf = sftemp = new StackFrame(skip);
            else
            {
                StackFrame.get_frame_info (
                    skip + 1,
                    false,
                    out sf.methodBase,
                    out sf.ilOffset, 
                    out sf.nativeOffset,
                    out sf.fileName,
                    out sf.lineNumber,
                    out sf.columnNumber
                );
            }

            var _method = sf.GetMethod();
            if (_method == null)
                return false;

            string temp = _method.DeclaringType.FullName + "::" + _method.Name;

            if ((co && temp.Contains(text)) || temp == text)
                return true;

            return false;
        }

        public static System.Reflection.MethodBase GetSkipMethed(int skip)
        {
            var sf = sftemp;
            if (sf == null)
            {
                sftemp = new StackFrame(skip);
                return sftemp.GetMethod();
            }
            StackFrame.get_frame_info (
                skip + 1,
                false,
                out sf.methodBase,
                out sf.ilOffset, 
                out sf.nativeOffset,
                out sf.fileName,
                out sf.lineNumber,
                out sf.columnNumber
            );
            return sf.GetMethod(); // unport mscorlib
        }

        public void InitFrames(int skipFrames, bool needFileInfo)
        {
            if (skipFrames < 0)
                throw new ArgumentOutOfRangeException("skipFrames", "Can't be less than 0.");

            if (skipFrames > 63)
                throw new ArgumentOutOfRangeException("skipFrames", "skipFrames >= 64");

            //List<StackFrame> arrayList = new List<StackFrame>(63);
            var arrayList = new StackFrame[64];

            skipFrames += 2;
            StackFrame stackFrame;
            int i;

            for (i = 0; i < arrayList.Length; i++)
            {
                stackFrame = new StackFrame(skipFrames, needFileInfo);
                if (stackFrame.GetMethod() == null) 
                    break;

                //arrayList.Add(stackFrame);
                arrayList[i] = stackFrame;

                skipFrames++;
            }

            if (i != 0)
                FrameCountFast = i + 1;

            

            DebugInfo = needFileInfo;

            //frames = arrayList.ToArray();
            Frames = arrayList;
            //libcs.assert(FrameCountFast == FrameCount, "FrameCountFast == FrameCount failed.\nFrameCountFast: " + FrameCountFast + "FrameCount: " + FrameCount);
        }
    }
}
