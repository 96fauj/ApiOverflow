using Microsoft.AspNetCore.Http;

namespace CsvApi.Helpers
{
    public static class FileHelpers
    {
        public static bool IsCsvFile(this IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".csv" || extension == ".csv");
        }
    }
}
