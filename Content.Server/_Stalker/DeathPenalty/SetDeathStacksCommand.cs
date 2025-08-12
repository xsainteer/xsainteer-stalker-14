using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server._Stalker.DeathPenalty;

[AdminCommand(AdminFlags.Admin)]
public sealed class SetDeathStacksCommand : IConsoleCommand
{
    [Dependency] private readonly DeathPenaltySystem _penaltySystem = default!;

    public string Command => "setdeathstacks";
    public string Description => "Sets the death penalty stacks for a player.";
    public string Help => $"Usage: {Command} <CKey> <StacksAmount>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length is < 2 or > 2)
        {
            shell.WriteError(Loc.GetString("shell-need-between-arguments",("lower", 2), ("upper", 2)));
            return;
        }

        if (uint.TryParse(args[1], out var stacksAmount))
            return;

        _penaltySystem.SetDeathStacks(args[0], stacksAmount);
    }
}

[AdminCommand(AdminFlags.Admin)]
public sealed class GetDeathStacksCommand : IConsoleCommand
{
    [Dependency] private readonly DeathPenaltySystem _penaltySystem = default!;

    public string Command => "getdeathstacks";
    public string Description => "Gets the death stacks of an entity.";
    public string Help => $"Usage: {Command} <CKey>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length is < 1 or > 1)
        {
            shell.WriteError(Loc.GetString("shell-need-between-arguments",("lower", 1), ("upper", 1)));
            return;
        }

        _penaltySystem.TryGetDeathStacks(args[0], out var stacks);

        shell.WriteLine(stacks.ToString());
    }
}
