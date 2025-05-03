using System.Text;
using calculator.api;
using calculator.extensions;

namespace calculator;

public class ExpressionLexer : Lexer {
    private int _ip;
    
    
    public ExpressionLexer(string context) : base(context) {
        expression = new string(context
                            .Where(c => !char.IsWhiteSpace(c))
                            .ToArray());
    }
    
    public override bool GetNext(out string next) {
        next = string.Empty;
        
        for (var i = _ip; i < expression.Length; i++) {
            if (expression.FirstNumber(out next, i)) {
                _ip = i+next.Length;
                return true;
            } if (expression.AnyOf(OperatorsInfo.commands, out next, i)) {
                _ip = _ip = i+next.Length;
                return true;
            }
        }
        return false;
    }
}