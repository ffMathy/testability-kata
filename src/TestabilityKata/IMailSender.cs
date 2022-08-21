namespace TestabilityKata
{
    public interface IMailSender
    {
        void SendMail(string recipient, string content);
    }
}