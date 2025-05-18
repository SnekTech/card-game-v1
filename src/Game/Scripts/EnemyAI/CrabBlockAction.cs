using Godot;

namespace CardGameV1.EnemyAI;

public class CrabBlockAction : EnemyChanceBasedAction
{
    public override void PerformAction()
    {
        if (Target == null || Enemy == null)
        {
            GD.Print("target or enemy is null");
            return;
        }

        // todo: implementation
        GD.Print("crab block");
    }
}