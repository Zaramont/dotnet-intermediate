using System;
using System.Threading.Tasks;

namespace DataProcessingService
{
    class Program
    {
        private static string directoryWithProcessedFiles = @"C:\.Net\03.Messaging\DataCaptureService\DataProcessingService\ProcessedFiles\";
        private static string connectionString = @"Endpoint=sb://files-queue.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=60Lp4ii2nTiADl9a1JM68MZ8mD7DGpu232CxGqmYw+c=";
        private static string queueName = "capturedFiles";

        static async Task Main(string[] args)
        {
            try
            {
                var processingService = new DataProcessingService(connectionString, queueName);
                Console.WriteLine("Started processing service.");
                await processingService.StartProcessing(directoryWithProcessedFiles);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
