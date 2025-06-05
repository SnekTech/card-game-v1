using Godot;

namespace CardGameV1.MyExtensions;

public static class NodeExtensions
{
    public static void ClearChildren(this Node node)
    {
        foreach (var child in node.GetChildren())
        {
            child.QueueFree();
        }
    }

    public static bool HasAnyChild(this Node node)
    {
        return node.GetChildCount() > 0;
    }
}