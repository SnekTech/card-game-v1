using System.Collections.Generic;
using CardGameV1.CustomResources;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.Character.Cards;

[GlobalClass]
public partial class WarriorAxeAttack : Card
{
    protected override void ApplyEffects(IEnumerable<ITarget> targets)
    {
        var damageEffect = new DamageEffect(6);
        damageEffect.Execute(targets);
    }
}