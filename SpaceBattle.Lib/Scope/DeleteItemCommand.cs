namespace SpaceBattle.Lib;

public class DeleteItemCommand : ICommand
{
    readonly string id;
    public DeleteItemCommand(string id)
    {
        this.id = id;
    }
    public void Execute()
    {
        IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Remove(id);
    }
}
