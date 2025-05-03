namespace calculator.attributes;

public static class AttributeUnwrapper {
    public static TA? Unwrap<TA>(Enum enumVal) {
        var type = enumVal.GetType();
        var memInfos = type.GetMember(enumVal.ToString());
        var attributes = memInfos[0].GetCustomAttributes(typeof(TA), false);
        
        return attributes.Length > 0 ? (TA)attributes[0] : default;
    }

}