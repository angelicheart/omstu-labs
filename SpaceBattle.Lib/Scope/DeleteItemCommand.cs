namespace SpaceBattle.Lib;

public class DeleteItemCommand : ICommand
{
    readonly string id;

    public DeleteItemCommand(string id)
    {
        this.id = id;
    }
    
    public void Execute()
    {
        Dictionary<string, IUObject> objects = IoC.Resolve<Dictionary<string, IUObject>>("Get.Objects");
        objects.Remove(id);
    }
}
