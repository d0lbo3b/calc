using System.Globalization;
using calculator.api;
using calculator.attributes;

namespace calculator;

public static class OperatorsInfo {
    public static readonly string[] commands = ["+", "-", "*", "/", "%",  "(", ")", "!", "sin", "cos", "tan", "sqrt", "^"];
    public static readonly Dictionary<int, OperationType[]> priority = new() {
                                                                                             {Iota.GetIota(),
                                                                                                 [OperationType.LeftBracket]},
                                                                                             {Iota.GetIota(),
                                                                                                 [OperationType.Factorial]},
                                                                                             {Iota.GetIota(),
                                                                                                 [OperationType.Negative, OperationType.Positive]},
                                                                                             {Iota.GetIota(),
                                                                                                 [OperationType.Sin, OperationType.Cos, OperationType.Tan]},
                                                                                             {Iota.GetIota(),
                                                                                                 [OperationType.Mul, OperationType.Div, OperationType.Sqrt, OperationType.Pow, OperationType.Mod]},
                                                                                             {Iota.GetIota(true),
                                                                                                 [OperationType.Minus, OperationType.Plus]},
                                                                                         };

    public static readonly Operation[] operationsDesc = [
                                                            new(
                                                             OperationType.Plus,
                                                             OperationScope.LeftAndRight,
                                                             Calculate,
                                                             Check
                                                            ),
                                                            new(
                                                                OperationType.Minus,
                                                                OperationScope.LeftAndRight,
                                                                Calculate,
                                                                Check
                                                               ),
                                                            new(
                                                                OperationType.Mul,
                                                                OperationScope.LeftAndRight,
                                                                Calculate,
                                                                Check
                                                               ),
                                                            new(
                                                                OperationType.Div,
                                                                OperationScope.LeftAndRight,
                                                                Calculate,
                                                                Check
                                                               ),
                                                            new(
                                                                OperationType.LeftBracket,
                                                                OperationScope.Right,
                                                                Calculate,
                                                                (_, _) => true,
                                                                true
                                                                ),
                                                            new(
                                                                OperationType.Positive,
                                                                OperationScope.Right,
                                                                Calculate,
                                                                Check
                                                               ),
                                                            new(
                                                                OperationType.Negative,
                                                                OperationScope.Right,
                                                                Calculate,
                                                                Check
                                                               ),
                                                            new(
                                                                OperationType.Factorial,
                                                                OperationScope.Left,
                                                                Calculate,
                                                                Check
                                                               ),
                                                            new(
                                                                OperationType.Sin,
                                                                OperationScope.Right,
                                                                Calculate,
                                                                Check
                                                               ),
                                                            new(
                                                                OperationType.Cos,
                                                                OperationScope.Right,
                                                                Calculate,
                                                                Check
                                                               ),
                                                            new(
                                                                OperationType.Tan,
                                                                OperationScope.Right,
                                                                Calculate,
                                                                Check
                                                               ),
                                                            new(
                                                                OperationType.Sqrt,
                                                                OperationScope.Right,
                                                                Calculate,
                                                                Check
                                                               ),
                                                            new(
                                                                OperationType.Pow,
                                                                OperationScope.LeftAndRight,
                                                                Calculate,
                                                                Check
                                                               ),
                                                            new(
                                                                OperationType.Mod,
                                                                OperationScope.LeftAndRight,
                                                                Calculate,
                                                                Check
                                                               ),
                                                        ];


    #region Operations
    private static void Calculate(List<string> context, Operation op, out List<string> output) {
        if (op.IsSpecial) {
            ActivateSpecial(context, op, out var temp);
            output = temp;
            return;
        }
        
        var (a, b) = Handle(context, op);
        var result = Activate(a, b, op);
        
        Insert(result, context, op, out var tempOut);
        
        output = tempOut;
    }
    
