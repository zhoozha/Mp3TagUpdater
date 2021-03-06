// Copyright(C) 2002-2009 Hugo Rumayor Montemayor, All rights reserved.
using System;
using System.Text;
using System.IO;

namespace Id3Lib
{
    /// <summary>
    /// Manage unique identifer frames.
    /// </summary>
    /// <remarks>
    ///   This frame's purpose is to be able to identify the audio file in a
    ///   database, that may provide more information relevant to the content.
    /// </remarks>
    [Frame("UFID")]
    public class FrameUniqueIdentifier : FrameBase, IFrameDescription
    {
        #region Fields
        private string _description;
        private byte[] _identifer;
        #endregion

        #region Constructors
        /// <summary>
        /// Create a FrameGEOB frame.
        /// </summary>
        /// <param name="frameId">ID3v2 UFID frame</param>
        public FrameUniqueIdentifier(string frameId)
            : base(frameId)
        {

        }
        #endregion

        #region Properties
        /// <summary>
        /// Frame description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Binary representation of the object
        /// </summary>
        public byte[] Identifier
        {
            get { return _identifer; }
            set
            {
                if (value.Length > 64)
                    throw new ArgumentOutOfRangeException("value", "The identifer can't be more than 64 bytes");
                _identifer = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Parse the binary UFID frame
        /// </summary>
        /// <param name="frame">binary frame</param>
        public override void Parse(byte[] frame)
        {
            int index = 0;
            _description = TextBuilder.ReadASCII(frame, ref index);
            _identifer = new byte[frame.Length - index];
            Memory.Copy(frame, index, _identifer, 0, frame.Length - index);
        }

        /// <summary>
        /// Create a binary UFID frame
        /// </summary>
        /// <returns>binary frame</returns>
        public override byte[] Make()
        {
            MemoryStream buffer = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(buffer);
            writer.Write(TextBuilder.WriteASCII(_description));
            writer.Write(_identifer);
            return buffer.ToArray();
        }

        /// <summary>
        /// Unique Tag Identifer description 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _description;
        }
        #endregion
    }
}
