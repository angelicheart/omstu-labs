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
        var list1 = IoC.Resolve<List<int>>("Game.Collision.GetList", obj1);
        var list2 = IoC.Resolve<List<int>>("Game.Collision.GetList", obj2);    

        var dlist = IoC.Resolve<IEnumerable<int>>("Game.Collision.GetDeltas", list1, list2);     

        bool collision = IoC.Resolve<bool>("Game.Collision.CheckWithTree", dlist);

        if (collision) throw new Exception("COLLISION!");
    }
}
