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
        public MeterReadingController(IOptions<MeterReadConfigSettings> configSettings)
        {
            _meterReadConfigSettings = configSettings.Value;
        }

        //Each entry in the CSV should be validated and if
        //valid, stored in a DB.

        [HttpPost("meter-reading-uploads", Name = "meter-reading-uploads")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult MeterReadingUploads(
            IFormFile file)
        {
            if (file.IsCsvFile())
            {
                //await ParseCsvFile(file);
                var parser = new MeterRowParse(new StreamReader(file.OpenReadStream()), _meterReadConfigSettings);
                var result = parser.ParseCsv();
                return Ok(result);
            }
            else
            {
                return BadRequest(new { message = "Invalid file extension" });
            }
        }

        private static readonly string[] Summaries = new[]
        {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

        private readonly MeterReadConfigSettings _meterReadConfigSettings;

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return Summaries;
        }
    }
}