    private static double Activate(double a, double b, Operation op) {
        return op.Scope switch {
                   OperationScope.Left => op.Type switch {
                                              OperationType.Factorial => MathD.Factorial((long)a),
                                              _                       => throw new ArgumentOutOfRangeException(nameof(op.Type), op.Type, null)
                                          },
                   OperationScope.LeftAndRight => op.Type switch {
                                                      OperationType.Plus  => a+b,
                                                      OperationType.Minus => a-b,
                                                      OperationType.Mul   => a*b,
                                                      OperationType.Div   => a/b,
                                                      OperationType.Mod   => a%b,
                                                      OperationType.Pow   => Math.Pow(a,b),
                                                      _                   => throw new ArgumentOutOfRangeException(nameof(op.Type), op.Type, null)
                                                  },
                   OperationScope.Right => op.Type switch {
                                               OperationType.Positive    => b,
                                               OperationType.Negative    => -b,
                                               OperationType.Sin         => MathD.Sin(b),
                                               OperationType.Cos         => MathD.Cos(b),
                                               OperationType.Tan         => MathD.Tan(b),
                                               OperationType.Sqrt        => Math.Sqrt(b),
                                               _                         => throw new ArgumentOutOfRangeException(nameof(op.Type), op.Type, null)
                                           },
                   _ => throw new ArgumentOutOfRangeException(nameof(op.Scope), op.Scope, null)
               };
    }
    
    private static void ActivateSpecial(List<string> context, Operation op, out List<string> output) {
        output = [];
        
        switch (op.Type) {
            case OperationType.LeftBracket: {
                Brackets(context, op, out var temp);
                output = temp;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static (double a, double b) Handle(List<string> context, Operation op) {
        (double a, double b) = (-1, -1);
        switch (op.Scope) {
            case OperationScope.Left: {
                context.RemoveAt(op.Position);
                a = double.Parse(context[op.Position-1]);
                context.RemoveAt(op.Position-1);
                break;
            }
            case OperationScope.LeftAndRight: {
                (a, b) = (double.Parse(context[op.Position-1]), double.Parse(context[op.Position+1]));
                for (var i = 0; i < 3; i++) {
                    context.RemoveAt(op.Position-1);
                }
                break;
            }
            case OperationScope.Right:
                context.RemoveAt(op.Position);
                b = double.Parse(context[op.Position]);
                context.RemoveAt(op.Position);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        return (a, b);
    }

    private static void Insert(double result, List<string> context, Operation op, out List<string> output) {
        switch (op.Scope) {
            case OperationScope.Left:
            case OperationScope.LeftAndRight: {
                context.Insert(
                               op.Position-1,
                               result.ToString(CultureInfo.CurrentCulture)
                              );
                break;
            }
            case OperationScope.Right:
                context.Insert(
                               op.Position,
                               result.ToString(CultureInfo.CurrentCulture)
                              );
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        output = context;
    }
    
    #region Special

    private static void Brackets(List<string> context, Operation op, out List<string> output) {
        // TODO implement nested brackets 
        var (openIndex, closeIndex) = (op.Position, context.IndexOf(AttributeUnwrapper.Unwrap<StringAttribute>(OperationType.RightBracket)!.String));
        
        context.RemoveAt(closeIndex);
        context.RemoveAt(op.Position);
        
        var slice = context[openIndex..(closeIndex-1)];
        
        var parser = new ExpressionsParser();
        parser.Parse(slice, out var result);

        var appendedWithResult = false;
        var temp = new List<string>();
        
        for (var i = 0; i < context.Count; i++) {
            if (i >= openIndex && !appendedWithResult) {
                temp.Add(result);
                appendedWithResult = true;
                continue;
            }
            if (i >= openIndex && i <= closeIndex) continue;
            temp.Add(context[i]);
        }

        output = temp;
    }

    #endregion
    
    #endregion Operations
    
    #region Checks

    private static bool Check(List<string> context, Operation op) {
        return op.Scope switch {
                   OperationScope.Left         => CheckLeft(context, op),
                   OperationScope.LeftAndRight => CheckAround(context, op),
                   OperationScope.Right        => CheckRight(context, op),
                   _                           => false
               };
    }
    private static bool CheckAround(List<string> context, Operation op) {
        var check = op.Position > 0 
                    && op.Position < context.Count - 1 
                    && !commands.Contains(context[op.Position-1])
                    && !commands.Contains(context[op.Position+1]);
        return check;
    }
    private static bool CheckRight(List<string> context, Operation op) {
        var check = op.Position < context.Count - 1
                    && (op.Position == 0 || commands.Contains(context[op.Position-1]))
                    && !commands.Contains(context[op.Position+1]);
        return check;
    }
    private static bool CheckLeft(List<string> context, Operation op) {
        var check = op.Position > 0
                    && !commands.Contains(context[op.Position-1])
                    && op.Position == context.Count-1 || (op.Position < context.Count-1 && commands.Contains(context[op.Position+1]));
        
        return check;
    }
    #endregion
}