namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class StringOperations
    {
        private readonly bool caseSensitive;
        private readonly StringComparison comparison;

        public StringOperations(bool caseSensitive)
        {
            this.caseSensitive = caseSensitive;
            comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        }

        public StringComparer Comparer => caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
        public bool StartsWith(string s, string prefix) => s.StartsWith(prefix, comparison);
        public bool EndsWith(string s, string suffix) => s.EndsWith(suffix, comparison);
        public bool Equals(string x, string y) => string.Equals(x, y, comparison);
        public bool Equals(char x, char y) => caseSensitive ? x == y : char.ToUpper(x) == char.ToUpper(y);
        public int IndexOf(string s, string substring) => s.IndexOf(substring, comparison);
        public int IndexOf(string s, string substring, int startIndex) => s.IndexOf(substring, startIndex, comparison);
        public bool Contains(string s, string substring) => s.IndexOf(substring, comparison) >= 0;
        public string Replace(string s, string oldValue, string newValue) => s.Replace(oldValue, newValue, comparison);
        public char ToLower(char c) => caseSensitive ? c : char.ToLower(c);
        public char ToUpper(char c) => caseSensitive ? c : char.ToUpper(c);
        public string ToLower(string s) => caseSensitive ? s : s.ToLower();
        public string ToUpper(string s) => caseSensitive ? s : s.ToUpper();
    }
}
