using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3.Models
{
    public class Mp3Folder
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public IEnumerable<Mp3File> Mp3Files { get; set; }
    }
}
