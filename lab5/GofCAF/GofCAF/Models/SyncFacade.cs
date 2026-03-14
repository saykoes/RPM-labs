using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofCAF.Models
{
    internal class SyncFacade
    {
        private IFileSystem _sourceFS;
        private IFileSystem _targetFS;

        // For logging
        public event Action<string>? OnLog;
        private void Log(string message) => OnLog?.Invoke(message); // Invoke because OnLog is nullable

        public SyncFacade(IFileSystem source, IFileSystem target)
        {
            _sourceFS = source;
            _targetFS = target;
        }

        public void SyncFolder(string sourcePath, string targetPath)
        {
            var items = _sourceFS.ListItems(sourcePath);
            foreach (var item in items)
            {
                string itemName = item.Split('\t')[1].Trim();
                string sourceItemPath = (sourcePath == "/") ? $"/{itemName}" : $"{sourcePath}/{itemName}";
                string targetItemPath = (targetPath == "/") ? $"/{itemName}" : $"{targetPath}/{itemName}";

                if (item.StartsWith("[File]"))
                {
                    Log($"Copying '{itemName}'...");
                    byte[] data = _sourceFS.ReadFile(sourceItemPath);
                    _targetFS.WriteFile(targetItemPath, data);
                    Log("OK");
                }
                else if (item.StartsWith("[Folder]"))
                {
                    Log($"Going into '{itemName}/'...");
                    SyncFolder(sourceItemPath, targetItemPath);
                }
            }
            Log("Sync Complete");
        }
        public void Backup(string sourcePath, string backupRootPath)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string backupPath = $"{backupRootPath}/backup_{timestamp}";

            SyncFolder(sourcePath, backupPath);

            Log($"Backup Complete\n");
        }
    }
}
