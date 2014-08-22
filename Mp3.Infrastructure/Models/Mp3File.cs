using System;
using System.Collections.Generic;
using System.IO;

namespace Mp3.Models
{
    public class Mp3File : Mp3Lib.Mp3File
    {
        public string Path { get; set; }
        public bool IsProcessed { get; set; }

        /// <summary>
        /// Construct from file name
        /// </summary>
        /// <param name="file"></param>
        public Mp3File(string file)
            : this(new FileInfo(file))
        {
        }

        /// <summary>
        /// Construct from file info
        /// </summary>
        /// <param name="fileinfo"></param>
        public Mp3File(FileInfo fileinfo):base(fileinfo)
        {
        }
    }
}
