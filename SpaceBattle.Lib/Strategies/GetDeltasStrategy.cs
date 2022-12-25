namespace SpaceBattle.Lib;

public class GetDeltasStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var list1 = (List<int>) args[0];
        var list2 = (List<int>) args[1];

        var SubstList = list1.Zip(list2, (l1, l2) => l1 - l2);

        return SubstList;
    }
}
