using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BelManagedLib
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct EventData
    {
        public int Code { get; set; }
        public int CurrentPage { get; set; }
        public int StartPage { get; set; }
        public int StopPage { get; set; }
        public int StartGlyph { get; set; }
        public int StopGlyph { get; set; }
        public string Text { get; set; }
        public string FilePath { get; set; }
        public IntPtr SelectionRects { get; set; }
        public int Len { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class ResultData
    {
        public int Code { get; set; }
        public bool Cancel { get; set; }
        public string Message { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct RequestFileStorageData
    {
        public string FilePath { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class ResultFileStorageData
    {
        public bool Cancel { get; set; }
        public string StorageFilePath { get; set; }
    }
}
