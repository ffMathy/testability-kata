using Autofac;
using System;
using System.IO;

namespace TestabilityKata
{
    public class Program : IProgram
    {
        public static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new TestabilityKataAutofacConfiguration());

            var container = containerBuilder.Build();
            container
                .Resolve<Program>()
                .Run();
        }

        private readonly ILogger logger;
        private readonly IMailSender mailSender;

        public Program(
            ILogger logger,
            IMailSender mailSender)
        {
            this.logger = logger;
            this.mailSender = mailSender;
        }

        public void Run()
        {
            try
            {
                logger.Log(LogLevel.Warning, "Some warning - program is starting up or whatever");
                mailSender.SendMail("some-invalid-email-address.com", "Program has started.");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "An error occured: " + ex);
            }
        }
    }

    public enum LogLevel
    {
        Warning,
        Error
    }

    public class Logger : ILogger
    {
        private readonly IMailSender mailSender;
        private readonly ICustomFileWriterFactory customFileWriterFactory;

        public Logger(
            IMailSender mailSender,
            ICustomFileWriterFactory customFileWriterFactory)
        {
            this.mailSender = mailSender;
            this.customFileWriterFactory = customFileWriterFactory;
        }

        public void Log(LogLevel logLevel, string logText)
        {
            Console.WriteLine(logLevel + ": " + logText);

            if (logLevel == LogLevel.Error)
            {

                //also log to file
                var writer = customFileWriterFactory.Create(@"C:\" + logLevel + "-annoying-log-file.txt");
                writer.AppendLine(logText);

                //send e-mail about error
                mailSender.SendMail("mathias.lorenzen@mailinator.com", logText);

            }
        }
    }

    public class MailSender : IMailSender
    {
        public void SendMail(string recipient, string content)
        {
            if (!recipient.Contains("@"))
                throw new ArgumentException("The recipient must be a valid e-mail.", nameof(recipient));

            //for the sake of simplicity, this actually doesn't send an e-mail right now - but let's pretend it does.
            Console.WriteLine("Sent e-mail to " + recipient + " with content \"" + content + "\"");
        }
    }

    public class CustomFileWriterFactory : ICustomFileWriterFactory
    {
        private readonly IMailSender mailSender;

        public CustomFileWriterFactory(
            IMailSender mailSender)
        {
            this.mailSender = mailSender;
        }

        public ICustomFileWriter Create(string filePath)
        {
            return new CustomFileWriter(
                mailSender, 
                filePath);
        }
    }

    public class CustomFileWriter : ICustomFileWriter
    {
        private readonly IMailSender mailSender;

        public string FilePath { get; }

        public CustomFileWriter(
            IMailSender mailSender,
            string filePath)
        {
            this.mailSender = mailSender;

            FilePath = filePath;
        }

        public void AppendLine(string line)
        {
            if (!File.Exists(FilePath))
            {
                mailSender.SendMail("mathias.lorenzen@mailinator.com", "The file " + FilePath + " was created since it didn't exist.");
                File.WriteAllText(FilePath, "");
            }

            File.SetAttributes(FilePath, FileAttributes.Normal);
            File.AppendAllLines(FilePath, new[] { line });
            File.SetAttributes(FilePath, FileAttributes.ReadOnly);
        }
    }
}
