using System;
using System.Collections.Generic;

namespace Friendslop.Reference
{
    /// <summary>
    /// Deterministic shuffled deck — the building block for "GustDeck"-style
    /// escalation systems (framework/02: systems over content). Draws cycle
    /// forever, reshuffling each pass with a seed derived from the original,
    /// so host and clients given the same seed see the same sequence.
    ///
    /// Deliberately a PLAIN CLASS, no UnityEngine: that's what makes it
    /// EditMode-testable (framework/12 level 1). Keep game logic in this
    /// shape wherever possible and let MonoBehaviours be thin wrappers.
    /// </summary>
    public class SeededDeck<T>
    {
        private readonly IReadOnlyList<T> items;
        private readonly List<int> order = new();
        private Random rng;
        private int cursor;
        private int pass;
        private readonly int seed;

        public SeededDeck(IReadOnlyList<T> items, int seed)
        {
            if (items == null || items.Count == 0)
                throw new ArgumentException("SeededDeck needs at least one item");
            this.items = items;
            this.seed = seed;
            Reshuffle();
        }

        public T Draw()
        {
            if (cursor >= order.Count) Reshuffle();
            return items[order[cursor++]];
        }

        private void Reshuffle()
        {
            // Seed folds in the pass number so successive passes differ but
            // remain fully determined by the original seed.
            rng = new Random(unchecked(seed * 486187739 + pass));
            pass++;
            cursor = 0;
            order.Clear();
            for (int i = 0; i < items.Count; i++) order.Add(i);
            for (int i = order.Count - 1; i > 0; i--)      // Fisher-Yates
            {
                int j = rng.Next(i + 1);
                (order[i], order[j]) = (order[j], order[i]);
            }
        }
    }
}
