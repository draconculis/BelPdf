using Dek.Bel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dek.Bel.Services;
using Dek.Bel.DB;
using Dek.Bel.InterOp;
using Dek.Bel.Models;

namespace BelManagedLib
{
    public class BelManagedClass
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ResultData BelDelegate(EventData data);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ResultFileStorageData BelRequestFileStoragePathDelegate(RequestFileStorageData data);

        private BelDelegate _delegate;
        private BelRequestFileStoragePathDelegate _fileStoragedelegate;

        public ResultData Result { get; set; }
            = new ResultData
            {
                Message = "Result stub",
                Code = 0,
                Cancel = false,
            };



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
            ReferenceService refsvc;
            string res;
            switch ((CodesEnum)data.Code)
            {
                case CodesEnum.DEKBELCODE_ADDVOLUMETITLE:
                    refsvc = new ReferenceService();
                    res = refsvc.EditVolumeTitle(data);
                    Result = new ResultData
                    {
                        Code = 0,
                        Message = $"New title set: {res}.",
                        Cancel = string.IsNullOrWhiteSpace(res),
                    };
                    break;
                case CodesEnum.DEKBELCODE_ADDBOOKTITLE:
                    refsvc = new ReferenceService();
                    res = refsvc.AddReference<Book>(data);
                    Result = new ResultData
                    {
                        Code = 0,
                        Message = $"Added book: {res}.",
                        Cancel = string.IsNullOrWhiteSpace(res),
                    };
                    break;
                case CodesEnum.DEKBELCODE_ADDCHAPTER:
                    refsvc = new ReferenceService();
                    res = refsvc.AddReference<Chapter>(data);
                    Result = new ResultData
                    {
                        Code = 0,
                        Message = $"Added chapter: {res}.",
                        Cancel = string.IsNullOrWhiteSpace(res),
                    };
                    break;
                case CodesEnum.DEKBELCODE_ADDSUBCHAPTER:
                    refsvc = new ReferenceService();
                    res = refsvc.AddReference<SubChapter>(data);
                    Result = new ResultData
                    {
                        Code = 0,
                        Message = $"Added subchapter: {res}.",
                        Cancel = string.IsNullOrWhiteSpace(res),
                    };
                    break;
                case CodesEnum.DEKBELCODE_ADDPARAGRAPH:
                    refsvc = new ReferenceService();
                    res = refsvc.AddReference<Paragraph>(data);
                    Result = new ResultData
                    {
                        Code = 0,
                        Message = $"Added paragraph: {res}.",
                        Cancel = string.IsNullOrWhiteSpace(res),
                    };
                    break;
                case CodesEnum.DEKBELCODE_ADDRAWCITATION:
                    var mainService = new MainService();
                    var result = mainService.AddRawCitations(data);
                    Result = new ResultData
                    {
                        Code = 0,
                        Message = $"Added raw citation. Id = {result.Id}.",
                        Cancel = false,
                    };
                    break;
                case CodesEnum.DEKBELCODE_SHOWBEL:
                case CodesEnum.DEKBELCODE_ADDANDSHOWCITATION:
                    BelGui belAdd = new BelGui(data);
                    if (belAdd.IsDisposed)
                    {
                        Result = new ResultData
                        {
                            Cancel = true,
                            Code = -1,
                            Message = "Cancel"
                        };
                        break;
                    }
                    belAdd.ShowDialog();
                    Result = belAdd.Result;
                    break;
                case CodesEnum.DEKBELCODE_ADDCITATIONSILENT:
                    var mainService2 = new MainService();
                    bool result2 = mainService2.AddCitationSilent(data);
                    if (!result2)
                    {
                        Result = new ResultData
                        {
                            Cancel = true,
                            Code = -1,
                            Message = "Cancel"
                        };
                        break;
                    }
                    break;
                case CodesEnum.DEKBELCODE_EDITCITATION:
                    BelGui belEdit = new BelGui(data);
                    if (belEdit.IsDisposed)
                    {
                        Result = new ResultData
                        {
                            Cancel = true,
                            Code = -1,
                            Message = "Cancel"
                        };
                        break;
                    }
                    belEdit.ShowDialog();
                    Result = belEdit.Result;
                    break;
                case CodesEnum.DEKBELCODE_STARTAUTOPAGINATION:
                    refsvc = new ReferenceService();
                    var page = refsvc.AddPage(data);
                    Result = new ResultData
                    {
                        Code = 0,
                        Message = page == null ? "Page add canceled." : $"Added page reference. Id = {page.Id}.",
                        Cancel = page == null,
                    };
                    refsvc.Dispose();
                    break;
                default:
                    break;
            }

            return Result;
        }

        // Copy to storage and add db entry
        public ResultFileStorageData RequestFileStoragePath(RequestFileStorageData data)
        {
            StorageService svc = new StorageService();
            return svc.GetStorage(data);
        }

    }
}
