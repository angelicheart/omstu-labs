namespace SpaceBattle.Lib.Test;

public class TreeCreateTest
{
    public TreeCreateTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var TreeStrategy = new TreeCreateStrategy();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Tree.Build", (object[] args) => (TreeStrategy.Execute(args))).Execute();
    }
}