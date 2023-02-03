namespace QRCode.Framework.Observer
{
    public interface IObservable<in T>
    {
        public void OnNotify(T arg);
    }
}
