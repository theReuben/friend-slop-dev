using System.Collections.Generic;
using NUnit.Framework;

namespace Friendslop.Reference.Tests
{
    /// <summary>
    /// THE level-1 pattern (framework/12): pure logic, no Unity objects, runs
    /// in milliseconds. Determinism tests like these are not optional in this
    /// factory — host-authoritative netcode replicates SEEDS, not sequences
    /// (framework/04), so "same seed, same sequence" is a networked invariant.
    /// </summary>
    public class SeededDeckTests
    {
        private static readonly string[] Cards = { "gust", "lull", "squall", "updraft" };

        [Test]
        public void SameSeed_SameSequence()
        {
            var a = new SeededDeck<string>(Cards, seed: 1234);
            var b = new SeededDeck<string>(Cards, seed: 1234);
            for (int i = 0; i < 50; i++)
                Assert.AreEqual(a.Draw(), b.Draw(), $"diverged at draw {i}");
        }

        [Test]
        public void DifferentSeeds_DifferentSequences()
        {
            var a = new SeededDeck<string>(Cards, seed: 1);
            var b = new SeededDeck<string>(Cards, seed: 2);
            var divergence = false;
            for (int i = 0; i < 20 && !divergence; i++)
                divergence = a.Draw() != b.Draw();
            Assert.IsTrue(divergence, "20 identical draws from different seeds is broken shuffling");
        }

        [Test]
        public void EveryPass_ContainsEveryItemExactlyOnce()
        {
            var deck = new SeededDeck<string>(Cards, seed: 99);
            for (int p = 0; p < 3; p++)                          // three full passes
            {
                var seen = new List<string>();
                for (int i = 0; i < Cards.Length; i++) seen.Add(deck.Draw());
                CollectionAssert.AreEquivalent(Cards, seen, $"pass {p} not a permutation");
            }
        }

        [Test]
        public void EmptyDeck_ThrowsInsteadOfLooping()
        {
            Assert.Throws<System.ArgumentException>(
                () => new SeededDeck<string>(new string[0], seed: 0));
        }
    }
}
