using Content.Server.GameTicking.Rules;
using Content.Server.Mind;
using Content.Shared.GameTicking.Components;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Systems;
using Content.Shared.Movement.Systems;
using Robust.Server.Player;
using Robust.Shared.Network;
using Robust.Shared.Player;

namespace Content.Server._Stalker.DeathPenalty;

/// <summary>
/// This handles the death penalty system for players.
/// </summary>
public sealed class DeathPenaltySystem : GameRuleSystem<DeathPenaltyComponent>
{
    [Dependency] private readonly MobThresholdSystem _thresholds = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _speedModifier = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly MindSystem _mind = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DeathPenaltyTargetComponent, MobStateChangedEvent>(OnDeath);
        SubscribeLocalEvent<DeathPenaltyTargetComponent, ComponentStartup>(OnComponentStartup);
        SubscribeLocalEvent<DeathPenaltyTargetComponent, RefreshMovementSpeedModifiersEvent>(OnRefresh);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<DeathPenaltyTargetComponent>();
        while (query.MoveNext(out var uid, out var player))
        {
            if(player.NextStackResetTime <= Timing.CurTime)
                continue;

            SetPenalty((uid, player));
        }
    }

    private void OnComponentStartup(Entity<DeathPenaltyTargetComponent> ent, ref ComponentStartup args)
    {
        // getting the original thresholds for the entity so we can subtract correctly percentage-wise.
        ent.Comp.OriginalCriticalThreshold = _thresholds.GetThresholdForState(ent.Owner, MobState.Critical);
        ent.Comp.OriginalDeadThreshold = _thresholds.GetThresholdForState(ent.Owner, MobState.Dead);

        SetPenalty(ent);
    }

    /// <summary>
    /// sets the death penalty for the entity according to DeathPenaltyManager.
    /// </summary>
    private void SetPenalty(Entity<DeathPenaltyTargetComponent> ent)
    {
        if (!TryComp<ActorComponent>(ent.Owner, out var actor))
            return;

        if (!TryGetGameRuleEntity(out var death, out var penalty))
            return;

        var stacks = death.Deaths[actor.PlayerSession.UserId];

        // health penalty start
        var totalHealthModifier = stacks * penalty.HealthModifier;

        var newCritical = ent.Comp.OriginalCriticalThreshold * (1 - totalHealthModifier);
        var newDead = ent.Comp.OriginalDeadThreshold * (1 - totalHealthModifier);

        _thresholds.SetMobStateThreshold(ent.Owner, newCritical, MobState.Critical);
        _thresholds.SetMobStateThreshold(ent.Owner, newDead, MobState.Dead);
        // health penalty end

        // speed penalty start
        _speedModifier.RefreshMovementSpeedModifiers(ent.Owner);
        // speed penalty end

        ent.Comp.NextStackResetTime = penalty.StackResetTime + Timing.CurTime;
    }

    /// <summary>
    /// just increments the death count for the player when they die.
    /// </summary>
    private void OnDeath(Entity<DeathPenaltyTargetComponent> ent, ref MobStateChangedEvent args)
    {
        if (!TryComp<ActorComponent>(ent.Owner, out var mind))
            return;

        if (args.NewMobState != MobState.Dead)
            return;

        if(!TryGetGameRuleEntity(out var death, out var penalty))
            return;

        // if the player has reached the max death stacks, do not increment.
        if(death.Deaths[mind.PlayerSession.UserId] >= penalty.MaxDeathStacks)
            return;

        death.Deaths[mind.PlayerSession.UserId]++;
    }

    /// <summary>
    /// sets the movement speed modifier for the entity according to DeathPenaltyManager.
    /// </summary>
    private void OnRefresh(Entity<DeathPenaltyTargetComponent> ent, ref RefreshMovementSpeedModifiersEvent args)
    {
        if (!TryComp<ActorComponent>(ent.Owner, out var actor))
            return;

        if (!TryGetGameRuleEntity(out var death, out var penalty))
            return;

        var stacks = death.Deaths[actor.PlayerSession.UserId];

        var modifier = 1 - stacks * penalty.MoveSpeedModifier;

        args.ModifySpeed(modifier);
    }

    /// <summary>
    /// Gets the game rule entity that manages the death penalty system.
    /// </summary>
    public bool TryGetGameRuleEntity(
        out DeathPenaltyManagerComponent death,
        out DeathPenaltyComponent penalty)
    {
        death = default!;
        penalty = default!;

        var query = EntityQueryEnumerator<DeathPenaltyManagerComponent, DeathPenaltyComponent, GameRuleComponent>();
        while (query.MoveNext(out var uid, out death!, out penalty!, out var rule))
        {
            return !GameTicker.IsGameRuleActive(uid, rule);
        }

        return false;
    }

    public bool TryGetDeathStacks(string ckey, out uint stacks, DeathPenaltyTargetComponent? target = null)
    {
        stacks = 0;

        if(!_player.TryGetUserId(ckey, out var userId))
            return false;

        if (!_player.TryGetSessionById(userId, out var session))
            return false;

        var uid = session.AttachedEntity ?? default;

        if (!TryComp<DeathPenaltyTargetComponent>(uid, out var comp))
            return false;

        if (!TryGetGameRuleEntity(out var death, out var penalty))
            return false;

        if (death.Deaths.TryGetValue(userId, out stacks))
        {
            if (stacks > penalty.MaxDeathStacks)
                stacks = penalty.MaxDeathStacks;

            SetPenalty((uid, comp));
            return true;
        }

        stacks = 0;
        return false;
    }

    public void SetDeathStacks(string ckey, uint stacks)
    {
        if(!_player.TryGetUserId(ckey, out var userId))
            return;

        if (!_player.TryGetSessionById(userId, out var session))
            return;

        var uid = session.AttachedEntity ?? default;

        if (!TryComp<DeathPenaltyTargetComponent>(uid, out var comp))
            return;

        if (!TryGetGameRuleEntity(out var death, out var penalty))
            return;

        if (stacks > penalty.MaxDeathStacks)
            stacks = penalty.MaxDeathStacks;

        death.Deaths[userId] = stacks;

        SetPenalty((uid, comp));
    }
}
