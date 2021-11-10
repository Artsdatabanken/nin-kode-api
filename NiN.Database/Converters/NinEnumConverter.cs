namespace NiN.Database.Converters
{
    using System;

    public static class NinEnumConverter
    {
        public static T? Convert<T>(string value) where T : struct
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            if (Enum.TryParse<T>(value, true, out var result))
                return result;
            return null;
        }

        public static string GetValue<T>(T? inputEnum) where T : struct
        {
            return inputEnum.HasValue
                ? Enum.GetName(typeof(T), inputEnum)
                : null;
        }
    }
}
