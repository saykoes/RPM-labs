using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofCAF.Models
{
    internal interface IFileSystem
    {
        List<string> ListItems(string path);
        byte[]? ReadFile(string path);
        void WriteFile(string path, byte[] data);
        void DeleteItem(string path);
    }
}
