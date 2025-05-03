namespace calculator.extensions;

public static class ListExtensions {
    public static (double, double) ParseAroundAsDouble(this List<string> list, int position) {
        return (double.Parse(list[position-1]), double.Parse(list[position+1]));
    }

    public static void Write<T>(this List<T> list) {
        foreach (var el in list) {
            Console.Write($"{el} ");
        }
    }
}