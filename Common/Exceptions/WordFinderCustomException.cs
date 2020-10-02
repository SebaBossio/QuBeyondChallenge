using System;

namespace Common.Exceptions
{
    public class WordFinderCustomException : Exception
    {
        public WordFinderCustomException(string message) : base(message) { }
    }
}
