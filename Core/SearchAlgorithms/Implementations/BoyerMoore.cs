using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.SearchAlgorithms.Implementations
{
    class BoyerMoore : ISearchAlgorithm
	{
        #region Fields
        private const int InvalidIndex = -1;
		private IEnumerable<string> _matrix { get; set; }
		private IEnumerable<string> _wordstream { get; set; }
		private Dictionary<string, int> _wordsCount { get; set; }
        #endregion Fields

        #region Constructors
        public BoyerMoore(IEnumerable<string> matrix)
        {
            this._matrix = matrix;
		}
		#endregion Constructors

		#region PublicMethods
		public IEnumerable<string> Find(IEnumerable<string> wordstream)
		{
			this._wordsCount = wordstream.GroupBy(x => x).ToDictionary(x => x.Key, y => 0);

            foreach (var pattern in wordstream)
            {
				int[] goodSuffixShift = BuildGoodSuffixShift(pattern);
				byte[] badCharShift = BuildBadCharacterShift(pattern);

				foreach (var text in _matrix)
				{
					var searchIndex = Search(pattern, text, goodSuffixShift, badCharShift, 0);
					if(searchIndex != InvalidIndex)
                    {
						this._wordsCount[pattern]++;
					}
				}
			}

			return this._wordsCount.Where(x => x.Value > 0).OrderByDescending(x => x.Value).Take(10).Select(x => x.Key).ToList();
		}
		#endregion PublicMethods

		#region PrivateMethods
		private int[] BuildGoodSuffixShift(string pattern)
		{
			int[] goodSuffixShift = new int[pattern.Length];
			int[] suff = new int[pattern.Length];
			int i, j;

			FindSuffixes(suff, pattern);

			for (i = 0; i < pattern.Length; ++i)
            {
				goodSuffixShift[i] = pattern.Length;
			}

			j = 0;
			for (i = pattern.Length - 1; i >= 0; --i)
			{
				if (suff[i] == i + 1)
				{
					for (; j < pattern.Length - 1 - i; ++j)
					{
						if (goodSuffixShift[j] == pattern.Length)
						{
							goodSuffixShift[j] = pattern.Length - 1 - i;
						}
					}
				}
			}
			for (i = 0; i <= pattern.Length - 2; ++i)
			{
				goodSuffixShift[pattern.Length - 1 - suff[i]] = pattern.Length - 1 - i;
			}

			return goodSuffixShift;
		}

		private void FindSuffixes(int[] suff, string pattern)
		{
			int f = 0, g, i;
			suff[pattern.Length - 1] = pattern.Length;
			g = pattern.Length - 1;
			for (i = pattern.Length - 2; i >= 0; --i)
			{
				if (i > g && suff[i + pattern.Length - 1 - f] < i - g)
				{
					suff[i] = suff[i + pattern.Length - 1 - f];
				}
				else
				{
					if (i < g)
					{
						g = i;
					}
					f = i;
					while (g >= 0 && pattern[g] == pattern[g + pattern.Length - 1 - f])
					{
						--g;
					}
					suff[i] = f - g;
				}
			}
		}

		private byte[] BuildBadCharacterShift(string pattern)
		{
			byte[] badCharShift = new byte[0x10000];

			for (int i = 0; i < badCharShift.Length; i++)
			{
				badCharShift[i] = (byte)pattern.Length;
			}
			for (int i = 0; i < pattern.Length - 1; i++)
			{
				badCharShift[pattern[i]] = (byte)(pattern.Length - i - 1);
			}

			return badCharShift;
		}

		private int Search(string pattern, string text, int[] goodSuffixShift, byte[] badCharShift, int startIndex)
		{
			int i = startIndex;
			while (i <= (text.Length - pattern.Length))
			{
				int j = pattern.Length - 1;
				while (j >= 0 && pattern[j] == text[i + j])
				{
					j--;
				}

				if (j < 0)
				{
					return i;
				}
				else
				{
					i += Math.Max(goodSuffixShift[j], badCharShift[text[i + j]] - pattern.Length + 1 + j);
				}
			}

			return InvalidIndex;
		}
		#endregion PrivateMethods
	}
}
