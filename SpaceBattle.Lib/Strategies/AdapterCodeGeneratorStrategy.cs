namespace SpaceBattle.Lib;
using System.Text.RegularExpressions;

public class AdapterCodeGeneratorStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        Type adapterType = (Type) args[0];
        Type targetType = (Type) args[1];
        

        String adapterName = adapterType.Name.Substring(1);

        PropertyInfo[] properties = adapterType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);   

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

            sproperties += 
            $@"
            {p.PropertyType.Name} {p.Name} {{
            {get}
            {set}
            }}
            ";
        }  
    

        IEnumerable<MethodInfo> methods = adapterType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(m => !m.IsSpecialName);;

        string smethods = string.Empty;

        foreach (MethodInfo m in methods) {
            
            ParameterInfo[] parameters = m.GetParameters();
            string sparameters = string.Empty;
            foreach (ParameterInfo p in parameters)
            {
                sparameters += @$"{p.ParameterType.Name} {p.Name}";   
            }

            smethods +=
            $@"
            {m.ReturnType.Name.ToLower()} {m.Name} ({sparameters}) {{
                return IoC.Resolve<{m.ReturnType}>(""Game.{m.Name}.Command"", {sparameters});
            }}
            ";
        }

        
        string AdapterCode = @$"class {adapterName}Adapter : {adapterType.Name} {{
        {targetType} target;
        public {adapterName}Adapter({targetType} target) => this.target = target; 
        {sproperties}
        {smethods}
        }}";

        AdapterCode = Regex.Replace(AdapterCode, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

        return AdapterCode;
    }
}
