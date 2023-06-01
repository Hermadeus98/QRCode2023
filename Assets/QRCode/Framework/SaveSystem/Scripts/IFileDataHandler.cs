namespace QRCode.Framework
{
    using System.Threading.Tasks;

    public interface IFileDataHandler
    {
        public Task<T> Load<T>();
        public Task Save(object saveData);
        public Task<bool> TryDeleteSave();
    }
}