using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SvmStdLib.PlcAccess
{
    public class PlcAds : IDisposable
    {
        public PlcState State = PlcState.Init;
        List<PlcAdsTag> Tags = new List<PlcAdsTag>();

        Thread t1;

        public PlcAds()
        {
            t1 = new Thread(Cyclic);
            t1.Start();
        }

        public void Cyclic()
        {
            while (State != PlcState.Dispposed)
            {
                Thread.Sleep(10);
                switch (State)
                {
                    case PlcState.Init:
                        //Connect
                        break;
                    case PlcState.Idle:
                        ScanTags();
                        break;
                    case PlcState.Error:
                        break;
                    case PlcState.Disposing:
                        ScanTags();
                        if (Tags.Count == 0)
                        {
                            //Disconnect
                            State = PlcState.Dispposed;
                        }
                        break;
                    case PlcState.Dispposed:
                        break;
                }
            }
        }
        private void ScanTags()
        {
            foreach (PlcAdsTag tag in Tags)
            {
                switch (tag.Command)
                {
                    case PlcTagCommand.Idle:
                        break;
                    case PlcTagCommand.Read:
                        if (tag.State == PlcTagState.Idle)
                        {
                            tag.State = PlcTagState.Reading;
                        }
                        break;
                    case PlcTagCommand.Write:
                        if (tag.State == PlcTagState.Idle)
                        {
                            tag.State = PlcTagState.Writing;
                        }
                        break;
                    case PlcTagCommand.Dispose:
                        switch (tag.State)
                        {
                            case PlcTagState.Init:
                                tag.State = PlcTagState.Disposed;
                                break;
                            case PlcTagState.Idle:
                                tag.State = PlcTagState.Disposing;
                                break;
                            case PlcTagState.Error:
                                tag.State = PlcTagState.Disposing;
                                break;
                            default:
                                tag.State = PlcTagState.Disposing;
                                break;
                        }
                        break;
                }
                switch (tag.State)
                {
                    case PlcTagState.Init:
                        break;
                    case PlcTagState.Idle:
                        break;
                    case PlcTagState.Reading:
                        break;
                    case PlcTagState.Writing:
                        break;
                    case PlcTagState.Error:
                        break;
                    case PlcTagState.Disposing:
                        break;
                    case PlcTagState.Disposed:
                        break;
                    default:
                        break;
                }
            }
        }
        public void AddTag(PlcTag tag)
        {
            //Lock Disposes
            //Add
        }
        public void DisposeTag(PlcTag tag)
        {
            //Lock Disposes
            //Add
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
    public class PlcTag
    {
        public string SymbolName;
        public byte[] ByteValue;
        public string TextValue { get; set; }
        public PlcTagCommand Command;
        public PlcTagState State;
        public PlcTagUpdated TagUpdated;
    }
    public class PlcAdsTag : PlcTag
    {
        public int PlcTag { get; set; }
        public int Handle;
    }
}
