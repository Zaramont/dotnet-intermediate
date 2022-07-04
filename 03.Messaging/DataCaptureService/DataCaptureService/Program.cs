using System;

namespace DataCaptureService
{
    class Program
    {
        private static string directoryWithFilesForCapture = @".\\CapturedFiles";
        private static string connectionString = @"Endpoint=sb://files-queue.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=60Lp4ii2nTiADl9a1JM68MZ8mD7DGpu232CxGqmYw+c=";
        private static string queueName = "capturedFiles";
        private static string fileExtensionTxt = "*.txt";
        private static string fileExtensionPdf = "*.pdf";

        static void Main(string[] args)
        {
            var dataSender = new CapturedDataSender(connectionString, queueName);
            var watcher = new DirectoryWatcher(dataSender);
            Console.WriteLine($"Started watching for {fileExtensionTxt} files.");
            watcher.WatchForDirectory(directoryWithFilesForCapture, fileExtensionTxt);

            var watcher2 = new DirectoryWatcher(dataSender);
            Console.WriteLine($"Started watching for {fileExtensionPdf} files.");
            watcher.WatchForDirectory(directoryWithFilesForCapture, fileExtensionPdf);

            Console.ReadLine();
        }
    }
}
