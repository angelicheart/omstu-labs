namespace SpaceBattle.Lib;

public class AdapterCodeGeneratorStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        Type adapterType = (Type) args[0];
        Type targetType = (Type) args[1];

        AdapterBuilder builder = new AdapterBuilder(adapterType, targetType);

        PropertyInfo[] properties = adapterType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);   

        foreach (PropertyInfo p in properties) {
            builder.CreateProperty(p);
        }  

        IEnumerable<MethodInfo> methods = adapterType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(m => !m.IsSpecialName);

        foreach (MethodInfo m in methods) {
            builder.CreateMethod(m);
        }

        return builder.Build();
    }
}


// namespace SpaceBattle.Lib;
// using System.Text.RegularExpressions;

// public class AdapterCodeGeneratorStrategy : IStrategy
// {
//     public object Execute(params object[] args)
//     {
//         Type adapterType = (Type) args[0];
//         Type targetType = (Type) args[1];

//         string typeName = FormatGenericType(targetType);


//         String adapterName = adapterType.Name.Substring(1);

//         PropertyInfo[] properties = adapterType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);   

//         string sproperties = string.Empty;

//         foreach (PropertyInfo p in properties) {
//             string get = string.Empty;
//             string set = string.Empty;

//             string propertyname = FormatGenericType(p.PropertyType);

//             if (p.CanRead) {
//                 get = $@"get {{ return IoC.Resolve<{propertyname}>(""Game.{p.Name}.Get"", target); }}";
//             }

//             if (p.CanWrite) {
//                 set = $@"set {{ IoC.Resolve<ICommand>(""Game.{p.Name}.Set"", target, value).Execute(); }}";
//             }

//             sproperties += 
//             $@"
//             {propertyname} {p.Name} {{
//             {get}
//             {set}
//             }}
//             ";
//         }  
    

//         IEnumerable<MethodInfo> methods = adapterType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(m => !m.IsSpecialName);;

//         string smethods = string.Empty;

//         foreach (MethodInfo m in methods) {
//             string methodreturnname = FormatGenericType(m.ReturnType);
            
//             ParameterInfo[] parameters = m.GetParameters();
//             string sparameters = string.Empty;
//             string sparametersnames = string.Empty;
//             foreach (ParameterInfo p in parameters) {
//                 sparameters += @$"{FormatGenericType(p.ParameterType)} {p.Name}, ";  
//                 sparametersnames += @$"{p.Name}, ";
//             }
//             sparameters = sparameters.Remove(sparameters.Length-2);
//             sparametersnames = sparametersnames.Remove(sparametersnames.Length-2);

//             if (methodreturnname == "Void") {
//                 smethods +=
//                 $@"
//                 public void {m.Name} ({sparameters}) {{
//                     IoC.Resolve<ICommand>(""Game.{m.Name}.Command"", target, {sparametersnames}).Execute();
//                 }}
//                 ";
//             }
//             else {
//             smethods +=
//             $@"
//             {methodreturnname} {m.Name} ({sparameters}) {{
//                 return IoC.Resolve<{methodreturnname}>(""Game.{m.Name}.Command"", target, {sparametersnames});
//             }}
//             ";
//             }
//         }

        
//         string AdapterCode = @$"class {adapterName}Adapter : {adapterType.Name} {{
//         {typeName} target;
//         public {adapterName}Adapter({typeName} target) => this.target = target; 
//         {sproperties}
//         {smethods}
//         }}";

//         AdapterCode = Regex.Replace(AdapterCode, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

//         return AdapterCode;
//     }

//     public string FormatGenericType(Type type)
//     {
//         string typeName = type.Name; 

//         if (type.IsGenericType)
//         {
//             typeName = typeName.Substring(0, typeName.IndexOf('`'));

//             Type[] typeArgs = type.GetGenericArguments();
//             string[] typeArgNames = new string[typeArgs.Length];
//             for (int i = 0; i < typeArgs.Length; i++){
//                 typeArgNames[i] = FormatGenericType(typeArgs[i]);
//             }
//             typeName = $"{typeName}<{string.Join(", ", typeArgNames)}>";
//         }
//         return typeName;
//     }
// }
