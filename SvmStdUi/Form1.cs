using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SvmStdLib;
namespace SvmStdUi
{
    public partial class Form1 : Form
    {
        ValueGen vals;
        public Form1()
        {
            InitializeComponent();
            vals = new();
            vals.Updated += Vals_Updated;
        }
        private void Vals_Updated(object sender, string e)
        {
            ValueGen vg = (ValueGen)sender;
            this.UIThread(() => this.textBox1.Text = vg.val1.ToString());
            this.UIThread(() => this.textBox2.Text = vg.val2.ToString());
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            vals.RunState = ValueGen.State.Stopping;

            while (vals.RunState == ValueGen.State.Stopping)
            {
                Thread.Sleep(100);
            }
        }
    }
    public static class ControlExtensions
    {
        public static void UIThread(this Control @this, Action code)
        {
            if (@this.InvokeRequired)
            {
                @this.BeginInvoke(code);
            }
            else
            {
                code.Invoke();
            }
        }
    }
    public class ValueGen
    {
        public int val1 { get; set; }
        public int val2 { get; set; }
        public State RunState { get; set; }
        private Thread t;
        public event EventHandler<string> Updated;
        public ValueGen()
        {
            t = new Thread(Gen);
            t.Start();
        }
        private void Gen()
        {
            while (RunState == State.Running)
            {
                val1 += 1;
                val2 += 2;
                Updated?.Invoke(this, "Updated");
                Thread.Sleep(50);
            }
            RunState = State.Stopped;
        }
        public enum State
        {
            Running,
            Stopping,
            Stopped
        }
    }
}
