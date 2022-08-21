namespace TestabilityKata
{
    public interface ICustomFileWriterFactory
    {
        ICustomFileWriter Create(string filePath);
    }
}