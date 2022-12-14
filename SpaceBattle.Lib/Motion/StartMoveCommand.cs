namespace SpaceBattle.Lib;

public class StartMoveCommand : ICommand
{
    private IMoveStartable ObjThatMove;

    public StartMoveCommand(IMoveStartable obj)
    {
        this.ObjThatMove = obj;
    }

    public void Execute()
    {
        IoC.Resolve<ICommand>("Game.SetProperty", ObjThatMove.obj, "Velocity", ObjThatMove.velocity);
        ICommand command = IoC.Resolve<ICommand>("Game.Event.Move", ObjThatMove.obj);
        IoC.Resolve<ICommand>("Game.SetProperty", ObjThatMove.obj, "Move", command).Execute();
        IoC.Resolve<ICommand>("Game.Queue.Push", IoC.Resolve<Queue<Hwdtech.ICommand>>("Game.Queue"), command).Execute();
    }
}
