namespace CsvApp.Business.Config
{
    public class MeterReadConfigSettings
    {
        public string DateFormat { get; set; }
        public string ReadValueRegex { get; set; }
    }

    public static class MeterReadSettings
    {
        public static MeterReadConfigSettings Settings { get; set; }
    }

    public static class MeterReadConfigExtension
    {
        public static void SetupConfigValues(this MeterReadConfigSettings settings)
        {
            MeterReadSettings.Settings = new MeterReadConfigSettings
            {
                DateFormat = settings.DateFormat,
                ReadValueRegex = settings.ReadValueRegex
            };
        }
    }
}