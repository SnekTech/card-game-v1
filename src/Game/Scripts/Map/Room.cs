using System.Collections.Generic;
using CardGameV1.CustomResources;
using Godot;

namespace CardGameV1.Map;

public class Room
{
    public RoomType Type { get; set; }
    public (int row, int column) GridPosition { get; init; } = (0, 0);
    public Vector2 Position { get; set; }
    public List<Room> NextRooms { get; } = [];
    public bool HasNextRooms => NextRooms.Count > 0;
    public bool IsBoss => GridPosition == (MapGenerator.BossFloor, MapGenerator.BossColumn);
    public bool IsSelected { get; set; }
    // only used by monster and boss room
    public BattleStats? BattleStats { get; set; }
    public int BattleTier => GridPosition.row <= 2 ? 0 : 1;

    public override string ToString()
    {
        return $"{GridPosition.column} ({Type.ToString()[0]})";
    }
}