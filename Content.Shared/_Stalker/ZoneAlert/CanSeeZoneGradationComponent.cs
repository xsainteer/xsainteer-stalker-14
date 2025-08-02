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
    public EntityUid ParentGrid;

    [DataField]
    ProtoId<AlertPrototype> ZoneAlert = "ZoneAlert";
}
