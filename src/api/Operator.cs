using calculator.attributes;

namespace calculator.api;

public enum OperatorType {
    [String("+")]
    Plus = 0,
    [String("-")]
    Minus = 1,
    [String("*")]
    Mul = 2,
    [String("/")]
    Div = 3,
    [String("(")]
    LeftBracket = 4,
    [String(")")]
    RightBracket = 5,
    [String("+")]
    Positive = 6,
    [String("-")]
    Negative = 7,
    [String("!")]
    Factorial = 8,
}

public delegate void OperationHandler(List<string> context, Operator op, out List<string> output);
public delegate bool ValidationHandler(List<string> context, Operator op);

public class Operator {
    public OperatorType Type { get; }
    public OperationHandler Activate { get; }
    public ValidationHandler Check { get; }
    public int Position { get; private set; }
    

    public Operator(OperatorType type, OperationHandler activate, ValidationHandler check) {
        Type = type;
        Activate = activate;
        Check = check;
    }

    public void AssignPosition(int position) {
        Position = position;
    }
}