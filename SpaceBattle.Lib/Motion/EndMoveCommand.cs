namespace SpaceBattle.Lib;

public class EndMoveCommand : ICommand
{
    private IMoveStopable ObjThatMove;

    public EndMoveCommand(IMoveStopable obj)
    {
        this.ObjThatMove = obj;
    }
    
    public void Execute()
    {
        IoC.Resolve<ICommand>("Game.RemoveProperty", ObjThatMove.obj, "velocity", ObjThatMove.velocity, ObjThatMove.queue).Execute();
        IoC.Resolve<IInjectable>("Game.GetProperty", ObjThatMove.obj, "Move").Inject(IoC.Resolve<ICommand>("Game.EmptyCommand"));
    }
}
