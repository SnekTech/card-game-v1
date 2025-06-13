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

    private const int BossFloor = Floors - 1;
    private const int TreasureFloor = Floors / 2;
    private const int FirstFloor = 0;

    private const float MonsterRoomWeight = 10.0f;
    private const float CampfireRoomWeight = 4.0f;
    private const float ShopRoomWeight = 2.5f;

    private readonly Dictionary<RoomType, float> _randomRoomWeights = new()
    {
        [RoomType.Monster] = 0,
        [RoomType.Campfire] = 0,
        [RoomType.Shop] = 0
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

        SetupBossRoom();
        SetupRandomRoomWeights();
        SetupRoomTypes();

        #region test map generation

        // todo: remove test
        for (var i = 0; i < Floors; i++)
        {
            GD.Print($"floor {i}:");
            var floor = _mapData[i];
            var usedRooms = floor.Where(room => room.NextRooms.Count > 0);
            GD.Print($"[{string.Join(", ", usedRooms)}]");
        }

        #endregion

        return _mapData;
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
        var (_, nextJ) = nextRoom.GridPosition;
        var hasCrossed = false;

        var hasLeftNeighbor = j > 0;
        var nextRoomGoesLeft = nextJ < j;
        if (hasLeftNeighbor && nextRoomGoesLeft)
        {
            var leftNeighbor = _mapData[i][j - 1];
            hasCrossed |= leftNeighbor.NextRooms.Any(room => room.GridPosition.column > nextJ);
        }

        var hasRightNeighbor = j < MapWidth - 1;
        var nextRoomGoesRight = nextJ > j;
        if (hasRightNeighbor && nextRoomGoesRight)
        {
            var rightNeighbor = _mapData[i][j + 1];
            hasCrossed |= rightNeighbor.NextRooms.Any(room => room.GridPosition.column < nextJ);
        }

        return hasCrossed;
    }

    private void SetupBossRoom()
    {
        var middle = Mathf.FloorToInt(MapWidth * 0.5);
        var bossRoom = _mapData[Floors - 1][middle];
        ConnectToBoss();
        bossRoom.Type = RoomType.Boss;
        return;

        void ConnectToBoss()
        {
            foreach (var room in _mapData[BossFloor - 1])
            {
                room.NextRooms.Clear();
                room.NextRooms.Add(bossRoom);
            }
        }
    }

    private void SetupRandomRoomWeights()
    {
        _randomRoomWeights[RoomType.Monster] = MonsterRoomWeight;
        _randomRoomWeights[RoomType.Campfire] = MonsterRoomWeight + CampfireRoomWeight;
        _randomRoomWeights[RoomType.Shop] = MonsterRoomWeight + CampfireRoomWeight + ShopRoomWeight;

        _randomRoomTotalWeight = _randomRoomWeights[RoomType.Shop];
    }

    private void SetupRoomTypes()
    {
        foreach (var room in _mapData[FirstFloor])
        {
            if (room.IsOnPath)
            {
                room.Type = RoomType.Monster;
            }
        }

        foreach (var room in _mapData[TreasureFloor])
        {
            if (room.IsOnPath)
            {
                room.Type = RoomType.Treasure;
            }
        }

        // last floor before boss is campfire
        foreach (var room in _mapData[BossFloor - 1])
        {
            if (room.IsOnPath)
            {
                room.Type = RoomType.Campfire;
            }
        }

        foreach (var floor in _mapData)
        {
            foreach (var room in floor)
            {
                if (room.Type == RoomType.NotAssigned)
                {
                    SetRoomRandomly(room);
                }
            }
        }
    }

    private void SetRoomRandomly(Room room)
    {
        var (row, _) = room.GridPosition;
        var campfireBelow4 = true;
        var consecutiveCampfire = true;
        var consecutiveShop = true;
        var campfireBeforeBossCampfire = true;

        var typeCandidate = RoomType.NotAssigned;
        while (campfireBelow4 && consecutiveCampfire && consecutiveShop && campfireBeforeBossCampfire)
        {
            typeCandidate = GetRandomRoomTypeByWeight();

            var isCampfire = typeCandidate == RoomType.Campfire;
            var hasCampfireParent = RoomHasParentOfType(room, RoomType.Campfire);
            var isShop = typeCandidate == RoomType.Shop;
            var hasShopParent = RoomHasParentOfType(room, RoomType.Shop);

            campfireBelow4 = isCampfire && row < 3;
            consecutiveCampfire = isCampfire && hasCampfireParent;
            consecutiveShop = isShop && hasShopParent;
            campfireBeforeBossCampfire = isCampfire && row == BossFloor - 2;
        }

        room.Type = typeCandidate;

        return;

        RoomType GetRandomRoomTypeByWeight()
        {
            var roll = GD.RandRange(0, _randomRoomTotalWeight);
            foreach (var (type, weight) in _randomRoomWeights)
            {
                if (weight > roll)
                {
                    return type;
                }
            }

            return RoomType.Monster;
        }
    }

    private bool RoomHasParentOfType(Room room, RoomType type)
    {
        var (i, j) = room.GridPosition;
        if (i == 0)
            return false;
        var parents = new List<Room>();

        var middleParentCandidate = _mapData[i - 1][j];
        CheckAndAddParentCandidate(middleParentCandidate);

        if (j > 0)
        {
            CheckAndAddParentCandidate(_mapData[i - 1][j - 1]);
        }

        if (j < MapWidth - 1)
        {
            CheckAndAddParentCandidate(_mapData[i - 1][j + 1]);
        }

        return parents.Any(parent => parent.Type == type);

        void CheckAndAddParentCandidate(Room parentCandidate)
        {
            if (parentCandidate.NextRooms.Contains(room))
            {
                parents.Add(parentCandidate);
            }
        }
    }
}