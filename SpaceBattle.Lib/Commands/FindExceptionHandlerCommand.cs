namespace SpaceBattle.Lib;

public class FindExceptionCommand : ICommand
{
    private readonly ICommand Command;
    
    public FindExceptionCommand(ICommand command)
    {
        this.Command = command;
    }

    public void Execute()
    {
        if (IoC.Resolve<bool>("Game.CatchesExceptions"))
        {
            try
            {
                this.Command.Execute();
            }

            catch (Exception Exception)
            {
                IoC.Resolve<IStrategy>("Game.Exception.FindHandlerWithTree", Command, Exception).Execute();
            }
        }
        else throw new Exception("GAME ISNT CATCHES EXCEPTIONS");
    }
}
