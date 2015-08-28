using EnvDTE;
using NewTranslator.Settings;

namespace NewTranslator
{
    static class Global
    {		
		public static DTE DTE = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE)) as DTE;
		public static Options Options = Options.Get();
    };	
}