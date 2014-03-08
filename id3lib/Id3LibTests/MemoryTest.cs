// Copyright(C) 2002-2009 Hugo Rumayor Montemayor, All rights reserved.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Id3Lib;
using NUnit.Framework;

namespace Id3Lib.Tests
{
    [TestFixture]
    public class MemoryTest
    {
        [Test]
        [Description("Test conversion from/to integer to a byte array")]
        public void GetBytes()
        {
            byte[] bitTests = new byte[] { 0, 2, 8, 16, 18, 24, 32, 40, 55, 60 };
            foreach (byte bits in bitTests)
            {
                ulong original = (ulong)Math.Pow(2, bits);
                byte[] bytes = Memory.GetBytes(original);

                int length = (bits / 8) + 1;
                Assert.AreEqual(length <= 4 ? 4 : length, bytes.Length);

                ulong restore = Memory.ToInt64(bytes);
                Assert.AreEqual(original, restore);
            }
        }
    }
}
