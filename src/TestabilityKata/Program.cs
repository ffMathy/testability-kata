﻿using System;
using System.IO;

namespace TestabilityKata
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Logger.Log(LogLevel.Information, "Program is starting up or whatever");
                MailSender.SendMail("some-invalid-email-address.com", "Program has started.");
            } 
            catch(Exception ex)
            {
                Logger.Log(LogLevel.Error, "An error occured: " + ex);
            }
        }
    }

    public enum LogLevel
    {
        Debug,
        Information,
        Warning,
        Error
    }

    public static class Logger
    {
        public static void Log(LogLevel logLevel, string logText)
        {
            Console.WriteLine(logLevel + ": " + logText);

            if (logLevel == LogLevel.Error || logLevel == LogLevel.Warning)
            {

                //also log to file
                var writer = new CustomFileWriter(@"C:\" + logLevel + "-annoying-log-file.txt");
                writer.AppendLine(logText);

                //send e-mail about error
                MailSender.SendMail("mathias.lorenzen@mailinator.com", logText);

            }
        }
    }

    public static class MailSender
    {
        public static void SendMail(string recipient, string content)
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
            lock(typeof(CustomFileWriter)) {
                if (!File.Exists(FilePath))
                {
                    MailSender.SendMail("mathias.lorenzen@mailinator.com", "The file " + FilePath + " was created since it didn't exist.");
                    File.WriteAllText(FilePath, "");
                }

                File.SetAttributes(FilePath, FileAttributes.Normal);
                File.AppendAllLines(FilePath, new[] { line });
                File.SetAttributes(FilePath, FileAttributes.ReadOnly);
            }
        }
    }
}
