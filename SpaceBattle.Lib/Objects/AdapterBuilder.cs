// namespace SpaceBattle.Lib;

// using System.Text.RegularExpressions;
// using Scriban;

// public class AdapterBuilder {
//     private string adaptername;
//     private string adaptertype;
//     private string targettype;
//     private string properties;
//     private string methods;

//     public AdapterBuilder (Type adaptertype, Type targettype) {
//         this.adaptername = adaptertype.Name.Substring(1);
//         this.adaptertype = adaptertype.Name;
//         this.targettype = targettype.Name;
//     }

//     public string CreateMethod() {

//     }

//     public void CreateProperty(bool canread, bool canwrite, string propertytypename, string propertyname) {
//         string get = string.Empty, set = string.Empty;

//         if (canread) {
//             get = $@"get {{ return IoC.Resolve<{propertytypename}>(""Game.{propertyname}.Get"", target); }}";
//         }

//         if (canwrite) {
//             set = $@"set {{ IoC.Resolve<ICommand>(""Game.{propertyname}.Set"", target, value).Execute(); }}";
//         }

//         properties += 
//         $@"
//         {propertytypename} {propertyname} {{
//         {get}
//         {set}
//         }}
//         ";
//     }

//     public string Build() {
//         var tpl = @$"
//         class {adaptername}Adapter : {adaptertype} {{
//         {targettype} target;
//         public {adaptername}Adapter({targettype} target) => this.target = target; 
//         {properties}
//         }}";

//         var template = Template.Parse(tpl);
//         var result = 

//     }
// }