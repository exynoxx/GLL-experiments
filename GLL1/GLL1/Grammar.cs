using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

namespace GLL1
{
    /*
     exp := exp '+' exp | int
     int := r"\d+"
     */

    public class Grammar
    {
        public Dictionary<Terminal, IRule> Definitions;
        public Dictionary<Terminal, Func<object[],object>> Convertions;
        public Terminal StartRule { get; set; }

        public Grammar(string grammar, string startRule)
        {
            Definitions = new Dictionary<Terminal, IRule>();
            Convertions = new Dictionary<Terminal, Func<object[], object>>();
            Parse(grammar);
            StartRule = new Terminal(startRule);
        }

        public void Parse(string grammar)
        {
            Parse(grammar.Split(";", StringSplitOptions.RemoveEmptyEntries));
        }

        public void Parse(string[] lines)
        {
            foreach (var line in lines)
            {
                var _ = line.Split(":=");

                var (id, body) = (_.First(), _.Last());

                if(id == null || body == null)
                {
                    throw new Exception("null");
                }

                Definitions[new Terminal(id.Trim())] = ParseBody(body.Trim());

            }
        }

        private static IRule ParseBody(string rawBody)
        {
            var body = rawBody.Trim();

            if (body.Contains('|'))
            {
                var alternatives = body.Split('|').Map(ParseBody).ToArray();
                return new ORRule(alternatives);
            }

            var trimmedBody = body.Trim();
            if (trimmedBody.Contains(' '))
            {
                return new AndRule(body.Split(' ').Map(ParseBody).ToArray(), 0);
            }

            if (trimmedBody[0] == 'r')
            {
                return new NonTerminalRegex(new Regex(trimmedBody.Substring(2, trimmedBody.Length - 3)));
            }

            if (trimmedBody[0] == '"' | trimmedBody[0] == '\'')
            {
                return new NonTerminal(trimmedBody.Substring(1, trimmedBody.Length - 2));
            }

            return new Terminal(trimmedBody);
        }

        public bool ReduceAble(IRule rule)
        {
            return rule switch
            {
                AndRule andRule => ReduceAble(andRule.Body[andRule.currentRule]) || andRule.currentRule < andRule.Body.Length-1, //if child not reduceable, have space to just return nexts
                ORRule => true,
                Terminal => true,
                _ => false
            };
        }

        public IEnumerable<IRule> ReduceRule(IRule rule)
        {
            if (rule is AndRule andRule && ReduceAble(andRule))
            {
                if (!ReduceAble(andRule.Body[andRule.currentRule]))
                {
                    //child cannot be reduced, just make next item the current rule
                    yield return new AndRule(andRule.Body, andRule.currentRule+1);
                }
                else
                {
                    //child can be reduced
                    var firstRuleReduced = ReduceRule(andRule.Body[andRule.currentRule]);
                    foreach (var child in firstRuleReduced)
                    {
                        var content = andRule.Body.ToArray();
                        content[andRule.currentRule] = child;


                        yield return new AndRule(content.ToArray(), andRule.currentRule);
                    }
                }                
            } else if (rule is ORRule orRule)
            {
                foreach (var child in orRule.Body)
                {
                    yield return child;
                }

            } else if (rule is Terminal terminal)
            {
                yield return Definitions[terminal];
            } /*else
            {
                yield return rule;
                //TODO non terminal
            }*/


        }

        public void RuleOutput(string terminal, Func<object[],object> converter)
        {
            Convertions[new Terminal(terminal)] = converter;
        }

    }
}
