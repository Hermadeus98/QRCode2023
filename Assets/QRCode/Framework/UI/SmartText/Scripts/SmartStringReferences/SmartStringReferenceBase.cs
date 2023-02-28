namespace QRCode.Framework
{
    using Sirenix.OdinInspector;

    public abstract class SmartStringReferenceBase<T> : SerializedScriptableObject
    {
        public T Value;

        protected override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();
            Value = default;
        }
    }
}
