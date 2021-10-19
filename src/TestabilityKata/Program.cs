using System;
using System.IO;

namespace TestabilityKata
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Program().Run();
        }

        public void Run()
        {
            try
            {
                new Logger().Log(LogLevel.Warning, "Some warning - program is starting up or whatever");
                new MailSender().SendMail("some-invalid-email-address.com", "Program has started.");
            }
            catch (Exception ex)
            {
                new Logger().Log(LogLevel.Error, "An error occured: " + ex);
            }
        }
    }

    enum LogLevel
    {
        Warning,
        Error
    }

    class Logger
    {
        public void Log(LogLevel logLevel, string logText)
        {
            Console.WriteLine(logLevel + ": " + logText);

            if (logLevel == LogLevel.Error)
            {

                //also log to file
                var writer = new CustomFileWriter(@"C:\" + logLevel + "-annoying-log-file.txt");
                writer.AppendLine(logText);

                //send e-mail about error
                new MailSender().SendMail("mathias.lorenzen@mailinator.com", logText);

            }
        }
    }

    class MailSender
    {
        public void SendMail(string recipient, string content)
        {
            if (!recipient.Contains("@"))
                throw new ArgumentException("The recipient must be a valid e-mail.", nameof(recipient));

            //for the sake of simplicity, this actually doesn't send an e-mail right now - but let's pretend it does.
            Console.WriteLine("Sent e-mail to " + recipient + " with content \"" + content + "\"");
        }
    }

    class CustomFileWriter
    {
        public string FilePath { get; }

        public CustomFileWriter(string filePath)
        {
            FilePath = filePath;
        }

        public void AppendLine(string line)
        {
            lock(typeof(CustomFileWriter)) {
                if (!File.Exists(FilePath))
                {
                    new MailSender().SendMail("mathias.lorenzen@mailinator.com", "The file " + FilePath + " was created since it didn't exist.");
                    File.WriteAllText(FilePath, "");
                }

                File.SetAttributes(FilePath, FileAttributes.Normal);
                File.AppendAllLines(FilePath, new[] { line });
                File.SetAttributes(FilePath, FileAttributes.ReadOnly);
            }
        }
    }
}
