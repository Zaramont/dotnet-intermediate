using System;
using System.IO;
using System.Threading;

namespace DataCaptureService
{
    class DirectoryWatcher
    {
        private FileSystemWatcher watcher;
        private readonly CapturedDataSender dataSender;

        public DirectoryWatcher(CapturedDataSender dataSender)
        {
            this.dataSender = dataSender;
        }

        public void WatchForDirectory(string path, string extension)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            watcher = new FileSystemWatcher(path)
            {
                EnableRaisingEvents = true,
                Filter = extension
            };

            watcher.Created += SendFileToQueue;
        }

        private async void SendFileToQueue(object sender, FileSystemEventArgs e)
        {
            try
            {
                var fileInfo = new FileInfo(e.FullPath);
                while (true) 
                {
                    if (!fileInfo.Exists)
                    {
                        return;
                    }
                    else if (!IsFileLocked(fileInfo))
                    {
                        break;
                    }    
                    
                    Thread.Sleep(200);
                };

                await dataSender.SendFileToQueue(e.FullPath);
                File.Delete(e.FullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }
            return false;
        }
    }
}
