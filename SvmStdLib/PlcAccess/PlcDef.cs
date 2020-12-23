using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvmStdLib.PlcAccess
{
    public delegate void PlcTagUpdated(PlcTag tag);
    public enum PlcTagCommand
    {
        Idle,
        Read,
        Write,
        Dispose
    }
    public enum PlcTagState
    {
        Init,
        Idle,
        Reading,
        Writing,
        Error,
        Disposing,
        Disposed
    }
    public enum PlcState
    {
        Init,
        Idle,
        Error,
        Disposing,
        Dispposed
    }
}
