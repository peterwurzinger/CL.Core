using System;

namespace CL.Core.API
{
    internal delegate void ContextErrorDelegate(string error, IntPtr privateInfo, int cb, IntPtr userData);
}
