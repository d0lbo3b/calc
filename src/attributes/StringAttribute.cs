namespace calculator.attributes;

[AttributeUsage(AttributeTargets.Field)]
public class StringAttribute : Attribute {
    public string String { get; }
    
    
    public StringAttribute(string str) {
        String = str;
    }
}