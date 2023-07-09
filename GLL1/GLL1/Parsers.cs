/*using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


public class EmptyParser<TOutput> : IParser<TOutput>
{
    public bool CanConsume(string input, int pos)
    {
        throw new NotImplementedException();
    }

    public Result<TOutput> Parse(string input, int pos)
    {
        throw new NotImplementedException();
    }

    public static IParser<TOutput> Create() => new EmptyParser<TOutput>();
}

public class IntParser<TOutput> : IParser<TOutput>
{
    private readonly Func<string, TOutput> _converter;

    public IntParser(Func<string, TOutput> converter)
    {
        _converter = converter;
    }

    public bool CanConsume(string input, int pos) => Parse(input,pos).IsSuccess;

    public Result<TOutput> Parse(string input, int pos)
    {
        int i = pos;
        while (char.IsDigit(input[i])) i++;

        if (i == pos)
        {
            return Result<TOutput>.Fail();
        }

        return Result<TOutput>.Ok(_converter(input.Substring(pos,i)));
    }

    public static AndParser<TOutput> operator +(IntParser<TOutput> a, IParser<TOutput> b)
    {
        return new AndParser<TOutput>(a, b);
    }

    public static OrParser<TOutput> operator |(IntParser<TOutput> a, IParser<TOutput> b)
    {
        return new OrParser<TOutput>(a, b);
    }
}

public class RegexParser<TOutput> : IParser<TOutput>
{
    public Regex Regex { get; set; }
    public readonly Func<string, TOutput> _converter;

    public RegexParser(Regex regex, Func<string, TOutput> converter)
    {
        Regex = regex;
        _converter = converter;
    }

    public bool CanConsume(string input, int pos) => Parse(input, pos).IsSuccess;

    public Result<TOutput> Parse(string input, int pos)
    {
        var match = Regex.Match(input, pos);
        if(match.Success)
        {
            return Result<TOutput>.Ok(_converter(match.Value));
        }

        return Result<TOutput>.Fail();
    }

    public static AndParser<TOutput> operator +(RegexParser<TOutput> a, IParser<TOutput> b)
    {
        return new AndParser<TOutput>(a, b);
    }

    public static OrParser<TOutput> operator |(RegexParser<TOutput> a, IParser<TOutput> b)
    {
        return new OrParser<TOutput>(a, b);
    }

}


public class OrParser<TOutput> : IComposableParser<TOutput>
{

    IParser<TOutput>[] _parsers;

    public OrParser(params IParser<TOutput>[] parsers)
    {
        _parsers = parsers;
    }

    public bool CanConsume(string input, int pos) => _parsers[0].CanConsume(input,pos); //TODO always true??
    public List<IParser<TOutput>> GetParsers() => _parsers.ToList();
    public Result<TOutput> Parse(string input, int pos) => _parsers[0].Parse(input, pos);

    public IEnumerator<IParser<TOutput>> GetEnumerator()
    {
        foreach(var parser in _parsers)
        {
            if(parser is IComposableParser<TOutput> composable)
            {
                foreach(var p  in composable) yield return p;
            }

            yield return parser;
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public List<IParser<TOutput>> Reduce()
    {
        return GetParsers();
    }

    public static AndParser<TOutput> operator +(OrParser<TOutput> a, IParser<TOutput> b)
    {
        return new AndParser<TOutput>(a, b);
    }

    public static OrParser<TOutput> operator |(OrParser<TOutput> a, IParser<TOutput> b)
    {
        return new OrParser<TOutput>(a, b);
    }
}

public class AndParser<TOutput> : IComposableParser<TOutput>
{

    IParser<TOutput>[] _parsers;

    public AndParser(params IParser<TOutput>[] parsers)
    {
        _parsers = parsers;
    }

    public bool CanConsume(string input, int pos) => _parsers[0].CanConsume(input, pos);
    public List<IParser<TOutput>> GetParsers() => _parsers.ToList();
    public Result<TOutput> Parse(string input, int pos) => _parsers[0].Parse(input, pos);

    public IEnumerator<IParser<TOutput>> GetEnumerator()
    {
        foreach (var parser in _parsers)
        {
            if (parser is IComposableParser<TOutput> composable)
            {
                foreach (var p in composable) yield return p;
            }

            yield return parser;
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public List<IParser<TOutput>> Reduce()
    {
        if (_parsers[0] is IComposableParser<TOutput> composable)
        {
            var result = composable
                .Reduce()
                .Select(child => new AndParser<TOutput>(_parsers.Select((p, i) => i == 0 ? child : p).ToArray()));
        }

        return new AndParser<TOutput>(_parsers.Skip(1).ToArray()).Reduce(); //TODO optimize later
    }

    public static AndParser<TOutput> operator +(AndParser<TOutput> a, IParser<TOutput> b)
    {
        return new AndParser<TOutput>(a, b);
    }

    public static OrParser<TOutput> operator |(AndParser<TOutput> a, IParser<TOutput> b)
    {
        return new OrParser<TOutput>(a, b);
    }
}

public static class ParserHelpers
{
    public static Func<string, IntParser<T>> Int<T>(Func<string,T> converter) => (s) => new IntParser<T>(converter);
    public static RegexParser<T> Regex<T>(string regex, Func<string,T> converter) => new RegexParser<T>(new Regex(regex),converter);

    public static IParser<T> Then<T>(this IParser<T> parser1, IParser<T> parser2) => new AndParser<T>(parser1, parser2);
}*/