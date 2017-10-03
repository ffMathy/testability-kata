using Autofac;
using FluffySpoon.Testing.Autofake;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace TestabilityKata.Tests
{
    [TestClass]
    public class ProgramTest
    {
        [TestMethod]
        public void ProgramSendsEmailWhenStartingUp()
        {
            var fakeMailSender = Substitute.For<IMailSender>();
            var fakeLogger = Substitute.For<ILogger>();

            var program = new Program(
                fakeLogger,
                fakeMailSender);

            program.Run();

            //we here make sure that SendMail was called
            //with the mail body "Program has started." and
            //any e-mail address (since that is not important for this test)
            fakeMailSender
                .Received()
                .SendMail(
                    Arg.Any<string>(),
                    "Program has started.");
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
