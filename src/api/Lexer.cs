namespace calculator.api;

public abstract class Lexer {
    protected string expression;
    
    
    protected Lexer(string context) {
        expression = context;
    }

    public abstract bool GetNext(out string next);
}