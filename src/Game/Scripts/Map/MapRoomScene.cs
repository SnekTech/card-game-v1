using System;
using System.Collections.Generic;
using CardGameV1.Constants;
using Godot;
using GodotUtilities;

namespace CardGameV1.Map;

[Scene]
public partial class MapRoomScene : Area2D
{
    public event Action<Room>? Selected;

    [Node]
    private Sprite2D sprite2D = null!;
    [Node]
    private Line2D line2D = null!;
    [Node]
    private AnimationPlayer animationPlayer = null!;

    private static readonly StringName HighlightAnimation = "highlight";
    private static readonly StringName SelectAnimation = "select";
    private static readonly StringName ResetAnimation = "RESET";

    private readonly Dictionary<RoomType, (Texture2D? icon, Vector2 scale)> _icons = new()
    {
        [RoomType.NotAssigned] = (null, Vector2.One),
        [RoomType.Monster] = (GD.Load<Texture2D>("res://art/tile_0103.png"), Vector2.One),
        [RoomType.Treasure] = (GD.Load<Texture2D>("res://art/tile_0089.png"), Vector2.One),
        [RoomType.Campfire] = (GD.Load<Texture2D>("res://art/player_heart.png"), Vector2.One * 0.6f),
        [RoomType.Shop] = (GD.Load<Texture2D>("res://art/gold.png"), Vector2.One * 0.6f),
        [RoomType.Boss] = (GD.Load<Texture2D>("res://art/tile_0105.png"), Vector2.One * 1.25f),
    };

    private bool _available;
    private Room _room = null!;

    public bool Available
    {
        set
        {
            _available = value;
            if (_available)
            {
                animationPlayer.Play(HighlightAnimation);
            }
            else if (_room.IsSelected == false)
            {
                animationPlayer.Play(ResetAnimation);
            }
        }
    }

    public Room Room
    {
        get => _room;
        set
        {
            _room = value;
            Position = _room.Position;
            line2D.RotationDegrees = GD.RandRange(0, 360);
            var (icon, scale) = _icons[_room.Type];
            sprite2D.Texture = icon;
            sprite2D.Scale = scale;
        }
    }

    public void ShowSelected()
    {
        line2D.Modulate = Colors.White;
    }

    public override void _InputEvent(Viewport viewport, InputEvent inputEvent, int shapeIdx)
    {
        if (_available == false || inputEvent.IsActionPressed(InputActions.LeftMouse) == false)
            return;

        _room.IsSelected = true;
        animationPlayer.Play(SelectAnimation);
    }

    private void OnSelectAnimationFinished(StringName animationName)
    {
        if (animationName == SelectAnimation)
        {
            Selected?.Invoke(_room);
        }
    }

    public override void _EnterTree()
    {
        animationPlayer.AnimationFinished += OnSelectAnimationFinished;
    }

    public override void _ExitTree()
    {
        animationPlayer.AnimationFinished -= OnSelectAnimationFinished;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}