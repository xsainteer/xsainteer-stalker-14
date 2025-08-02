using Robust.Shared.GameStates;

namespace Content.Shared._Stalker.ZoneAlert;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ZoneGradationTriggerComponent : Component
{
    /// <summary>
    /// What gradation this trigger is set to.
    /// </summary>
    public ZoneGradation ZoneGradation = ZoneGradation.GreenZone;
}

public enum ZoneGradation : short
{
    GreenZone,
    GroupBase,
    YellowZone,
    RedZone,
    BlackZone,
}
