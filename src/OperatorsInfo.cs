using System.Globalization;
using calculator.api;
using calculator.attributes;
using calculator.extensions;

namespace calculator;

public static class OperatorsInfo {
    public static readonly string[] commands = ["+", "-", "*", "/", "(", ")", "!"];
    public static readonly Dictionary<int, OperatorType[]> priority = new() {
                                                                                             {Iota.GetIota(),           [OperatorType.LeftBracket]},
                                                                                             {Iota.GetIota(),           [OperatorType.Factorial]},
                                                                                             {Iota.GetIota(),           [OperatorType.Negative, OperatorType.Positive]},
                                                                                             {Iota.GetIota(),           [OperatorType.Mul, OperatorType.Div]},
                                                                                             {Iota.GetIota(true), [OperatorType.Minus, OperatorType.Plus]},
                                                                                         };


    public static readonly Operator[] operatorsDesc = [new(
                                                          OperatorType.Plus,
                                                          Calculate,
                                                          CheckAround
                                                       ),
                                                      new(
                                                          OperatorType.Minus,
                                                          Calculate,
                                                          CheckAround
                                                         ),
                                                      new(
                                                          OperatorType.Mul,
                                                          Calculate,
                                                          CheckAround
                                                         ),
                                                      new(
                                                          OperatorType.Div,
                                                          Calculate,
                                                          CheckAround
                                                         ),
                                                      new(
                                                          OperatorType.LeftBracket,
                                                          Bracket,
                                                          (_, _) => true),
                                                      new(
                                                          OperatorType.Positive,
                                                          Positive,
                                                          CheckRight
                                                         ),
                                                      new(
                                                          OperatorType.Negative,
                                                          Negative,
                                                          CheckRight
                                                         ),
                                                      new(
                                                          OperatorType.Factorial,
                                                          Negative,
                                                          CheckLeft
                                                         ),
                                                  ];


    #region Operations
    private static void Calculate(List<string> context, Operator op, out List<string> output) {
        var (a, b) = context.ParseAroundAsDouble(op.Position);
        
        for (var i = 0; i < 3; i++) {
            context.RemoveAt(op.Position-1);
        }
        context.Insert(
                       op.Position-1,
                       Activate(a, b, op.Type).ToString(CultureInfo.CurrentCulture)
                       );
        
        output = context;
    }

    private static double Activate(double a, double b, OperatorType operation) {
        return operation switch {
                   OperatorType.Plus  => a+b,
                   OperatorType.Minus => a-b,
                   OperatorType.Mul   => a*b,
                   OperatorType.Div   => a/b,
                   _                  => -1
               };
    }
    
    private static void Bracket(List<string> context, Operator op, out List<string> output) {
        var (openIndex, closeIndex) = (op.Position, context.IndexOf(AttributeUnwrapper.Unwrap<StringAttribute>(OperatorType.RightBracket)!.String));
        
        context.RemoveAt(closeIndex);
        context.RemoveAt(op.Position);
        
        var slice = context[openIndex..(closeIndex-1)];
        
        var parser = new ExpressionsParser();
        parser.Parse(slice, out var result);

        var appendedWithResult = false;
        var temp = new List<string>();
        
        for (var i = 0; i < context.Count; i++) {
            if (i >= openIndex && !appendedWithResult) {
                temp = [result];
                appendedWithResult = true;
                continue;
            }
            if (Enumerable.Range(openIndex, closeIndex-1).Contains(i)) continue;
            temp.Add(context[i]);
        }

        output = temp;
    }
    private static void Positive(List<string> context, Operator op, out List<string> output) {
        context.RemoveAt(op.Position);
        output = context;
    }
    
    private static void Negative(List<string> context, Operator op, out List<string> output) {
        context.RemoveAt(op.Position);
        context[op.Position] = $"-{context[op.Position]}";
        output = context;
    }
    
    #endregion Operations
    #region Checks
    private static bool CheckAround(List<string> context, Operator op) {
        var check = op.Position > 0 
                    && op.Position < context.Count - 1 
                    && !commands.Contains(context[op.Position-1])
                    && !commands.Contains(context[op.Position+1]);
        return check;
    }
    private static bool CheckRight(List<string> context, Operator op) {
        var check = op.Position < context.Count - 1
                    && (op.Position == 0 || commands.Contains(context[op.Position-1]))
                    && !commands.Contains(context[op.Position+1]);
        return check;
    }
    private static bool CheckLeft(List<string> context, Operator op) {
        var check = op.Position > 0
                    && !commands.Contains(context[op.Position-1])
                    && ((op.Position < context.Count-1 || commands.Contains(context[op.Position+1])) || op.Position == context.Count-1);
        
        return check;
    }
    #endregion
}