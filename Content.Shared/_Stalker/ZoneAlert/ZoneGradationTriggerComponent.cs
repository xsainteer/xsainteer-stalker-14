using Content.Shared.Alert;
using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.ZoneAlert;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class ZoneGradationTriggerComponent : Component
{
    /// <summary>
    /// What gradation this trigger is set to.
    /// </summary>
    [DataField]
    public ProtoId<AlertPrototype> ZoneGradation;
}
