using Content.Shared.Alert;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.ZoneAlert;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class CanSeeZoneGradationComponent : Component
{
    /// <summary>
    /// What grid the entity is currently on.
    /// </summary>
    public EntityUid? ParentGrid;

    /// <summary>
    /// What does the player see when they are in a zone.
    /// </summary>
    public ProtoId<AlertPrototype> ZoneAlert = "ZoneAlert";

    /// <summary>
    /// Whether the entity is currently in a trigger zone for the zone gradation alert.
    /// </summary>
    public bool IsInTriggerZone;
}
