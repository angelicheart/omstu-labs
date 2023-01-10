namespace SpaceBattle.Lib;

public class InjectCommand : ICommand
{
    private readonly ICommand Command;
    public InjectCommand(ICommand command) 
    {
        this.Command = command;
    }
    public void Execute()
    {
        Command.Execute();
    }
}