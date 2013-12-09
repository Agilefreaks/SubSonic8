namespace Common.Interfaces
{
    public interface IError
    {
        int Code { get; set; }

        string Message { get; set; }
    }
}