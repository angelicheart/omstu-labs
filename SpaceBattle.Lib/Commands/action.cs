public class ActionCommand : ICommand {
    private Action<object[]> action;
    private object[] args;

    public ActionCommand(Action action, params object[] args)
    {
        this.args = args;
        this.action = action;
    }
    
    public void Execute()
    {

    }
}