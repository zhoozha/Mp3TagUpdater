﻿using Utils;
using System;

#if VSNET
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
    using NUnit.Framework;
    using TestMethod = NUnit.Framework.TestAttribute;
    using TestClass = NUnit.Framework.TestFixtureAttribute;
    using TestInitialize = NUnit.Framework.SetUpAttribute;
    using TestCleanup = NUnit.Framework.TearDownAttribute;
#endif

namespace TestProject
{
    /// <summary>
    ///This is a test class for ExceptionMessageBoxTest and is intended
    ///to contain all ExceptionMessageBoxTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExceptionMessageBoxTest
    {
        //private TestContext testContextInstance;

        ///// <summary>
        /////Gets or sets the test context which provides
        /////information about and functionality for the current test run.
        /////</summary>
        //public TestContext TestContext
        //{
        //    get
        //    {
        //        return testContextInstance;
        //    }
        //    set
        //    {
        //        testContextInstance = value;
        //    }
        //}

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Show
        ///</summary>
        [TestMethod()]
        public void TestExceptionMessageBox()
        {
            try
            {
                ApplicationException inner = new ApplicationException("inner exception");
                throw new ApplicationException("a message", inner);
            }
            catch( Exception ex )
            {
                ExceptionMessageBox.Show(ex, "a title");
            }
            //Assert.Inconclusive("gui test requires user to verify it looks right.");
        }
    }
}
