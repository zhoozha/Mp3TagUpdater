using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3.Infrastructure.Interfaces;

namespace Mp3.Infrastructure.Models
{
    public class Mp3Folders
    {
        IMp3Settings _settings;
        public Mp3Folders(IMp3Settings settings)
        {
            _settings = settings;
            Init();
        }
        private void Init()
        {
            FolderFrom = _settings.FolderFrom;
            FolderTo = _settings.FolderTo;
            ProcessTags = _settings.ProcessTags;
            ShowLog = _settings.ShowLog;
        }
        public string FolderFrom { get; set; }
        public string FolderTo { get; set; }
        public bool ProcessTags { get; set; }
        public bool ShowLog { get; set; }

        ~Mp3Folders()
        {
            _settings.FolderFrom = FolderFrom;
            _settings.FolderTo = FolderTo;
            _settings.ProcessTags = ProcessTags;
            _settings.ShowLog = ShowLog;
            _settings.Save();
        }
    }
}
