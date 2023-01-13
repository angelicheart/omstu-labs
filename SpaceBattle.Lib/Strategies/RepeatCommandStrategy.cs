namespace SpaceBattle.Lib;

public class RepeatCommandStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        string operation = (string) args[0];
        IUObject obj = (IUObject) args[1];

        var mcd = IoC.Resolve<ICommand>("Game.Commands.MacroCommand", operation, obj);
        var repeat = IoC.Resolve<ICommand>("Game.Commands.RepeatCommand", mcd);
        var inject = IoC.Resolve<ICommand>("Game.Commands.InjectCommand", repeat);

        return inject;
    }
}
