using Content.Shared.FixedPoint;

namespace Content.Server._Stalker.DeathPenalty;

/// <summary>
/// Entity with this component is a target for the death penalty system.
/// </summary>
[RegisterComponent]
public sealed partial class DeathPenaltyTargetComponent : Component
{
    /// <summary>
    /// timing when a stack will reset.
    /// </summary>
    public TimeSpan NextStackResetTime = TimeSpan.Zero;

    /// <summary>
    /// critical threshold the entity had before the death penalty was applied.
    /// </summary>
    public FixedPoint2 OriginalCriticalThreshold = FixedPoint2.New(100.0f);

    /// <summary>
    /// dead threshold the entity had before the death penalty was applied.
    /// </summary>
    public FixedPoint2 OriginalDeadThreshold = FixedPoint2.New(200.0f);
}
