using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SearchAlgorithms
{
    public interface ISearchAlgorithm
    {
        IEnumerable<string> Find(IEnumerable<string> wordstream);
    }
}
