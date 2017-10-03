using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestabilityKata.Tests
{
    [TestClass]
    public class CustomFileWriterTest
    {
        private IMailSender fakeMailSender;

        [TestInitialize]
        public void Initialize()
        {
            fakeMailSender = Substitute.For<IMailSender>();
        }

        [TestMethod]
        public void WhenCreatingFileAnEmailIsSentOut()
        {
            const string testFilePath = @"C:\WhenCreatingFileAnEmailIsSentOut.txt";

            var customFileWriter = new CustomFileWriter(
                fakeMailSender,
                testFilePath);

            //if the existing file exists, delete it before running the test, since
            //we need to get to the point where a new file is created.
            if (File.Exists(testFilePath))
            {
                File.SetAttributes(testFilePath, FileAttributes.Normal);
                File.Delete(testFilePath);
            }

            customFileWriter.AppendLine("Some line");

            fakeMailSender
                .Received()
                .SendMail(
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }
    }
}
