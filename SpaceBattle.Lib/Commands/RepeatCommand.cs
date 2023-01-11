namespace SpaceBattle.Lib;

public class RepeatCommand : ICommand
{
    private readonly ICommand Command;
    public RepeatCommand(ICommand command)
    {
        this.Command = command;
    }
    public void Execute()
    {
        Command.Execute();
        IoC.Resolve<ICommand>("Game.Queue.Push", Command).Execute();
    }
}
