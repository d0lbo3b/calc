using calculator.api;
using calculator.attributes;

namespace calculator;

public class ExpressionsParser : Parser {
    public override bool Parse(List<string> context, out string result) {
        result = string.Empty;
        
        foreach (var (_, ops) in OperatorsInfo.priority) {
            for (var i = 0; i < context.Count; i++) {
                foreach (var op in ops) {
                    if (i >= context.Count) break;
                    if (!Equals(context[i], AttributeUnwrapper.Unwrap<StringAttribute>(op)!.String)) continue;
                    foreach (var opDesc in OperatorsInfo.operatorsDesc) {
                        if (!Equals(op, opDesc.Type)) continue;
                        opDesc.AssignPosition(i);
                        if (!opDesc.Check(context, opDesc)) continue;
                        opDesc
                        .Activate
                        .Invoke(context, opDesc, out context);
                    }
                }
            }
        }

        result = context[0];
        return !Equals(context, null);
    }
}