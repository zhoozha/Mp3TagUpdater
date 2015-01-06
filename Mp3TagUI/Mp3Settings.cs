using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3.Infrastructure.Interfaces;
using Mp3TagUI.Properties;

namespace Mp3TagUI
{
    public class Mp3Settings : IMp3Settings
    {
        Settings _settings = Properties.Settings.Default;

        public string FolderFrom
        {
            get
            {
                return _settings.FolderFrom;
            }
            set
            {
                _settings.FolderFrom = value;
            }
        }

        public string FolderTo
        {
            get
            {
                return _settings.FolderTo;
            }
            set
            {
                _settings.FolderTo = value;
            }
        }

        public bool ProcessTags
        {
            get
            {
                return _settings.ProcessTags;
            }
            set
            {
                _settings.ProcessTags = value;
            }
        }

        public bool ShowLog
        {
            get
            {
                return _settings.ShowLog;
            }
            set
            {
                _settings.ShowLog = value;
            }
        }

        public bool DelUserTags 
        {
            get
            {
                return _settings.DelUserTags;
            }
            set
            {
                _settings.DelUserTags = value;
            }
        }
        public bool DelCopyRight 
        {
            get
            {
                return _settings.DelCopyRight;
            }
            set
            {
                _settings.DelCopyRight = value;
            }
        }

        public void Read()
        {
            _settings.Reload();
        }

        public void Save()
        {
            _settings.Save();
        }
    }
}
