namespace calculator.extensions;

public static class StringExtensions {
    public static bool FirstNumber(this string str, out string num, int start = 0) {
        num = string.Empty;
        var ip = start;

        if (!char.IsDigit(str[ip]) && !Equals(str[ip], '.')) {
            return false;
        }
        
        while (ip < str.Length && (char.IsDigit(str[ip]) || Equals(str[ip], '.'))) {
            num += str[ip++];
        }

        return true;
    }

    public static bool AnyOf(this string str, string[] target, out string result, int start = 0) {
        result = string.Empty;

        var enumerator = target
                     .Where(x => x[0] == str[start])
                     .GetEnumerator();

        foreach (var el in (IEnumerable<string>)enumerator) {
            var slice = str.Take(new Range(start, start+el.Length));

            var ip = 0;
            result = slice
                 .TakeWhile(ch => ip < el.Length && Equals(ch, el[ip]))
                 .Aggregate(result, (current, _) => current+el[ip++]);
        }
        
        return result.Length != 0;
    }
}