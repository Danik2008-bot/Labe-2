using System;
using System.Globalization;
using System.IO;
using System.Text;

enum GameState
{
    Start,
    End
}

class Game
{
    public static string InputFile = "ChaseData.txt";
    public static string OutFile = "PursuitLog.txt";

    private const string Separator = "-------------------";

    public int size; 
    public Player cat;
    public Player mouse;
    public GameState state;

    private int caughtAt = -1;
    private readonly StringBuilder log = new StringBuilder();

    public Game(int size)
    {
        this.size = size;
        cat = new Player("Cat", size);
        mouse = new Player("Mouse", size);
        state = GameState.Start;
    }
    public static int ReadFieldSize()
    {
        using (StreamReader reader = new StreamReader(InputFile))
        {
            return int.Parse(reader.ReadLine().Trim(), CultureInfo.InvariantCulture);
        }
    }

    public void Run()
    {
        string[] lines = File.ReadAllLines(InputFile);
        int next = 1; 

        WriteLine("Cat and Mouse");
        WriteLine("");
        WriteLine("Cat Mouse  Distance");
        WriteLine(Separator);

        while (state != GameState.End)
        {
            while (next < lines.Length && lines[next].Trim() == "")
            {
                next++;
            }
            if (next >= lines.Length)
            {
                state = GameState.End;
                break;
            }

            string[] parts = lines[next].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            next++;
            char command = char.ToUpper(parts[0][0]);

            if (command == 'P')
            {
                DoPrintCommand();
            }
            else
            {
                int steps = int.Parse(parts[1], CultureInfo.InvariantCulture);
                DoMoveCommand(command, steps);
                if (IsMouseCaught())
                {
                    OnMouseCaught();
                }
            }
        }

        if (caughtAt == -1)
        {
            mouse.state = State.Winner;
            cat.state = State.Loser;
        }

        WriteReport();
        SaveLog();
    }

    private void DoMoveCommand(char command, int steps)
    {
        switch (command)
        {
            case 'M': MovePlayer(mouse, steps); break;
            case 'C': MovePlayer(cat, steps); break;
        }
    }

    private void MovePlayer(Player player, int steps)
    {
        if (player.state == State.NotInGame)
        {
            player.PlaceAt(steps);
        }
        else
        {
            player.Move(steps);
        }
    }
    private void DoPrintCommand()
    {
        string line = string.Format("{0,3}{1,6}", Show(cat), Show(mouse));
        if (cat.state != State.NotInGame && mouse.state != State.NotInGame)
        {
            line += string.Format("{0,10}", GetDistance());
        }
        WriteLine(line);
    }

    private static string Show(Player player)
    {
        return player.state == State.NotInGame ? "??" : player.location.ToString();
    }

    private int GetDistance()
    {
        return Math.Abs(cat.location - mouse.location);
    }

    private bool IsMouseCaught()
    {
        return cat.state != State.NotInGame
            && mouse.state != State.NotInGame
            && cat.location == mouse.location;
    }

    private void OnMouseCaught()
    {
        caughtAt = cat.location;
        cat.state = State.Winner;
        mouse.state = State.Loser;
        state = GameState.End;
    }
    private void WriteReport()
    {
        WriteLine(Separator);
        WriteLine("");
        WriteLine("");
        WriteLine("Distance traveled:   Mouse    Cat");
        WriteLine(new string(' ', 18)
            + mouse.distanceTraveled.ToString().PadLeft(8)
            + cat.distanceTraveled.ToString().PadLeft(7));
        WriteLine("");

        if (caughtAt != -1)
        {
            WriteLine(string.Format("Mouse caught at: {0,2}", caughtAt));
        }
        else
        {
            WriteLine("Mouse evaded Cat");
        }
    }
    private void WriteLine(string text)
    {
        log.Append(text).Append('\n');
    }

    private void SaveLog()
    {
        string folder = Path.GetDirectoryName(Path.GetFullPath(OutFile));
        Directory.CreateDirectory(folder);
        File.WriteAllText(OutFile, log.ToString());
        Console.Write(log.ToString());
    }
}
