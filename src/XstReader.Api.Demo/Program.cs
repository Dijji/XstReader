using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XstReader;

namespace XstReader.Api.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var xstFile  = new XstFile(@"C:\Dev\pst\Backup_20200904.pst"))
            {
                var folder = xstFile.RootFolder;
                foreach(var message in folder.Messages)
                    message.FileAttachmentDisplayList
            }
        }
    }
}
