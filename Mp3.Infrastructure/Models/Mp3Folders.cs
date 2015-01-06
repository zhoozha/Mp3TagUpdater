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
        public Mp3Folders(IMp3Settings settings)
        {
            this.Settings = settings;
        }

        public IMp3Settings Settings { get; private set; }

        ~Mp3Folders()
        {
            Settings.Save();
        }


    }
}
