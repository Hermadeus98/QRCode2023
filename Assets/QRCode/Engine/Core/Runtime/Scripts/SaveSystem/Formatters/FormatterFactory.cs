namespace QRCode.Engine.Core.SaveSystem.Formatters
{
    using System;

    public static class FormatterFactory
    {
        public static IFormatter CreateFormatter(FormatterTypeEnum formatterType)
        {
            return formatterType switch
            {
                FormatterTypeEnum.JSON => new JSONFormatter(),
                FormatterTypeEnum.BINARY => new BinaryFormatter(),
                _ => throw new ArgumentOutOfRangeException(nameof(formatterType), formatterType, null)
            };
        }
    }
}