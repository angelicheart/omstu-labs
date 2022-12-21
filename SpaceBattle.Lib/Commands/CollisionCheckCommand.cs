namespace SpaceBattle.Lib;

public class CollisionCheckCommand : ICommand
{
    private readonly IUObject obj1, obj2;
    public CollisionCheckCommand(IUObject Obj1, IUObject Obj2)
    {
        obj1 = Obj1;
        obj2 = Obj2;
    }
    
    public void Execute()
    {   
        var CollisionSolutionTree = IoC.Resolve<IDictionary<double, object>>("Game.Collision.Tree");

        bool collision = IoC.Resolve<bool>("Game.Collision.CheckWithTree", CollisionSolutionTree, obj1, obj2);

        if (collision) throw new Exception("COLLISION!");
    }
}