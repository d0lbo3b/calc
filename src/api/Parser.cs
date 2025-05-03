namespace calculator.api;

public abstract class Parser {
    public abstract bool Parse(List<string> input, out string result);
}