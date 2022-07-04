using Azure.Messaging.ServiceBus;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataCaptureService
{
    class CapturedDataSender
    {
        private readonly string connectionString;
        private readonly string queueName;

        public CapturedDataSender(string connectionString, string queueName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException($"'{nameof(connectionString)}' cannot be null or whitespace.", nameof(connectionString));
            }

            if (string.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentException($"'{nameof(queueName)}' cannot be null or whitespace.", nameof(queueName));
            }

            this.connectionString = connectionString;
            this.queueName = queueName;
        }

        public async Task SendFileToQueue(string pathToFile)
        {
            var client = new ServiceBusClient(connectionString);
            var sender = client.CreateSender(queueName);

            using (var fs = new FileStream(pathToFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                int maxMessageSize = 195_000;
                long fileSize = fs.Length;
                int positionInFile = 0;
                int partitionsAmount = (int)(fileSize / maxMessageSize);
                if (fileSize % maxMessageSize > 0) partitionsAmount++;

                byte[] arrayToSent = new byte[maxMessageSize];
                var fileName = Path.GetFileName(pathToFile);
                Guid id = Guid.NewGuid();
                int bytesRead;

                for (int numberInSequence = 1; numberInSequence <= partitionsAmount; numberInSequence++)
                {
                    bytesRead = await fs.ReadAsync(arrayToSent, 0, maxMessageSize);
                    if (bytesRead < maxMessageSize)
                    {
                        var temporaryArray = new byte[bytesRead];
                        Array.Copy(arrayToSent, temporaryArray, bytesRead);
                        arrayToSent = temporaryArray;
                    }

                    var message = new MessageSequencePart(id, fileName, numberInSequence, partitionsAmount, positionInFile, arrayToSent);
                    var data = BinaryData.FromObjectAsJson(message);
                    await sender.SendMessageAsync(new ServiceBusMessage(data));
                    Console.WriteLine($"A messages with file {fileName} part {numberInSequence} from {partitionsAmount} has been published to the queue.");
                    positionInFile += bytesRead;
                }
            }

        }

    }
}
