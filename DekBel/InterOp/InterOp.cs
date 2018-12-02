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
            /*
            #define DEKBELCODE_ADDVOLUMETITLE          100
            #define DEKBELCODE_ADDBOOKTITLE            110
            #define DEKBELCODE_ADDCHAPTER              120
            #define DEKBELCODE_ADDCITATION             200
            #define DEKBELCODE_ADDANDSHOWCITATION      300
            #define DEKBELCODE_STARTAUTOPAGINATION     400
            */
            ResultData res = null;

            switch (data.Code)
            {
                case 100: // DEKBELCODE_ADDVOLUMETITLE
                    break;
                case 110: // DEKBELCODE_ADDBOOKTITLE
                    break;
                case 120: // DEKBELCODE_ADDCHAPTER
                    break;
                case 200: // DEKBELCODE_ADDCITATION
                    break;
                case 300: // DEKBELCODE_ADDANDSHOWCITATION
                    BelGui bel = new BelGui(data);
                    bel.ShowDialog();
                    res = bel.Result;
                    break;
                case 400: // DEKBELCODE_STARTAUTOPAGINATION
                    break;
                default:
                    break;
            }

            return res;
        }

        // Copy to storage and add db entry
        public ResultFileStorageData RequestFileStoragePath(RequestFileStorageData data)
        {
            StorageService svc = new StorageService();
            return svc.InitiateNewStorageForFile(data);
        }

    }
}
