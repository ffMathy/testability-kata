using Autofac;
using FluffySpoon.Testing.Autofake;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestabilityKata.Tests
{
    [TestClass]
    public class ProgramTest
    {
        [TestMethod]
        public void ProgramSendsEmailWhenStartingUp()
        {
            //problem: we certainly don't want to send e-mails out
            Program.Main(new string[0]);
        }
        
        [TestMethod]
        public void ProgramLogsErrorWhenExceptionIsThrown()
        {
            //problem: how can we know if the logging works, and how can we
            //simulate an error when we can't modify the MailSender?
            Program.Main(new string[0]);
        }
    }
}
