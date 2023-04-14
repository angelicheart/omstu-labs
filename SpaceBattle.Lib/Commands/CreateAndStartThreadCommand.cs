namespace SpaceBattle.Lib;

public class CreateAndStartThreadCommand : ICommand
{
    private readonly List<ICommand> Commands;
    
    public CreateAndStartThreadCommand(List<ICommand> commands)
    {
        this.Commands = commands;
    }

    public void Execute()
    {
        Commands.ForEach(cmd => cmd.Execute());
    }
}