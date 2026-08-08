using System;
using UnityEngine;

public class Player
{
    public string Id;
    public string Name;
    public GameColor Color;
    public bool IsMadeFirstTurn;
    public bool IsLost;

    public Player(string name, GameColor color)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Color = color;
        IsMadeFirstTurn = false;
        IsLost = false;
    }
}
