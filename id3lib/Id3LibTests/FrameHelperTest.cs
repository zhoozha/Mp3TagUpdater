using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Id3Lib.Tests
{
    [TestFixture]
    public class FrameHelperTest
    {
        [Test]
        public void TagCompression()
        {
            FrameModel frameModel = new FrameModel();

            FrameHelper frameHelper = new FrameHelper(frameModel.Header);

            FrameText originalFrame = (FrameText)FrameFactory.Build("TALB");
            originalFrame.Text = "Hello World!!!";

            OptionHandler flagHandler = new OptionHandler(frameModel.Header);
            flagHandler.Flags = originalFrame.Flags;
            flagHandler.Compression = true;
            originalFrame.Flags = flagHandler.Flags;

            byte[] body = frameHelper.Make(originalFrame);

            FrameText resultFrame = (FrameText)frameHelper.Build("TALB", originalFrame.Flags, body);

            Assert.AreEqual(resultFrame.Text, originalFrame.Text);
        }
    }
}
