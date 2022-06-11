namespace Maxwell.Mxi.Repls;

public enum ReplResultType
{
    Result,
    Error
}

public enum ReplEvent
{
    Exit,
    Continue,
    None,
}

public struct ReplResult
{
    public String Value { get; }
    public ReplResultType Type { get; }
    public ReplEvent Event { get; }

    public ReplResult(String value, ReplResultType type, ReplEvent ev)
    {
        this.Value = value;
        this.Type = type;
        this.Event = ev;
    }
}