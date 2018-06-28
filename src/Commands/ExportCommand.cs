﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.ExtensionManager;
using Microsoft.VisualStudio.Shell;
using Newtonsoft.Json;

namespace ExtensionPackTools
{
    internal sealed class ExportCommand
    {
        private readonly Package _package;

        private ExportCommand(Package package, OleMenuCommandService commandService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));

            var cmdId = new CommandID(PackageGuids.guidExportPackageCmdSet, PackageIds.ExportCmd);
            var cmd = new MenuCommand(Execute, cmdId);
            commandService.AddCommand(cmd);
        }

        public static ExportCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return _package; }
        }

        public static void Initialize(Package package, OleMenuCommandService commandService)
        {
            Instance = new ExportCommand(package, commandService);
        }

        private void Execute(object sender, EventArgs e)
        {
            var manager = ServiceProvider.GetService(typeof(SVsExtensionManager)) as IVsExtensionManager;
            var repository = ServiceProvider.GetService(typeof(SVsExtensionRepository)) as IVsExtensionRepository;

            try
            {
                IEnumerable<Extension> extensions = ExtensionHelpers.GetInstalledExtensions(manager, repository);

                var dialog = Importer.ImportWindow.Open(extensions, Importer.Purpose.List);

                if (dialog.DialogResult == true)
                {
                    if (!TryGetFilePath(out string filePath))
                    {
                        return;
                    }

                    var manifest = new Manifest(dialog.SelectedExtension);
                    string json = JsonConvert.SerializeObject(manifest, Formatting.Indented);

                    File.WriteAllText(filePath, json);
                    VsShellUtilities.OpenDocument(ServiceProvider, filePath);
                }
            }
            catch (Exception ex)
            {
                VsShellUtilities.ShowMessageBox(
                    ServiceProvider,
                    ex.Message,
                    Vsix.Name,
                    Microsoft.VisualStudio.Shell.Interop.OLEMSGICON.OLEMSGICON_WARNING,
                    Microsoft.VisualStudio.Shell.Interop.OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    Microsoft.VisualStudio.Shell.Interop.OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST
                );
            }
        }

        private bool TryGetFilePath(out string filePath)
        {
            filePath = null;

            using (var sfd = new SaveFileDialog())
            {
                sfd.DefaultExt = ".vsext";
                sfd.FileName = "extensions";
                sfd.Filter = "VSEXT File|*.vsext";

                DialogResult result = sfd.ShowDialog();

                if (result == DialogResult.OK)
                {
                    filePath = sfd.FileName;
                    return true;
                }
            }

            return false;
        }
    }
}
