using GLL1;

namespace GLLTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReduceRecursiveAnd()
        {
            var g = new Grammar("exp := a '-'", "exp");


            var rule = new AndRule(new IRule[] {
                    new AndRule(new IRule[] {
                        new NonTerminal(""),
                        new NonTerminal("") ,
                        new NonTerminal("")
                    },2),
                    new Terminal("exp"),
                    new Terminal("exp")
            }, 0);
            var reduced = g.ReduceRule(rule).Single();
            Assert.That(reduced is AndRule);
            Assert.That(((AndRule)reduced).currentRule == 1);
        }

        [Test]
        public void ReduceAnd()
        {
            var g = new Grammar("exp := a '-'", "exp");


            var rule = new AndRule(
                new IRule[] {
                    new NonTerminal("x"),
                        new NonTerminal("5") ,
                        new NonTerminal("5")
            }, 0);
            var reduced = g.ReduceRule(rule).Single();
            Assert.That(reduced is AndRule);
            Assert.That(((AndRule)reduced).currentRule == 1);
        }
    }
}