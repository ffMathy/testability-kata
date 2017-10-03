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
        [TestMethod]
        public void WhenCreatingFileAnEmailIsSentOut()
        {
            var fakeMailSender = Substitute.For<IMailSender>();

            const string testFilePath = @"C:\WhenCreatingFileAnEmailIsSentOut.txt";

            //if the existing file exists, delete it before running the test, since
            //we need to get to the point where a new file is created.
            if(File.Exists(testFilePath))
            {
                File.SetAttributes(testFilePath, FileAttributes.Normal);
                File.Delete(testFilePath);
            }

            var customFileWriter = new CustomFileWriter(
                fakeMailSender,
                testFilePath);
            customFileWriter.AppendLine("Some line");

            fakeMailSender
                .Received()
                .SendMail(
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }
    }
}
