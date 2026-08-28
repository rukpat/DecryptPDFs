using System.IO;
using System.IO.MemoryMappedFiles;

namespace DecryptPDFs
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // First instance
            ApplicationConfiguration.Initialize();
            Application.Run(new EnD(args));
        }
    }
}