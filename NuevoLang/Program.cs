namespace NuevoLang;

public static class Program
{
    public static void Main(string[] args)
    {
        // Nuevo lang
        
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: nuevo <file path>");
            return;
        }

        string path = args[0];
        string fileName = Path.GetFileName(path);
        Console.WriteLine($"Starting compilation of {fileName}");
    }
}