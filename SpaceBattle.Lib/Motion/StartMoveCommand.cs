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
        IoC.Resolve<ICommand>("Game.SetProperty", ObjThatMove.obj, "velocity", ObjThatMove.velocity).Execute();

        var MovableAdapter = IoC.Resolve<IMovable>("Game.Adapters.Movable", ObjThatMove.obj);
        var MoveCommand = IoC.Resolve<ICommand>("Game.Event.Move", MovableAdapter);

        IoC.Resolve<ICommand>("Game.SetProperty", ObjThatMove.obj, "Move", MoveCommand).Execute();
        IoC.Resolve<ICommand>("Game.Queue.Push", ObjThatMove.queue, MoveCommand).Execute();
    }
}
