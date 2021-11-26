namespace System.IO.Abstractions.TestingHelpers
{

    /// <summary>
    /// </summary>
    [Serializable]
    public class StringOperations
    {
        private readonly bool caseSensitive;
        private readonly StringComparison comparison;

        /// <summary>
        /// </summary>
        public StringOperations(bool caseSensitive)
        {
            this.caseSensitive = caseSensitive;
            comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        }

        /// <summary>
        /// </summary>
        public StringComparer Comparer => caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
        /// <summary>
        /// </summary>
        public bool StartsWith(string s, string prefix) => s.StartsWith(prefix, comparison);
        /// <summary>
        /// </summary>
        public bool EndsWith(string s, string suffix) => s.EndsWith(suffix, comparison);
        /// <summary>
        /// </summary>
        public bool Equals(string x, string y) => string.Equals(x, y, comparison);
        /// <summary>
        /// </summary>
        public bool Equals(char x, char y) => caseSensitive ? x == y : char.ToUpper(x) == char.ToUpper(y);
        /// <summary>
        /// </summary>
        public int IndexOf(string s, string substring) => s.IndexOf(substring, comparison);
        /// <summary>
        /// </summary>
        public int IndexOf(string s, string substring, int startIndex) => s.IndexOf(substring, startIndex, comparison);
        /// <summary>
        /// </summary>
        public bool Contains(string s, string substring) => s.IndexOf(substring, comparison) >= 0;
        /// <summary>
        /// </summary>
        public string Replace(string s, string oldValue, string newValue) => s.Replace(oldValue, newValue, comparison);
        /// <summary>
        /// </summary>
        public char ToLower(char c) => caseSensitive ? c : char.ToLower(c);
        /// <summary>
        /// </summary>
        public char ToUpper(char c) => caseSensitive ? c : char.ToUpper(c);
        /// <summary>
        /// </summary>
        public string ToLower(string s) => caseSensitive ? s : s.ToLower();
        /// <summary>
        /// </summary>
        public string ToUpper(string s) => caseSensitive ? s : s.ToUpper();
    }
}
