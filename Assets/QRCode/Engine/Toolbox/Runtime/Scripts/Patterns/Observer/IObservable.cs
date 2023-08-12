namespace QRCode.Engine.Toolbox.Pattern.Observable
{
    public interface IObservable
    {
        public void OnNotify();
    }
    
    public interface IObservable<in T>
    {
        public void OnNotify(T arg);
    }
}
