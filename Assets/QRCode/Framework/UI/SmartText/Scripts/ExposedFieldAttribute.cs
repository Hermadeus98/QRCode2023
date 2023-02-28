namespace QRCode.Framework
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class ExposedFieldAttribute : Attribute
    {
        public string DisplayName { get; }
        
        public ExposedFieldAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
