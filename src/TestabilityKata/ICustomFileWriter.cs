namespace TestabilityKata
{
    public interface ICustomFileWriter
    {
        string FilePath { get; }

        void AppendLine(string line);
    }
}