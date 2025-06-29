﻿using System.Threading;
using Godot;

namespace CardGameV1.CardVisual.CardStates;

public class ReleasedState(CardStateMachine cardStateMachine) : CardState(cardStateMachine)
{
    private bool _played;
    
    public override void OnEnter()
    {
        _played = false;

        if (CardUI.Targets.Count > 0)
        {
            EventBus.EventBusOwner.CardEvents.EmitTooltipHideRequested();
            _played = true;
            CardUI.PlayAsync(CancellationToken.None).Fire();
        }
    }

    public override void OnInput(InputEvent inputEvent)
    {
        if (_played)
            return;

        ChangeState<BaseState>();
    }
}