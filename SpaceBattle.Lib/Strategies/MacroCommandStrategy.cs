namespace SpaceBattle.Lib;

public class MacroCommandStrategy : IStrategy
{
    public object Execute(object[] args)
    {   
        List<ICommand> Commands = new List<ICommand>();

        string DepStratReturnsList = (string) args[0];
        IUObject obj = (IUObject) args[1];

        List<string> DependenciesList = IoC.Resolve<List<string>>(DepStratReturnsList);

        DependenciesList.ForEach(dep => Commands.Add(IoC.Resolve<ICommand>(dep, obj)));
        
        return new MacroCommand(Commands);
    }
}
