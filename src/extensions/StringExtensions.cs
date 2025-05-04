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

        var results = target.Where(x => x[0] == str[start]);
        List<string> enumerable = [];
        
        for (var i = 0; i < str.Length-start; i++) {
            enumerable = results.ToList();
            if (enumerable.Count <= 1) break;
            results = enumerable.Where(x => x[i] == str[start+i]);
        }
        
        result = enumerable.Count > 0? enumerable[0] : string.Empty;
        
        return enumerable.Count > 0;
    }
}