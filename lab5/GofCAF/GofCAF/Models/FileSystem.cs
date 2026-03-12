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

        public FileSystemItem(string name)
        {
            Name = name;
        }

        public abstract long Size { get; }
        public abstract void Add(FileSystemItem item);

        public abstract void Remove(FileSystemItem item);
        public abstract FileSystemItem? GetChild(int index);
    }

    public class File : FileSystemItem
    {
        protected long _size;
        public override long Size => _size;

        public File(long size, string name) : base(name)
        {
            _size = size;
        }

        public override void Add(FileSystemItem item)
        {
            throw new InvalidOperationException("File cannot have children");
        }

        public override void Remove(FileSystemItem item)
        {
            throw new InvalidOperationException("File cannot have children");
        }

        public override FileSystemItem? GetChild(int index)
        {
            throw new InvalidOperationException("File cannot have children");
        }
    }

    public class Folder : FileSystemItem
    {
        protected readonly List<FileSystemItem> Children;

        public Folder(string name) : base(name)
        {
            this.Children = new List<FileSystemItem>();
        }

        public override long Size => Children.Sum(c => c.Size);

        public override void Add(FileSystemItem item)
        {
            if (Children.Any(i => i.Name == item.Name))
                throw new InvalidOperationException("Item with this name already exists");

            Children.Add(item);
        }

        public override void Remove(FileSystemItem item)
        {
            if (!Children.Contains(item))
                throw new InvalidOperationException("File not found");
            
            Children.Remove(item);
        }

        public override FileSystemItem? GetChild(int index) =>
            Children.ElementAtOrDefault(index);
    }


}
