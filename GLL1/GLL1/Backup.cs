/*using LanguageExt;
using System.Collections.Immutable;
using System.Data;

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

public interface IParser<AST>
{
    public Result<AST> Parse(string input, int pos);
    public bool CanConsume(string input, int pos);
    //WWstatic abstract IParser<AST> operator +(IParser<AST> a, IParser<AST> b);
}

public interface IComposableParser<AST> : IParser<AST>, IEnumerable<IParser<AST>>
{
    public List<IParser<AST>> GetParsers();
    public List<IParser<AST>> Reduce();
}



public static class Extensions
{
    *//*public static IComposableParser<T> ReduceComposable<T>(IComposableParser<T> parser)
    {
        //find first non-terminal and reduce that


        throw new NotImplementedException();
    }

    private static IComposableParser<T> Reduce<T>(AndParser<T> parser)
    {
        //find first non-terminal and reduce that
        if (parser.GetParsers[0] is not IComposableParser<T> composable)

        throw new NotImplementedException();
    }

    private static IComposableParser<T> Reduce<T>(OrParser<T> parser)
    {
        //find first non-terminal and reduce that


        throw new NotImplementedException();
    }*//*

}

public record State<T>
{
    public State<T> Parent;
    public int InputPosition;
    public IParser<T> CurrentRule;
    public T Result;

    public State(State<T> parent, int inputPosition, IParser<T> currentRule, T result)
    {
        Parent = parent;
        InputPosition = inputPosition;
        CurrentRule = currentRule;
        Result = result;
    }

    public static State<T> FromPrevious(State<T> state, IParser<T> currentRule)
    {
        return new State<T>(state, state.InputPosition, currentRule, state.Result);
    }
}


public class GLL<AST>
{

    private readonly GSS<AST> GSS = new();
    private readonly System.Collections.Generic.HashSet<State<AST>> MemorizationTable = new();

    public AST Parse(IParser<AST> parser, string input)
    {
        if(parser is not IComposableParser<AST> composable)
        {
            return parser.Parse(input, 0).Value;
        }

        GSS.Add(new State<AST>(default, 0, composable, default));

        while (true)
        {

            *//*
             1) try parse
             2) fail ? stop
             3) success ? new state with position and reduced changed
             *//*


            var newStates = new List<State<AST>>(GSS.Heads.Count);
            foreach (var state in GSS.Heads)
            {

                //shift
                var parseResult = state.CurrentRule.Parse(input, state.InputPosition);
                if (parseResult.IsFailure)
                {
                    //stop progress.
                }

                if (state.CurrentRule is not IComposableParser<AST> composableCurrent)
                {
                    //we done maybe?
                    return parseResult.Value; //TODO reconstruct ast
                }

                //reduce
                var reducedStates = composableCurrent.Reduce()
                        .Select(p => new State<AST>(state, parseResult.Pos, p, parseResult.Value!))
                        .Where(state => !MemorizationTable.Contains(state));

                MemorizationTable.Append(reducedStates);

                *//*if (newState.InputPosition == input.Length)
                {
                    //return. we done
                }*//*
                newStates.Append(reducedStates);
            }

            GSS.Heads = newStates;
        }
    }
}

*//*//reduce    
                    var reducedParsers = Extensions.ReduceComposable(composable);
                    var reducedStates = reducedParsers
                        .Select(p => State<AST>.FromPrevious(state, p))
                        .Where(state => !MemorizationTable.Contains(state))
                        .ToArray();

                    MemorizationTable.Append(reducedStates);

                    var (head,tail) = reducedStates switch
                    {
                        [var x,.. var xs] => (x, xs ),
                        _ => throw new NotImplementedException(),
                    };

                    state = head;
                    newStates.Append(reducedStates);*//*
public class GSS<T>
{
    public List<State<T>> Heads = new();

    public void Add(State<T> state)
    {
        Heads.Add(state);
    }

}

*/