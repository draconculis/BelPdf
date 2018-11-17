using Dek.Bel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DekManagedLib
{
    public class ManagedClass
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ResultData BelDelegate(EventData data);

        private BelDelegate _delegate;

        public ManagedClass()
        {
            _delegate = new BelDelegate(this.DoStuff);
        }

        public BelDelegate GetDelegate()
        {
            return _delegate;
        }

        public ResultData DoStuff(EventData data)
        {
            //Debug.WriteLine("Make a day!!!!!!!!!!!!!!!!!!!!");
            //Debug.WriteLine(data.I);
            //Debug.WriteLine(data.Text);

            BelGui bel = new BelGui(data);
            bel.ShowDialog();

            return bel.Result;
        }

    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct EventData

    {
        public int Code;
        public string Text;
        public string FilePath;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct ResultData

    {
        public int Code;
        bool Cancel;
        public string Message;
    }
}
