using Content.Shared.Alert;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.ZoneAlert;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ZoneGradationComponent : Component
{
    [DataField]
    public ProtoId<AlertPrototype> ZoneAlert = "ZoneAlert";
}
