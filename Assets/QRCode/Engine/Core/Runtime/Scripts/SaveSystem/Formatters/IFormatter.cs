namespace QRCode.Engine.Core.SaveSystem.Formatters
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IFormatter : IDisposable
    {
        public Task<T> Load<T>(string path);
        public Task Save(object obj, string path);
    }
}