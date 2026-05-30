using System.Diagnostics;

class Program
{
    static void Main()
    {
        Game game = new Game();
        game.Start();

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}