namespace Content.Server._Stalker.DeathPenalty;

/// <summary>
/// Entity with this component is a target for the death penalty system.
/// </summary>
[RegisterComponent]
public sealed partial class DeathPenaltyTargetComponent : Component
{
    /// <summary>
    /// times a playuh died
    /// </summary>
    public uint DeathStacks = 0;

    public TimeSpan NextStackResetTime = TimeSpan.Zero;
}
