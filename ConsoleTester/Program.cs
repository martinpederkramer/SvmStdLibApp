using System;
using System.Threading;
using SvmStdLib;
using System.Xml;
using System.Collections.Generic;

namespace ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine($"Machine: {Environment.MachineName}");
        }
    }
    public delegate void PlcTagUpdated(PlcTag tag);
    public enum PlcTagCommand
    {
        Read,
        Write,
        Dispose
    }
    public class PlcTag
    {
        public string SymbolName;
        public byte[] ByteValue;
        public string TextValue { get; set; }
        public PlcTagCommand Command;
        public PlcTagUpdated TagUpdated;
    }
    public class PlcAds
    {
        List<PlcAdsTag> Tags= new List<PlcAdsTag>();

        public void Cyclic()
        {

        }
        public void AddTag(PlcTag tag)
        {
            //Assign handle
            //Add
        }
    }
    public class PlcAdsTag
    {
        public int PlcTag { get; set; }
        public int Handle;
    }
}
