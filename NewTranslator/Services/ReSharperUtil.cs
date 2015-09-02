using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NewTranslator.Services
{
    /// <summary>
    /// Interface for getting information about the R# install.  
    /// </summary>
    internal interface IReSharperUtil
    {
        bool IsInstalled { get; }
    }

    [Export(typeof(IReSharperUtil))]
    internal sealed class ReSharperUtil : IReSharperUtil
    {
        // https://github.com/jaredpar/VsVim/tree/master/Src/VsVimShared/Implementation/ReSharper

        private static readonly Guid s_resharper5Guid = new Guid("0C6E6407-13FC-4878-869A-C8B4016C57FE");

        private readonly bool _isResharperInstalled;

        [ImportingConstructor]
        internal ReSharperUtil(SVsServiceProvider serviceProvider)
        {
            var vsShell = ServicesHelper.GetServiceFromPackage<IVsShell, SVsShell>();
            // var vsShell = serviceProvider.GetService<SVsShell, IVsShell>();
            _isResharperInstalled = vsShell.IsPackageInstalled(s_resharper5Guid);
        }

        internal ReSharperUtil(bool isResharperInstalled)
        {
            _isResharperInstalled = isResharperInstalled;
        }
        
        bool IReSharperUtil.IsInstalled
        {
            get { return _isResharperInstalled; }
        }
    }
}