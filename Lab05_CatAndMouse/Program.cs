using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 2)
        {
            RunGame(args[0], args[1]);
            return;
        }
        for (int i = 1; i <= 3; i++)
        {
            string input = "data/" + i + ".ChaseData.txt";
            string output = "out/" + i + ".PursuitLog.txt";
            string expected = "data/" + i + ".PursuitLog.txt";

            RunGame(input, output);

            bool ok = Normalize(File.ReadAllText(output)) == Normalize(File.ReadAllText(expected));
            string result = ok ? "OK" : "FAILED";
            Console.WriteLine("Test " + i + ": " + result);
            Console.WriteLine();
        }
    }

    static void RunGame(string input, string output)
    {
        Game.InputFile = input;
        Game.OutFile = output;

        Game game = new Game(Game.ReadFieldSize());
        game.Run(); 
    }
    static string Normalize(string text)
    {
        return text.Replace("\r\n", "\n");
    }
}
