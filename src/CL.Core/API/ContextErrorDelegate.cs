using System;

namespace CL.Core.API
{
    public delegate void ContextErrorDelegate(string error, IntPtr privateInfo, int cb, IntPtr userData);
}
