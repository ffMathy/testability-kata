using System;
using System.IO;

namespace TestabilityKata
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Program(
                    new Logger(
                        new MailSender()),
                    new MailSender())
                .Run();
        }

        private readonly Logger logger;
        private readonly MailSender mailSender;

        public Program(
            Logger logger,
            MailSender mailSender)
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

    public class Logger
    {
        //we can't do CustomFileWriter yet, because its file name depends on the log level.
        //see the next step for that.

        private readonly MailSender mailSender;

        public Logger(
            MailSender mailSender)
        {
            this.mailSender = mailSender;
        }

        public void Log(LogLevel logLevel, string logText)
        {
            Console.WriteLine(logLevel + ": " + logText);

            if (logLevel == LogLevel.Error)
            {

                //also log to file
                var writer = new CustomFileWriter(@"C:\" + logLevel + "-annoying-log-file.txt");
                writer.AppendLine(logText);

                //send e-mail about error
                mailSender.SendMail("mathias.lorenzen@mailinator.com", logText);

            }
        }
    }

    public class MailSender
    {
        public void SendMail(string recipient, string content)
        {
            if (!recipient.Contains("@"))
                throw new ArgumentException("The recipient must be a valid e-mail.", nameof(recipient));

            //for the sake of simplicity, this actually doesn't send an e-mail right now - but let's pretend it does.
            Console.WriteLine("Sent e-mail to " + recipient + " with content \"" + content + "\"");
        }
    }

    public class CustomFileWriter
    {
        public string FilePath { get; }

        public CustomFileWriter(string filePath)
        {
            FilePath = filePath;
        }

        public void AppendLine(string line)
        {
            if (!File.Exists(FilePath))
                File.WriteAllText(FilePath, "");

            File.SetAttributes(FilePath, FileAttributes.Normal);
            File.AppendAllLines(FilePath, new[] { line });
            File.SetAttributes(FilePath, FileAttributes.ReadOnly);
        }
    }
}
