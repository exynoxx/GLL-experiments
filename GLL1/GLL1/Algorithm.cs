using LanguageExt.ClassInstances.Pred;
using System.Text.RegularExpressions;

namespace GLL1
{





    public class Result<T>
    {
        public T Value;
        public bool IsSuccess;
        public bool IsFailure => !IsSuccess;

        private Result(T value)
        {
            Value = value;
            IsSuccess = true;
        }

        private Result()
        {
            IsSuccess = false;
        }

        public static Result<T> Ok(T value) => new Result<T>(value);
        public static Result<T> Fail() => new Result<T>();
    }

    

    public class Algorithm
    {
        public static bool CanParse(Grammar g, IRule rule)
        {
            return rule switch
            {
                AndRule andRule => CanParse(g, andRule.Body[andRule.currentRule]),
                ORRule => false,
                Terminal => false,
                _ => true
            };
        }

        public IRule FindNonTerminal(IRule rule)
        {
            return rule switch
            {
                AndRule andRule => FindNonTerminal(andRule.Body[andRule.currentRule]),
                ORRule => throw new Exception(""),
                var x => x
            };
        }


        public void ParseState(Grammar g, StablePriorityQueue<State> queue, State state, string input)
        {
            var rule = state.Rule switch
            {
                AndRule andRule => FindNonTerminal(andRule),
                var x => x
            };

            if (rule is NonTerminalRegex regex)
            {
                var match = regex.Body.Match(input, state.InputPosition);
                if (match.Success)
                {
                    int newPosition = match.Index + match.Length;
                    var newState = state.ParseSuccess (newPosition, match.Value);

                    //TODO find solution
                    if(newPosition == input.Length)
                    {
                        queue.Enqueue(newState,-newState.InputPosition);
                    }

                    if(state.Rule is AndRule)
                    {
                        newState.Reduce(g, queue);
                    }
                }
            } 
            else if(rule is NonTerminal nonTerminal)
            {
                var currentInput = input.Substring(state.InputPosition);
                if (currentInput.StartsWith(nonTerminal.Body))
                {
                    var newPosition = state.InputPosition + nonTerminal.Body.Length;
                    var newState = state.ParseSuccess(newPosition, nonTerminal.Body);

                    if (newPosition == input.Length)
                    {
                        queue.Enqueue(newState, -newState.InputPosition);
                    }

                    if (state.Rule is AndRule)
                    {
                        newState.Reduce(g, queue);
                    }
                }
            }

        }

        public State Parse(Grammar g, string input)
        {
            //start of rule
            //terminal: reduce to new state
            //non terminal: try parse rule. inc positions and save the returned result

            //reduce:
            //1 A->B = B
            //2 AND: reduce first terminal
            //3 OR: branch state foreach alternative.


            var initialState = State.GetInitialState(g.StartRule);

            var queue = new StablePriorityQueue<State>();
            queue.Enqueue(initialState,0);

            while (queue.Count>0)
            {
                var state = queue.Dequeue();

                Console.WriteLine($"{state.InputPosition} ({queue.Count})");

                if(state.InputPosition == input.Length && !g.ReduceAble(state.Rule))
                {
                    return state;
                }

                //parse
                if (CanParse(g, state.Rule))
                {
                    ParseState(g, queue, state, input);
                    continue;
                }

                //reduce
                state.Reduce(g, queue);
            }

            return null;
            
        }
    }
}
