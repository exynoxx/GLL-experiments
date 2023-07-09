namespace GLL1
{
    public enum StateType
    {
        ParseSuccess,
        Reduction
    }

    public class State
    {
        public State Parent;
        public IRule Rule;
        public int InputPosition;
        public string? ParseResult;
        public StateType Type;
        public string Debug => Rule.AsString();

        public State(State parent, IRule rule, int inputPosition, string? parseResult, StateType type)
        {
            Parent = parent;
            Rule = rule;
            InputPosition = inputPosition;
            ParseResult = parseResult;
            Type = type;
        }

        public State ParseSuccess(int newInputPosition, string match)
        {
            return new State(this, Rule, newInputPosition, match, StateType.ParseSuccess);
        }

        public State Reduce(IRule newRule)
        {
            return new State(this, newRule, InputPosition, null, StateType.Reduction);
        }

        public void Reduce(Grammar g, StablePriorityQueue<State> queue)
        {
            foreach (var child in g.ReduceRule(Rule))
            {
                var reducedState = Reduce(child);
                queue.Enqueue(reducedState, -reducedState.InputPosition);
            }
        }

        public static State GetInitialState(IRule startRule)
        {
            return new State(null, startRule, 0, null, StateType.Reduction);
        }
    }
}
