using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CsvApi.Helpers;
using CsvApp.Business;
using CsvApp.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CsvApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterReadingController : ControllerBase
    {
        //public MeterReadingController(IOptions<MeterReadConfigSettings> configSettings)
        //{
        //    _meterReadConfigSettings = configSettings.Value;
        //}

        [HttpPost("meter-reading-uploads", Name = "meter-reading-uploads")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult MeterReadingUploads(
            IFormFile file)
        {
            if (file.IsCsvFile())
            {
                var parser = new MeterRowParser();
                var result = parser.ParseCsv(new StreamReader(file.OpenReadStream()));
                // PersistValidRows(result.GoodRows);

                return Ok(result);
            }
            else
            {
                return BadRequest(new { message = "Invalid file extension" });
            }
        }

        [HttpPost("account-csv-parse", Name = "account-csv-parse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult AccountCsvParse(
            IFormFile file)
        {
            if (file.IsCsvFile())
            {
                var parser = new AccountRowParser();
                var result = parser.ParseCsv(new StreamReader(file.OpenReadStream()));
                // PersistValidRows(result.GoodRows);

                return Ok(result);
            }
            else
            {
                return BadRequest(new { message = "Invalid file extension" });
            }
        }

        private void PersistValidRows(IEnumerable<MeterRow> resultGoodRows)
        {
            throw new System.NotImplementedException();
        }

        private static readonly string[] Summaries = new[]
        {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

        //private readonly MeterReadConfigSettings _meterReadConfigSettings;

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return Summaries;
        }
    }
}