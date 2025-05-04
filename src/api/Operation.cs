using calculator.attributes;

namespace calculator.api;

public enum OperationType {
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
    [String("sin")]
    Sin = 9,
    [String("cos")]
    Cos = 10,
    [String("tan")]
    Tan = 11,
    [String("sqrt")]
    Sqrt = 12,
    [String("^")]
    Pow = 13,
    [String("%")]
    Mod = 14
}

public enum OperationScope {
    Left,
    Right,
    LeftAndRight,
}

public delegate void OperationHandler(List<string> context, Operation op, out List<string> output);
public delegate bool ValidationHandler(List<string> context, Operation op);

public class Operation {
    public OperationType Type { get; }
    public OperationHandler Activate { get; }
    public ValidationHandler Check { get; }
    public OperationScope Scope { get; }
    public bool IsSpecial { get; }
    public int Position { get; private set; }
    

    public Operation(OperationType type, OperationScope scope, OperationHandler activate, ValidationHandler check, bool isSpecial = false) {
        Type = type;
        Scope = scope;
        Activate = activate;
        Check = check;
        IsSpecial = isSpecial;
    }

    public void AssignPosition(int position) {
        Position = position;
    }
}