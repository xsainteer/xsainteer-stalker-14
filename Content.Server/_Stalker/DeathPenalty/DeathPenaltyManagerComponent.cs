using Content.Shared.FixedPoint;
using Robust.Shared.Network;

namespace Content.Server._Stalker.DeathPenalty;

/// <summary>
/// Gamerule that applies a death penalty to players when they die.
/// </summary>
[RegisterComponent]
public sealed partial class DeathPenaltyManagerComponent : Component
{
    /// <summary>
    /// times a player has died, mapped to their NetUserId.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Dictionary<NetUserId, uint> Deaths = new();
}
