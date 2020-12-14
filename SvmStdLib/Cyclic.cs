using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SvmStdLib
{
    public delegate void CyclicDel();
    public enum StateEnum
    {
        Idle,
        Running,
        Stopping,
        Stopped,
        Aborted
    }
    public enum CommandEnum
    {
        Start,
        Stop,
        Abort
    }
    public class Cyclic
    {
        private StateEnum state;
        public StateEnum State
        {
            get { return state; }
            set { state = value; }
        }
        private CommandEnum commands;
        public CommandEnum Command
        {
            get { return commands; }
            set { commands = value; }
        }
        public Cyclic(CyclicDel cyclic)
        {
            cyclicDel = cyclic;
            t = new Thread(mainSweep);
            t.Start();
        }
        private CyclicDel cyclicDel;
        private Thread t;
        private void mainSweep()
        {
            cyclicDel();
            Thread.Sleep(1);
        }
    }
}
