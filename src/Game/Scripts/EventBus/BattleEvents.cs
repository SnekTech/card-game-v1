using System;
using CardGameV1.UI;
using BattleOverPanel = CardGameV1.UI.BattleUIComponents.BattleOverPanel;

namespace CardGameV1.EventBus;

public class BattleEvents
{
    public event Action<string, BattleOverPanel.PanelType>? BattleOverScreenRequested;
    public event Action? Won;

    public void EmitBattleOverScreenRequested(string text, BattleOverPanel.PanelType type)
        => BattleOverScreenRequested?.Invoke(text, type);

    public void EmitWon() => Won?.Invoke();
}