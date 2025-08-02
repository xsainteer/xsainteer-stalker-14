using Content.Shared.Alert;
using Robust.Shared.Physics.Events;

namespace Content.Shared._Stalker.ZoneAlert;

/// <summary>
/// This handles...
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

        if (!canSeeZoneGradation.IsInTriggerZone)
        {
            canSeeZoneGradation.IsInTriggerZone = true;
            _alerts.ClearAlert(args.OtherEntity, canSeeZoneGradation.ZoneAlert);
        }
        else
        {
            canSeeZoneGradation.IsInTriggerZone = false;
            _alerts.ShowAlert(args.OtherEntity, canSeeZoneGradation.ZoneAlert, (short)ent.Comp.ZoneGradation);
        }
    }

    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<CanSeeZoneGradationComponent>();
        while(query.MoveNext(out var uid, out var component))
        {
            var grid = Transform(uid).GridUid;

            if(grid == component.ParentGrid)
                continue;

            _alerts.ClearAlert(uid, component.ZoneAlert);

            // If the grid has changed, we need to update the alert.

            if (!TryComp<ZoneGradationComponent>(grid, out var zoneGradationComponent))
                continue;

            _alerts.ShowAlert(uid, component.ZoneAlert, (short)zoneGradationComponent.ZoneGradation);
        }
    }
}
