namespace QRCode.Framework.Formatters
{
    using System;

    public static class FormatterFactory
    {
        public static IFormatter CreateFormatter(FormatterTypeEnum formatterType)
        {
            switch (formatterType)
            {
                case FormatterTypeEnum.JSON:
                    return new JSONFormatter(); 
                case FormatterTypeEnum.BYTES:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(formatterType), formatterType, null);
            }

            return null;
        }
    }
}