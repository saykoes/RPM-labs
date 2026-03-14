using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofCAF.Models
{
    public abstract class FileSystemItem
    {
        public string Name { get; protected set; }
        public FileSystemItem(string name) => Name = name;
        public abstract long Size { get; }
        public abstract void Add(FileSystemItem item);
        public abstract void Remove(FileSystemItem item);
    }

    public class File : FileSystemItem
    {
        public override long Size => Data?.Length ?? 0;
        public byte[]? Data { get; set; }
        public File(string name) : base(name) { }
        public override void Add(FileSystemItem item) => throw new InvalidOperationException("File cannot have children");
        public override void Remove(FileSystemItem item) => throw new InvalidOperationException("File cannot have children");
    }

    public class Folder : FileSystemItem
    {
        protected readonly List<FileSystemItem> _children;
        public IReadOnlyList<FileSystemItem> Children => _children.AsReadOnly();

        public Folder(string name) : base(name) => _children = new List<FileSystemItem>();

        public override long Size => _children.Sum(c => c.Size);
        public override void Add(FileSystemItem item)
        {
            if (_children.Any(i => i.Name == item.Name))
                throw new InvalidOperationException("Item with this name already exists");

            _children.Add(item);
        }
        public override void Remove(FileSystemItem item)
        {
            if (!_children.Contains(item))
                throw new InvalidOperationException("File not found");
            
            _children.Remove(item);
        }

    }


}
