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
        var startingPoints = GetRandomStartingPoints();

        foreach (var j in startingPoints)
        {
            var currentJ = j;

            for (var i = 0; i < Floors - 1; i++)
            {
                // exclude the boss floor
                currentJ = SetupConnection(i, currentJ);
            }
        }

        for (var i = 0; i < Floors; i++)
        {
            GD.Print($"floor {i}:");
            var floor = _mapData[i];
            var usedRooms = floor.Where(room => room.NextRooms.Count > 0);
            GD.Print($"[{string.Join(", ", usedRooms)}]\n");
        }

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

    private List<int> GetRandomStartingPoints()
    {
        var yCoordinates = new List<int>();
        var uniquePoints = 0;

        while (uniquePoints < 2)
        {
            uniquePoints = 0;
            yCoordinates.Clear();

            for (var i = 0; i < Paths; i++)
            {
                var staringPoint = GD.RandRange(0, MapWidth - 1);
                if (yCoordinates.Contains(staringPoint) == false)
                {
                    uniquePoints++;
                }

                yCoordinates.Add(staringPoint);
            }
        }

        return yCoordinates;
    }

    private int SetupConnection(int i, int j)
    {
        Room? nextRoom = null;
        var currentRoom = _mapData[i][j];

        while (nextRoom == null || WouldCrossExistingPath(i, j, nextRoom))
        {
            var randomJ = Mathf.Clamp(GD.RandRange(j - 1, j + 1), 0, MapWidth - 1);
            nextRoom = _mapData[i + 1][randomJ];
        }

        currentRoom.NextRooms.Add(nextRoom);
        return nextRoom.GridPosition.column;
    }

    private bool WouldCrossExistingPath(int i, int j, Room nextRoom)
    {
        var (nextI, nextJ) = nextRoom.GridPosition;
        var currentRoom = _mapData[i][j];
        var hasCrossed = false;

        // has left neighbor and next room goes left
        if (j > 0 && nextJ < j)
        {
            var leftNeighbor = _mapData[i][j - 1];
            hasCrossed |= leftNeighbor.NextRooms.Any(room => room.GridPosition.column > nextJ);
        }

        // has right neighbor and next room goes right
        if (j < MapWidth - 1 && nextJ > j)
        {
            var rightNeighbor = _mapData[i][j + 1];
            hasCrossed |= rightNeighbor.NextRooms.Any(room => room.GridPosition.column < nextJ);
        }

        return hasCrossed;
    }
}