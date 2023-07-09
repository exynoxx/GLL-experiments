
using System.Runtime.InteropServices;

namespace GLL1
{
    public class Program
    {


        public static int Main(string[] args)
        {
            var gll = new Algorithm();
            var ast = new ASTCreator();
            var grammar = new Grammar(new StreamReader(File.Open("../../../ExampleGrammer.txt", FileMode.Open)).ReadToEnd(), "exp");

            grammar.RuleOutput("exp", (x) => x);
            grammar.RuleOutput("mult", (x) => x);
            grammar.RuleOutput("int", (x) => int.Parse((string)x[0]));

            var state = gll.Parse(grammar, "1+2*3");

            var tree = new Dictionary<IRule, List<State>>();

            PrintTree(state);

            return 0;
        }

        public static void PrintTree(State state)
        {
            if(state.Parent.Parent != null) PrintTree(state.Parent);
            if (state.Type == StateType.Reduction) {
                Console.WriteLine($"{state.InputPosition} REDUCTION {state.Parent.Rule.AsString()} -> {state.Rule.AsString()}");

            }
            else
            {
                Console.WriteLine($"{state.InputPosition} PARSE {state.ParseResult}");
            }
        }

        
    }
}
