namespace SpaceBattle.Lib;

public class InjectCommand : ICommand, IInjectable
{
    private ICommand Command;
    public InjectCommand(ICommand command) 
    {
        this.Command = command;
    }

    public void Inject(ICommand obj)
    {
        Command = obj;
    }

    public void Execute()
    {
        Command.Execute();
    }
}
