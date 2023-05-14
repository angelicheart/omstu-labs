namespace SpaceBattle.Lib;

public class SoftStopThreadStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        string id = (string)args[0];

        ActionCommand action_after_stop;

        if (args.Length == 2) {
            action_after_stop = (ActionCommand) args[1];
        }
        else {
            action_after_stop = new ActionCommand(() => new EmptyCommand().Execute());
        }

        return new SoftStopThreadCommand(id, action_after_stop);
    }
}
