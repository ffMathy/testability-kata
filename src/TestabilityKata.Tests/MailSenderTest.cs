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
            mailSender = new MailSender();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MailSenderThrowsExceptionIfEmailIsInvalid()
        {
            mailSender.SendMail("my-invalid-email", "some content");
        }
    }
}
