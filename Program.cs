namespace calculator;

internal static class Program {
    private static void Main() {
        var parser = new ExpressionsParser();
        
        while (true) {
            var lexer = new ExpressionLexer(Console.ReadLine() ?? string.Empty);
            var context = new List<string>();
            
            while (lexer.GetNext(out var next)) {
                context.Add(next);
            }
            
            Console.WriteLine(parser.Parse(context, out var result)? result : "Could not parse the expression");
        }
    }
}