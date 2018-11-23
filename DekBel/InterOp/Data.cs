using System.Runtime.InteropServices;

namespace BelManagedLib
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct EventData

    {
        public int Code { get; set; }
        public string Text { get; set; }
        public string FilePath { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class ResultData
    {
        public int Code { get; set; }
        public bool Cancel { get; set; }
        public string Message { get; set; }
    }
}
