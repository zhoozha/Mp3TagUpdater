using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3.Infrastructure.Interfaces
{
    public interface IMp3Settings
    {
        string FolderFrom { get; set; }
        string FolderTo { get; set; }
        bool ProcessTags { get; set; }
        bool ShowLog { get; set; }
        bool DelUserTags { get; set; }
        bool DelCopyRight { get; set; }
        void Read();
        void Save();
    }
}
