using Content.Shared.Alert;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.ZoneAlert;

/// <summary>
/// Entity with this component can see the zone gradation alert.
/// </summary>
[RegisterComponent]
public sealed partial class CanSeeZoneGradationComponent : Component
{
    /// <summary>
    /// What does the player see when they are in a zone.
    /// </summary>
    public ProtoId<AlertPrototype> ZoneAlert;
}
