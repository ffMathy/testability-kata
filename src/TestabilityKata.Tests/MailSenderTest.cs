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
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new TestabilityKataAutofacConfiguration());
            
            var container = containerBuilder.Build();
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
