using System.Text.RegularExpressions;

namespace GLL1
{
    public interface IRule
    {
    }

    public record AndRule(IRule[] Body, int currentRule) : IRule;
    public record ORRule(IRule[] Body) : IRule;
    public record Terminal(string Id) : IRule;
    public record NonTerminal(string Body) : IRule;
    public record NonTerminalRegex(Regex Body) : IRule;

    public static class RuleEx
    {
        public static string AsString(this IRule rule)
        {
            return rule switch
            {
                ORRule or => string.Join("|", or.Body.Select(AsString)),
                AndRule and => string.Join(",", and.Body.Select(AsString)),
                Terminal t => t.Id,
                NonTerminal non => non.Body,
                NonTerminalRegex non => non.Body.ToString(),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
