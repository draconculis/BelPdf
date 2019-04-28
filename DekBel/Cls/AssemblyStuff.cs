using System;
using System.Diagnostics;

namespace Dek.Bel.Cls
{
    public class AssemblyStuff
    {
        public static string AssemblyVersion => GetAssemblyVersion();

        public static string GetAssemblyVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            return version;
        }
    }
}
