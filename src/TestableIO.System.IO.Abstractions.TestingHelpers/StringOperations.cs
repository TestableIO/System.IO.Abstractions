namespace System.IO.Abstractions.TestingHelpers
{

    /// <summary>
    /// Provides operations against path strings dependeing on the case-senstivity of the runtime platform.
    /// </summary>
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public class StringOperations
    {
        private readonly bool caseSensitive;
        private readonly StringComparison comparison;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public StringOperations(bool caseSensitive)
        {
            this.caseSensitive = caseSensitive;
            comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        }

        /// <summary>
        /// Provides a string comparer.
        /// </summary>
        public StringComparer Comparer => caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
        /// <summary>
        /// Determines whether the given string starts with the given prefix.
        /// </summary>
        public bool StartsWith(string s, string prefix) => s.StartsWith(prefix, comparison);
        /// <summary>
        /// Determines whether the given string ends with the given suffix.
        /// </summary>
        public bool EndsWith(string s, string suffix) => s.EndsWith(suffix, comparison);
        /// <summary>
        /// Determines whether the given strings are equal.
        /// </summary>
        public bool Equals(string x, string y) => string.Equals(x, y, comparison);
        /// <summary>
        /// Determines whether the given characters are equal.
        /// </summary>
        public bool Equals(char x, char y) => caseSensitive ? x == y : char.ToUpper(x) == char.ToUpper(y);
        /// <summary>
        /// Determines the index of the given substring in the string.
        /// </summary>
        public int IndexOf(string s, string substring) => s.IndexOf(substring, comparison);
        /// <summary>
        /// Determines the index of the given substring in the string.
        /// </summary>
        public int IndexOf(string s, string substring, int startIndex) => s.IndexOf(substring, startIndex, comparison);
        /// <summary>
        /// Determines whether the given string contains the given substring.
        /// </summary>
        public bool Contains(string s, string substring) => s.IndexOf(substring, comparison) >= 0;
        /// <summary>
        /// Replaces a given value by a new value.
        /// </summary>
        public string Replace(string s, string oldValue, string newValue) => s.Replace(oldValue, newValue, comparison);
        /// <summary>
        /// Provides the lower-case representation of the given character.
        /// </summary>
        public char ToLower(char c) => caseSensitive ? c : char.ToLower(c);
        /// <summary>
        /// Provides the upper-case representation of the given character.
        /// </summary>
        public char ToUpper(char c) => caseSensitive ? c : char.ToUpper(c);
        /// <summary>
        /// Provides the lower-case representation of the given string.
        /// </summary>
        public string ToLower(string s) => caseSensitive ? s : s.ToLower();
        /// <summary>
        /// Provides the upper-case representation of the given string.
        /// </summary>
        public string ToUpper(string s) => caseSensitive ? s : s.ToUpper();
    }
}
