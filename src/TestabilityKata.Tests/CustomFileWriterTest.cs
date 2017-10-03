using Autofac;
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

        private ICustomFileWriterFactory customFileWriterFactory;

        [TestInitialize]
        public void Initialize()
        {
            var container = new TestHelper().CreateAutofakedIocContainerFor<ICustomFileWriter>();
            fakeMailSender = container.Resolve<IMailSender>();

            customFileWriterFactory = container.Resolve<ICustomFileWriterFactory>();
        }

        [TestMethod]
        public void WhenCreatingFileAnEmailIsSentOut()
        {
            const string testFilePath = @"C:\WhenCreatingFileAnEmailIsSentOut.txt";

            var customFileWriter = customFileWriterFactory.Create(testFilePath);

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
