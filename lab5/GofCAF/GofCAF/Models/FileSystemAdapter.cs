using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofCAF.Models
{
    internal class FileSystemAdapter : IFileSystem
    {
        private FileSystemItem _root;

        public FileSystemAdapter(FileSystemItem root)
        {
            _root = root;
        }

        private FileSystemItem FindByPath(string path)
        {
            var parts = path?.Trim().Split('/', '\\').Where(p => !string.IsNullOrEmpty(p)) ?? Enumerable.Empty<string>();
            FileSystemItem current = _root;

            foreach (var part in parts)
            {
                if (current is Folder folder && folder.Children.FirstOrDefault(c => c.Name == part) is FileSystemItem found)
                    current = found;
                else
                    throw new InvalidOperationException($"{path}: '{part}' not found");
            }

            return current;
        }

        public List<string> ListItems(string path)
        {
            var result = new List<string>();
            var item = FindByPath(path);

            if (item is Folder folder)
            {
                foreach (var child in folder.Children)
                {
                    if (child is File)
                        result.Add($"[File] \t{child.Name} \t{child.Size} Bytes");
                    else if (child is Folder)
                        result.Add($"[Folder] \t{child.Name}");
                }
            }
            else
                result.Add($"[File] \t{item.Name} \t{item.Size} Bytes");

            return result;
        }

        public byte[]? ReadFile(string path)
        {
            var item = FindByPath(path);

            if (item is File file)
                return file.Data;
            
            throw new InvalidOperationException($"{path}: '{item.Name}' is not a File");
        }

        public void WriteFile(string path, byte[] data)
        {
            var parts = path.Split('/', '\\').Where(p => !string.IsNullOrEmpty(p)).ToList();
            if (!parts.Any()) return;

            string fileName = parts.Last();
            var current = (Folder)_root;

            // Find and Create all non-existing folders
            foreach (var part in parts.SkipLast(1))
            {
                var existing = current.Children.FirstOrDefault(c => c.Name == part);

                if (existing == null)
                {
                    existing = new Folder(part);
                    current.Add(existing);
                }

                if (existing is Folder folder)
                {
                    current = folder;
                }

                else
                    throw new InvalidOperationException($"{path}: '{part}' is not a Folder and already exists");
            }

            // Write file and create if non-existent
            var item = current.Children.FirstOrDefault(c => c.Name == fileName);
            
            if (item == null)
            {
                item = new File(fileName);
                current.Add(item);
            }

            if (item is File file)
                file.Data = data?.ToArray();
            else
                throw new InvalidOperationException($"{path}: '{item.Name}' is not a File and already exists");
                
        }

        public void DeleteItem(string path)
        {
            var item = FindByPath(path);
            if (path == "/") throw new InvalidOperationException("Cannot delete root");

            if(item is Folder folder && folder.Children.Count > 0) throw new InvalidOperationException("Cannot delete non-empty folder");

            int lastSlash = path.LastIndexOf('/');
            string parentPath = lastSlash == 0 ? "/" : path.Substring(0, lastSlash);
            var parent = FindByPath(parentPath);

            parent.Remove(item);

            if (item is File file)
                file.Data = null;

            item = null;
            return;
        }

    }
}
