using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3.Infrastructure.Models;

namespace Mp3.Infrastructure
{
    public interface IFolderConverter : IConverter<string>
    {
        void Convert(string sourceFolder, string targetFolder);
        void Convert(Mp3Folders model);
    }
}
