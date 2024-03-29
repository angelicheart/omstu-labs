namespace SpaceBattle.Lib;

public class DeleteGameCommand : ICommand
{
    readonly string id;

    public DeleteGameCommand(string id)
    {
        this.id = id;
    }
    
    public void Execute()
    {
        IoC.Resolve<ICommand>("Scope.Delete", id).Execute();
    }
}
