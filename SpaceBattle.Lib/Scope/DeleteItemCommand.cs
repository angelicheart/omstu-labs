using Hwdtech;


namespace SpaceBattle.Lib;

public class DeleteItemCommand : ICommand
{
    string key;
    public DeleteItemCommand(string _key)
    {
        key = _key;
    }
    public void Execute()
    {
        IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Remove(key);
    }
}
