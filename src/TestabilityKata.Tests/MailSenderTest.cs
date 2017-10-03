using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestabilityKata.Tests
{
    [TestClass]
    public class MailSenderTest
    {
        private IMailSender mailSender;

        [TestInitialize]
        public void Initialize()
        {
            var container = new UnitTestHelper().CreateIocContainerFor<IMailSender>();
            mailSender = container.Resolve<IMailSender>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MailSenderThrowsExceptionIfEmailIsInvalid()
        {
            mailSender.SendMail("my-invalid-email", "some content");
        }
    }
}
