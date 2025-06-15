using System.Collections.Generic;
using CardGameV1.Constants;
using CardGameV1.EventBus;
using Godot;
using GodotUtilities;

namespace CardGameV1.Map;

[Scene]
public partial class MapScene : Node2D
{
    [Node]
    private Node2D lines = null!;
    [Node]
    private Node2D rooms = null!;
    [Node]
    private Node2D visuals = null!;
    [Node]
    private Camera2D camera2D = null!;

    private const int ScrollSpeed = 15;

    private readonly MapGenerator _mapGenerator = new();

    private List<List<Room>> _mapData = [];
    private int _floorsClimbed;
    private Room? _lastRoom;
    private float _cameraEdgeY;

    public override void _Ready()
    {
        _cameraEdgeY = MapGenerator.YDistance * (MapGenerator.Floors - 1);
    }

    public override void _ExitTree()
    {
        foreach (var mapRoom in rooms.GetChildrenOfType<MapRoomScene>())
        {
            mapRoom.Selected -= OnMapRoomSelected;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(InputActions.ScrollUp))
        {
            ScrollCameraY(-ScrollSpeed);
        }
        else if (@event.IsActionPressed(InputActions.ScrollDown))
        {
            ScrollCameraY(ScrollSpeed);
        }

        var clampedY = Mathf.Clamp(camera2D.Position.Y, -_cameraEdgeY, 0);
        camera2D.Position = camera2D.Position with { Y = clampedY };

        return;

        void ScrollCameraY(float deltaY)
        {
            camera2D.Position = camera2D.Position with { Y = camera2D.Position.Y + deltaY };
        }
    }

    public void GenerateNewMap()
    {
        _floorsClimbed = 0;
        _mapData = _mapGenerator.GenerateMap();
        CreateMap();
    }

    private void CreateMap()
    {
        foreach (var floor in _mapData)
        {
            foreach (var room in floor)
            {
                if (room.HasNextRooms || room.IsBoss)
                {
                    SpawnRoom(room);
                }
            }
        }

        const int mapWidthPixels = MapGenerator.XDistance * (MapGenerator.MapWidth - 1);
        var visualsX = (GetViewportRect().Size.X - mapWidthPixels) / 2;
        var visualsY = GetViewportRect().Size.Y / 2;
        visuals.Position = new Vector2(visualsX, visualsY);
    }

    private void SpawnRoom(Room room)
    {
        var newMapRoom = SceneFactory.Instantiate<MapRoomScene>();
        rooms.AddChild(newMapRoom);
        newMapRoom.Room = room;
        newMapRoom.Selected += OnMapRoomSelected;
        ConnectLines(room);

        if (room.IsSelected && room.GridPosition.row < _floorsClimbed)
        {
            newMapRoom.ShowSelected();
        }
    }

    private void ConnectLines(Room room)
    {
        if (room.HasNextRooms == false)
            return;

        foreach (var next in room.NextRooms)
        {
            var newMapLine = SceneFactory.Instantiate<MapLine>();
            newMapLine.AddPoint(room.Position);
            newMapLine.AddPoint(next.Position);
            lines.AddChild(newMapLine);
        }
    }

    private void OnMapRoomSelected(Room room)
    {
        foreach (var mapRoom in rooms.GetChildrenOfType<MapRoomScene>())
        {
            if (mapRoom.Room.GridPosition.row == room.GridPosition.row)
            {
                mapRoom.Available = false;
            }
        }

        _lastRoom = room;
        _floorsClimbed++;
        EventBusOwner.Events.EmitMapExited(room);
    }

    public void UnlockFloor(int floor)
    {
        foreach (var mapRoom in rooms.GetChildrenOfType<MapRoomScene>())
        {
            if (mapRoom.Room.GridPosition.row == floor)
            {
                mapRoom.Available = true;
            }
        }
    }

    public void UnlockNextRooms()
    {
        if (_lastRoom == null)
            return;

        foreach (var mapRoom in rooms.GetChildrenOfType<MapRoomScene>())
        {
            if (_lastRoom.NextRooms.Contains(mapRoom.Room))
            {
                mapRoom.Available = true;
            }
        }
    }

    public void ShowMap()
    {
        Show();
        camera2D.Enabled = true;
    }

    public void HideMap()
    {
        Hide();
        camera2D.Enabled = false;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}