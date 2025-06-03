using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using GTweensGodot.Extensions;
using Timer = Godot.Timer;

namespace CardGameV1;

public static class CustomExtensions
{
    public static async void Fire(this Task task, Action? onComplete = null, Action<Exception>? onError = null)
    {
        try
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                GD.Print("something wrong during fire & forget: ");
                GD.Print(e);
                onError?.Invoke(e);
            }

            onComplete?.Invoke();
        }
        catch (Exception e)
        {
            GD.Print("something wrong on fire & forget complete : ");
            GD.Print(e);
            onError?.Invoke(e);
        }
    }

    public static SceneTreeTimer CreateSceneTreeTimer(this Node node, double timeSec, bool processAlways = true,
        bool processInPhysics = false, bool ignoreTimeScale = false)
    {
        var timer = node.GetTree().CreateTimer(timeSec, processAlways, processInPhysics, ignoreTimeScale);
        return timer;
    }

    public static async Task DelayGd(this Node node, double timeSec, bool processAlways = true,
        bool processInPhysics = false, bool ignoreTimeScale = false)
    {
        var timer = node.CreateSceneTreeTimer(timeSec, processAlways, processInPhysics, ignoreTimeScale);
        await timer.ToSignal(timer, Timer.SignalName.Timeout);
    }

    public static void SetModulateAlpha(this CanvasItem canvasItem, float alpha)
    {
        canvasItem.Modulate = canvasItem.Modulate with { A = alpha };
    }

    public static async Task ShakeAsync(this Node2D node2D, float strength, float duration = 0.2f)
    {
        var originalPosition = node2D.Position;
        const int shakeCount = 10;

        for (var i = 0; i < shakeCount; i++)
        {
            var offset = GetRandomOffset();
            var targetPosition = originalPosition + offset * strength;
            if (i % 2 == 0)
            {
                targetPosition = originalPosition;
            }

            await node2D.TweenPosition(targetPosition, duration / shakeCount).PlayAsync(CancellationToken.None);
            strength *= 0.75f;
        }

        node2D.Position = originalPosition;

        return;

        Vector2 GetRandomOffset()
        {
            return new Vector2(GD.Randf() * 2 - 1, GD.Randf() * 2 - 1);
        }
    }

    public static bool HasAnyChild(this Node node)
    {
        return node.GetChildCount() > 0;
    }
}