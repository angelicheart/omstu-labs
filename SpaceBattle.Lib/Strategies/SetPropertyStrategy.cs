namespace SpaceBattle.Lib;

public class SetPropertyStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var command = new SetProperty((IUObject)args[0], (string)args[1], (object)args[2]);
        return command;
    }
}

public class SetProperty : ICommand
{
    private IUObject obj;
    private string key;
    private object value;
    public SetProperty(IUObject obj, string key, object value)
    {   
        this.obj = obj;
        this.key = key;
        this.value = value;
    }
    public void Execute()
    {
        obj.setProperty(key, value);
    }
}
