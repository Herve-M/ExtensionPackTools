// ------------------------------------------------------------------------------
// <auto-generated>
//     This file was generated by Extensibility Tools v1.10.211
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ExtensionPackTools
{
    using System;
    
    /// <summary>
    /// Helper class that exposes all GUIDs used across VS Package.
    /// </summary>
    internal sealed partial class PackageGuids
    {
        public const string guidVsPackageString = "3ec2fa73-1f0d-4e31-88c3-604c4e46ec14";
        public const string guidExportPackageCmdSetString = "e84b4658-2e40-46fc-90e5-f29db9b73b46";
        public static Guid guidVsPackage = new Guid(guidVsPackageString);
        public static Guid guidExportPackageCmdSet = new Guid(guidExportPackageCmdSetString);
    }
    /// <summary>
    /// Helper class that encapsulates all CommandIDs uses across VS Package.
    /// </summary>
    internal sealed partial class PackageIds
    {
        public const int MyMenu = 0x0001;
        public const int MyMenuGroup = 0x1020;
        public const int ExportCmd = 0x0100;
        public const int ImportCmd = 0x0200;
        public const int ExportSolutionCmd = 0x0300;
    }
}
