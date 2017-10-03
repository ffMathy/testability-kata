using Autofac;
using FluffySpoon.Testing.Autofake;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

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

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new TestabilityKataAutofacConfiguration());

            containerBuilder.Register(c => fakeMailSender).As<IMailSender>();
            containerBuilder.Register(c => fakeLogger).As<ILogger>();

            var container = containerBuilder.Build();

            var program = container.Resolve<IProgram>();
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
            var fakeMailSender = Substitute.For<IMailSender>();
            var fakeLogger = Substitute.For<ILogger>();

            var program = new Program(
                fakeLogger,
                fakeMailSender);

            //we here make the EmailSender's SendMail throw an exception no 
            //matter what arguments it was called with.
            fakeMailSender
                .When(x => x.SendMail(
                    Arg.Any<string>(),
                    Arg.Any<string>()))
                .Throw(new Exception());

            program.Run();

            //by now the logger should have received some "error" level log message
            fakeLogger
                .Received()
                .Log(
                    LogLevel.Error,
                    Arg.Any<string>());
        }
    }
}
