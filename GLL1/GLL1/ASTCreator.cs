using System.Collections.Generic;
using System.Data;

namespace GLL1
{
    public class ASTCreator
    {
        public void ReconstructUpwards(Grammar g, State state, Dictionary<IRule,List<State>> tree)
        {
            var parent = state.Parent;
            if (parent == null) return;


            ReconstructUpwards(g, parent, tree);


            /*if (state.Type is StateType.ParseSuccess && parent.Rule is Terminal t)
            {
                var converter = g.Convertions[t];
                var ourNode = new object[] { converter.DynamicInvoke(state.ParseResult) };
                ourNode.Append(accumulated);
                return ReconstructUpwards(g, state.Parent, ourNode); 
            }*/

            //if (state.Type is StateType.Reduction)

            /*if (parent.Rule is Terminal tt)
            {
                var converter = g.Convertions[tt];
                var ourNode = new object[] { converter.DynamicInvoke(state.ParseResult) };
                ourNode.Append(accumulated);
                return ReconstructUpwards(g, state.Parent, ourNode);
            }*/

            if(!tree.ContainsKey(parent.Rule))
            {
                tree[parent.Rule] = new List<State>();
            }

            tree[parent.Rule].Add(state);

            Console.WriteLine($"{state.InputPosition}, {state.Rule.AsString()}, {state.ParseResult}");

        }
    }
}
