namespace SpaceBattle.Lib;

public class GetItemStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        IoC.Resolve<Dictionary<string, IUObject>>("Get.Objects").TryGetValue((string) args[0], out IUObject obj);

        if (obj != null) return obj;
        
        throw new Exception();
    }
}
