using Dek.Bel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dek.Bel.Storage;
using Dek.Bel.DB;
using Dek.Bel.InterOp;

namespace BelManagedLib
{
    public class BelManagedClass
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate ResultData BelDelegate(EventData data);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
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
            #define DEKBELCODE_ADDVOLUMETITLE          9100
            #define DEKBELCODE_ADDBOOKTITLE            9110
            #define DEKBELCODE_ADDCHAPTER              9120
            #define DEKBELCODE_ADDCITATION             9200
            #define DEKBELCODE_ADDANDSHOWCITATION      9300
            #define DEKBELCODE_STARTAUTOPAGINATION     9400
            */
            ResultData res = null;

            switch ((CodesEnum)data.Code)
            {
                case CodesEnum.DEKBELCODE_ADDVOLUMETITLE:
                    break;
                case CodesEnum.DEKBELCODE_ADDBOOKTITLE:
                    break;
                case CodesEnum.DEKBELCODE_ADDCHAPTER:
                    break;
                case CodesEnum.DEKBELCODE_ADDCITATION:
                    var citationRepo = new CitationRepo();
                    var result = citationRepo.AddRawCitations(data);
                    res = new ResultData
                    {
                        Code = 0,
                        Message = $"Added raw citation. Id = {result.Id}.",
                        Cancel = false,
                    };
                    break;
                case CodesEnum.DEKBELCODE_ADDANDSHOWCITATION:
                    BelGui bel = new BelGui(data);
                    bel.ShowDialog();
                    res = bel.Result;
                    break;
                case CodesEnum.DEKBELCODE_STARTAUTOPAGINATION:
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
            return svc.GetStorage(data);
        }

    }
}
