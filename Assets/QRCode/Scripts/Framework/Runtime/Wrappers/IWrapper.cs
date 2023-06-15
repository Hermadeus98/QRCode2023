namespace QRCode.Wrappers
{
    public interface IWrapper<out T>
    {
        T Arg { get; }
    }

    public interface IWrapper<out T, out U>
    {
        T Arg1 { get; }
        U Arg2 { get; }
    }
    
    public interface IWrapper<out T, out U, out V>
    {
        T Arg1 { get; }
        U Arg2 { get; }
        V Arg3 { get; }
    }
}
