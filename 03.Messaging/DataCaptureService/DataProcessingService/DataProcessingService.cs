using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DataProcessingService
{
    class DataProcessingService
    {
        private readonly string connectionString;
        private readonly string queueName;
        private static Dictionary<Guid, int> sequences = new Dictionary<Guid, int>();

        public DataProcessingService(string connectionString, string queueName)
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

        internal async Task StartProcessing(string directoryForProcessedFiles)
        {
            await EnsureQueueExisting();

            if (!Directory.Exists(directoryForProcessedFiles))
            {
                Directory.CreateDirectory(directoryForProcessedFiles);
            }

            var client = new ServiceBusClient(connectionString);
            var receiver = client.CreateReceiver(queueName);

            while (true)
            {
                var message = await receiver.ReceiveMessageAsync();
                if (message is null) continue;

                MessageSequencePart parfOfMessageSequence = message?.Body.ToObjectFromJson<MessageSequencePart>();

                string fullPath = Path.Combine(directoryForProcessedFiles, parfOfMessageSequence.FileName);
                using (var fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Write))
                {
                    fs.Seek(parfOfMessageSequence.PositionInFile, SeekOrigin.Begin);
                    await fs.WriteAsync(parfOfMessageSequence.Data);
                    if (sequences.ContainsKey(parfOfMessageSequence.Id))
                    {
                        int currentValue = --sequences[parfOfMessageSequence.Id];
                        if (currentValue == 0)
                        {
                            sequences.Remove(parfOfMessageSequence.Id);
                        }
                    }
                    else
                    {
                        sequences.Add(parfOfMessageSequence.Id, parfOfMessageSequence.SequenceSize);
                    }
                    await receiver.CompleteMessageAsync(message);
                }

                Console.WriteLine($"Guid {parfOfMessageSequence.Id} part {parfOfMessageSequence.PositionInSequence} from {parfOfMessageSequence.SequenceSize} Body: {System.Text.Encoding.Default.GetString(parfOfMessageSequence?.Data).Length}");
            }
        }

        private async Task EnsureQueueExisting()
        {
            var managementClient = new ManagementClient(connectionString);
            if (await managementClient.QueueExistsAsync(queueName) == false)
            {
                await managementClient.CreateQueueAsync(new QueueDescription(queueName));
            };
        }
    }
}
