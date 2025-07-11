using CardGameV1.CustomResources;

namespace CardGameV1.Map;

public class MapGenerator
{
    public const int XDistance = 30;
    public const int YDistance = 25;
    private const int PlacementRandomness = 5;
    public const int Floors = 15;
    public const int MapWidth = 7;
    private const int Paths = 6;

    public const int BossFloor = Floors - 1;
    public const int BossColumn = MapWidth / 2;
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
    private readonly BattleStatsPool _battleStatsPool = BattleStatsPool.DefaultPool;

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

                if (i == BossFloor)
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
        var bossRoom = _mapData[BossFloor][BossColumn];
        ConnectToBoss();
        bossRoom.Type = RoomType.Boss;
        bossRoom.BattleStats = _battleStatsPool.GetRandomBattleForTier(2);
        return;

        void ConnectToBoss()
        {
            foreach (var room in _mapData[BossFloor - 1])
            {
                if (room.HasNextRooms == false)
                    continue;

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
        // first floor is always a battle
        foreach (var room in _mapData[FirstFloor].Where(room => room.HasNextRooms))
        {
            room.Type = RoomType.Monster;
            room.BattleStats = _battleStatsPool.GetRandomBattleForTier(0);
        }

        foreach (var room in _mapData[TreasureFloor].Where(room => room.HasNextRooms))
        {
            room.Type = RoomType.Treasure;
        }

        // last floor before boss is campfire
        foreach (var room in _mapData[BossFloor - 1].Where(room => room.HasNextRooms))
        {
            room.Type = RoomType.Campfire;
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
        while (campfireBelow4 || consecutiveCampfire || consecutiveShop || campfireBeforeBossCampfire)
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
        if (typeCandidate == RoomType.Monster)
        {
            room.BattleStats = _battleStatsPool.GetRandomBattleForTier(room.BattleTier);
        }

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