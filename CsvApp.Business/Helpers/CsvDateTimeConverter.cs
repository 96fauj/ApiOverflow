using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace CsvApp.Business.Helpers
{
    [Obsolete("No longer required as we only want to support a single date time format")]
    public class CsvDateTimeConverter : DateTimeConverter
    {
        private readonly string _dateFormat;

        public CsvDateTimeConverter(string dateFormat)
        {
            this._dateFormat = dateFormat;
        }

        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            DateTime value;

            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            var validDate = DateTime.TryParseExact(text, 
                this._dateFormat, 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.AdjustToUniversal, 
                out value);

            return value;
        }
    }
}
