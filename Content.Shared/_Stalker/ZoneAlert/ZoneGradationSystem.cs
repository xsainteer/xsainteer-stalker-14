using Content.Shared.Alert;

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
    }

    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<CanSeeZoneGradationComponent>();
        while(query.MoveNext(out var uid, out var component))
        {
            var grid = Transform(uid).GridUid;

            if(grid == component.ParentGrid)
                continue;

            _alerts.ClearAlert(uid, component.CurrentZoneAlert);

            // If the grid has changed, we need to update the alert.

            if (!TryComp<ZoneGradationComponent>(grid, out var zoneGradationComponent))
                continue;

            _alerts.ShowAlert(uid, zoneGradationComponent.ZoneAlert);

            component.CurrentZoneAlert = zoneGradationComponent.ZoneAlert;
        }
    }
}
