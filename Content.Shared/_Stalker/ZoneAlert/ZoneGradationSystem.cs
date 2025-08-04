using Content.Shared.Alert;
using Robust.Shared.Physics.Events;

namespace Content.Shared._Stalker.ZoneAlert;

/// <summary>
/// This handles Zone Gradation Alert System.
/// </summary>
public sealed class ZoneGradationSystem : EntitySystem
{
    [Dependency] private readonly AlertsSystem _alerts = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<ZoneGradationTriggerComponent, StartCollideEvent>(OnCollide);
    }

    private void OnCollide(Entity<ZoneGradationTriggerComponent> ent, ref StartCollideEvent args)
    {
        if (!TryComp<CanSeeZoneGradationComponent>(args.OtherEntity, out var canSeeZoneGradation))
            return;

        if (ent.Comp.ZoneGradation == canSeeZoneGradation.ZoneAlert)
            return;

        if(!string.IsNullOrWhiteSpace(canSeeZoneGradation.ZoneAlert))
            _alerts.ClearAlert(args.OtherEntity, canSeeZoneGradation.ZoneAlert);

        _alerts.ShowAlert(args.OtherEntity, ent.Comp.ZoneGradation);

        canSeeZoneGradation.ZoneAlert = ent.Comp.ZoneGradation;
    }
}
