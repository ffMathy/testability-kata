using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestabilityKata.Tests
{
    [TestClass]
    public class MailSenderTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MailSenderThrowsExceptionIfEmailIsInvalid()
        {
            var mailSender = new MailSender();
            mailSender.SendMail("my-invalid-email", "some content");
        }
    }
}
