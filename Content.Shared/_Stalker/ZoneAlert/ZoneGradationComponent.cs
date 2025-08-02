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
    /// <summary>
    /// What gradation this zone is set to.
    /// </summary>
    public ZoneGradation ZoneGradation = ZoneGradation.GreenZone;
}

public enum ZoneGradation : short
{
    GreenZone,
    BaseZone,
    YellowZone,
    RedZone,
    BlackZone,
}
