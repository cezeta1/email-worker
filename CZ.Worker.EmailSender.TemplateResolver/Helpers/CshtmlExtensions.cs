using System.Text.RegularExpressions;
using Jering.Javascript.NodeJS;

namespace CZ.Worker.EmailSender.TemplateResolver.Helpers;
public static partial class CshtmlExtensions
{
    [GeneratedRegex(@"\s*---(\r\n|\r|\n)(.*)\s*---(\r\n|\r|\n)")]
    private static partial Regex _rgx();
    
    private const string _relativeTo = "../src/CZ.Worker.EmailSender.Templates/Styles";

    public static string CleanFrontMatter(this string str)
        => _rgx().Replace(str, "");

    public static async Task<string> PostProcess(this string inputString, INodeJSService nodeService)
        => await nodeService.InvokeFromFileAsync<string>("post-process.js", args: [ inputString, _relativeTo ]);
}
