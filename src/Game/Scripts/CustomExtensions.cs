using System;
using System.Threading.Tasks;
using Godot;

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
}