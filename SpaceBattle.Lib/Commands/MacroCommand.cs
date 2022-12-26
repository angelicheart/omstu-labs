namespace SpaceBattle.Lib;

public class MacroCommand : ICommand
{
    private readonly List<ICommand> Commands;
    
    public MacroCommand(List<ICommand> commands)
    {
        this.Commands = commands;
    }

    public void Execute()
    {
        Commands.ForEach(cmd => cmd.Execute());
    }
}
