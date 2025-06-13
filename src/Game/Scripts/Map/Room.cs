using System.Collections.Generic;
using Godot;

namespace CardGameV1.Map;

public class Room
{
    public RoomType Type { get; set; }
    public (int row, int column) GridPosition { get; set; } = (0, 0);
    public Vector2 Position { get; set; }
    public List<Room> NextRooms { get; } = [];
    public bool IsSelected { get; set; }

    public override string ToString()
    {
        return $"{GridPosition.column} ({Type.ToString()[0]})";
    }
}