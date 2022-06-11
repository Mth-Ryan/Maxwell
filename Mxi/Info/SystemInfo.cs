using System.Runtime.InteropServices;

namespace Maxwell.Mxi.Info;

public static class SystemInfo
{
    public static String DotnetVersion => Environment.Version.ToString();

    // TODO: Get kernel name only.
    public static String OsName => RuntimeInformation.OSDescription;
}