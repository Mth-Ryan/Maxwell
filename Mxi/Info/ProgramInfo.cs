using System.Reflection;

namespace Maxwell.Mxi.Info;

public static class ProgramInfo
{
    public static String Version => Assembly.GetEntryAssembly()!
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
            .InformationalVersion;
}