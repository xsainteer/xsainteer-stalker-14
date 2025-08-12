namespace Content.Server._Stalker.DeathPenalty;

/// <summary>
/// Component that defines the death penalty system for players.
/// </summary>
[RegisterComponent]
public sealed partial class DeathPenaltyComponent : Component
{
    /// <summary>
    /// max amount of death stacks a player can have
    /// </summary>
    [DataField]
    public uint MaxDeathStacks = 10;

    /// <summary>
    /// Time it takes for a stack to reset
    /// </summary>
    [DataField]
    public TimeSpan StackResetTime = TimeSpan.FromMinutes(30);

    [DataField]
    public float MoveSpeedModifier = 0.5f;

    [DataField]
    public float HealthModifier = 0.5f;
}
