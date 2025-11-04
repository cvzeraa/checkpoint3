using System.Data;

public abstract class MessageBase
{
    protected MessageBase(string message)
    {
        Message = message;
        SendAt = DataSetDateTime.Now;
    }

    public string Message { get; }
    public DateTime SendAt { get; }
}