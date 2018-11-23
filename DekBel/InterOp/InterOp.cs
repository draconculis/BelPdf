using Dek.Bel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BelManagedLib
{
    public class BelManagedClass
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate ResultData BelDelegate(EventData data);
        private BelDelegate _delegate;

        public BelManagedClass()
        {
            _delegate = new BelDelegate(DoStuff);
        }

        public BelDelegate GetBelDelegate()
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
}
