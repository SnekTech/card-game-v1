using System.Collections.Generic;
using System.Linq;
using Godot;

namespace CardGameV1.Map;

public class MapGenerator
{
    private const int XDistance = 30;
    private const int YDistance = 25;
    private const int PlacementRandomness = 5;
    private const int Floors = 15;
    private const int MapWidth = 7;
    private const int Paths = 6;
    private const float MonsterRoomWeight = 10.0f;
    private const float ShopRoomWeight = 2.5f;
    private const float CampfireRoomWeight = 4.0f;

    private readonly Dictionary<RoomType, float> _randomRoomWeights = new()
    {
        [RoomType.Monster] = 0,
        [RoomType.Shop] = 0,
        [RoomType.Campfire] = 0
    };

    private float _randomRoomTotalWeight;
    private List<List<Room>> _mapData = [];

    public List<List<Room>> GenerateMap()
    {
        _mapData = GenerateInitialGrid();

        return [];
    }

    private List<List<Room>> GenerateInitialGrid()
    {
        var result = new List<List<Room>>();
        for (var i = 0; i < Floors; i++)
        {
            var adjacentRooms = new List<Room>();
            for (var j = 0; j < MapWidth; j++)
            {
                var offset = new Vector2(GD.Randf(), GD.Randf()) * PlacementRandomness;
                var room = new Room
                {
                    Position = new Vector2(j * XDistance, -i * YDistance) + offset,
                    GridPosition = (i, j),
                };

                var isBossFloor = i == Floors - 1;
                if (isBossFloor)
                {
                    room.Position = room.Position with { Y = (i + 1) * -YDistance };
                }

                adjacentRooms.Add(room);
            }

            result.Add(adjacentRooms);
        }

        return result;
    }
}