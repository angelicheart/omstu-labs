namespace SpaceBattle.Lib;
using System.Text.RegularExpressions;

public class AdapterCodeGeneratorStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        Type adapterType = (Type) args[0];
        Type targetType = (Type) args[1];

        String adapterName = adapterType.Name.Substring(1);

        PropertyInfo[] properties = adapterType.GetProperties(BindingFlags.Public | BindingFlags.Instance);   

        string sproperties = string.Empty;

        foreach (PropertyInfo p in properties) {
            string get = string.Empty;
            string set = string.Empty;

            if (p.CanRead) {
                get = $@"get {{ return IoC.Resolve<{p.PropertyType.Name}>(""Game.{p.Name}.Get"", target); }}";
            }

            if (p.CanWrite) {
                set = $@"set {{ IoC.Resolve<ICommand>(""Game.{p.Name}.Set"", target, value).Execute(); }}";
            }

            sproperties += $@"
            {p.PropertyType.Name} {p.Name} {{
            {get}
            {set}
            }}
            ";
        }   

        MethodInfo[] methods = adapterType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

        String smethods = string.Empty;

        foreach (MethodInfo m in methods) {
            smethods += $@"{m} {{ IoC.Resolve<IStrategy>(""Game.{m.Name}.Set"", target, ) }}";
        } 


            
        string AdapterCode = @$"class {adapterName}Adapter : {adapterType.Name} {{
        {targetType.Name} target;
        public {adapterName}Adapter({targetType.Name} target) => this.target = target; 
        {sproperties}
        {smethods}
        }}";

        AdapterCode = Regex.Replace(AdapterCode, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

        return AdapterCode;
    }
}
