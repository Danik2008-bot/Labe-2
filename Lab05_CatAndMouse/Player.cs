using System;

enum State
{
    Winner,
    Loser,
    Playing,
    NotInGame
}

class Player
{
    public string name;                   
    public int location;                   
    public State state = State.NotInGame;  
    public int distanceTraveled = 0;       

    private readonly int fieldSize;        

    public Player(string name, int fieldSize)
    {
        this.name = name;
        this.fieldSize = fieldSize;
        this.location = -1;
    }
    public void PlaceAt(int cell)
    {
        location = cell;
        state = State.Playing;
    }
    public void Move(int steps)
    {
        location = ((location - 1 + steps) % fieldSize + fieldSize) % fieldSize + 1;
        distanceTraveled += Math.Abs(steps);
    }
}
