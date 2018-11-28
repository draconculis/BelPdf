using Dek.Bel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dek.Bel.Storage;

namespace BelManagedLib
{
    public class BelManagedClass
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate ResultData BelDelegate(EventData data);
        public delegate ResultFileStorageData BelRequestFileStoragePathDelegate(RequestFileStorageData data);
        private BelDelegate _delegate;
        private BelRequestFileStoragePathDelegate _fileStoragedelegate;


        public BelManagedClass()
        {
            _delegate = new BelDelegate(DoStuff);
            _fileStoragedelegate = new BelRequestFileStoragePathDelegate(RequestFileStoragePath);
        }

        public BelDelegate GetBelDelegate()
        {
            return _delegate;
        }

        public BelRequestFileStoragePathDelegate GetFileStorageDelegate()
        {
            return _fileStoragedelegate;
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

        // Copy to storage and add db entry
        public ResultFileStorageData RequestFileStoragePath(RequestFileStorageData data)
        {
            StorageService svc = new StorageService();
            return svc.InitiateStorageForFile(data);
        }

    }
}
