namespace QRCode.Framework.Formatters
{
    using System.Threading.Tasks;

    public interface IFormatter
    {
        public Task<T> Load<T>(string path);
        public Task Save(object obj, string path);
    }
}