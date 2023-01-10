namespace SpaceBattle.Lib;

public class TreeCreateStrategy : IStrategy
{
    public object Execute(params object[] args) 
    {
        var matrix = (List<List<int>>) args[0];
        var tree = new Dictionary<int, object>();

        foreach (var i in matrix)
        {
            var temp = tree;

            foreach (var j in i)
            {
                temp.TryAdd(j, new Dictionary<int, object>());
                temp = (Dictionary<int, object>) temp[j];
            }
        }

        return tree;
    }
}
