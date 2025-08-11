using Content.Server.GameTicking;
using Content.Server.GameTicking.Rules;
using Content.Server.Mind;
using Content.Shared.Damage;
using Content.Shared.GameTicking.Components;
using Content.Shared.Humanoid;
using Content.Shared.Mind;
using Content.Shared.Mobs;
using Robust.Shared.Player;

namespace Content.Server._Stalker.DeathPenalty;

/// <summary>
/// This handles the death penalty system for players.
/// </summary>
public sealed class DeathPenaltySystem : GameRuleSystem<DeathPenaltyComponent>
{
    [Dependency] private readonly GameTicker _gameTicker = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HumanoidAppearanceComponent, MobStateChangedEvent>(OnDeath);
    }

    private void OnDeath(Entity<HumanoidAppearanceComponent> ent, ref MobStateChangedEvent args)
    {
        if (!TryComp<ActorComponent>(ent.Owner, out var mind))
            return;

        if (args.NewMobState != MobState.Dead)
            return;

        var query = EntityQueryEnumerator<DeathPenaltyManagerComponent, GameRuleComponent>();

        while (query.MoveNext(out var uid, out var death, out var rule))
        {
            if (!_gameTicker.IsGameRuleActive(uid, rule))
                continue;

            death.Deaths[mind.PlayerSession.UserId]++;
        }
    }
}
