using Core.SearchAlgorithms.Implementations;
using Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SearchAlgorithms
{
    static class AlgorithmsCreator
    {
        public static ISearchAlgorithm GetAlgorithm(Dictionary<string, string> algorithmsKeys, string key, IEnumerable<string> matrix)
        {
            ISearchAlgorithm algorithm;

            if (algorithmsKeys.ContainsKey(key))
            {
                algorithm = (ISearchAlgorithm) Activator.CreateInstance(Type.GetType(algorithmsKeys[key]), matrix);
            }
            else
            {
                throw new WordFinderCustomException($"The key '{key}' dosen't correspond to any algorithm configured.");
            }

            return algorithm;
        }

        //public static ISearchAlgorithm GetAlgorithmFactory(string key, IEnumerable<string> matrix)
        //{
        //    ISearchAlgorithm algorithm;

        //    switch (key)
        //    {
        //        case "Boyer-Moore":
        //            algorithm = new BoyerMoore(matrix);
        //            break;
        //        default:
        //            throw new WordFinderCustomException($"The key '{key}' dosen't correspond to any algorithm configured.");
        //    }

        //    return algorithm;
        //}
    }
}
