namespace SpaceBattle.Lib;

public class FindExceptionCommand : ICommand
{
    private readonly Queue<ICommand> Commands;
    public ICommand command;
    
    public FindExceptionCommand(Queue<ICommand> commands)
    {
        this.Commands = commands;
    }

    public void Execute()
    {
        while (IoC.Resolve<bool>("Game.CatchesExceptions"))
        {
            try
            {
                this.command = this.Commands.Dequeue();
            }
            catch
            {
                break;
            }

            try
            {
                this.command.Execute();
            }

            catch (Exception Exception)
            {
                IoC.Resolve<IStrategy>("Game.Exception.FindHandlerWithTree", this.command.GetHashCode(), Exception.GetHashCode()).Execute();
            }
        }
    }
}
