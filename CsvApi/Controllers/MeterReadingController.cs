using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvApi.Helpers;
using CsvApp.Business.Interfaces;
using CsvApp.Business.Parsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CsvApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterReadingController : ControllerBase
    {
        public MeterReadingController(IEnergyService energyService)
        {
            _energyService = energyService;
        }

        [HttpPost("meter-reading-uploads", Name = "meter-reading-uploads")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult MeterReadingUploads(
            IFormFile file)
        {
            if (file.IsCsvFile())
            {
                var parser = new MeterRowParser();
                var csvParseResult = parser.ParseCsv(new StreamReader(file.OpenReadStream()));
                var successfulRows = _energyService.AddMeterReadings(csvParseResult.Completed.Values);
                
                var result = new
                {
                    successful = successfulRows,
                    failed = (csvParseResult.SuccessfulCount + csvParseResult.FailedCount) - successfulRows
                };

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

                return Ok(result);
            }
            else
            {
                return BadRequest(new { message = "Invalid file extension" });
            }
        }

        [HttpPost("get-accounts", Name = "get-accounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetAccounts()
        {
            return Ok(_energyService.GetAllAccounts());
        }

        [HttpPost("get-meter-readings", Name = "get-meter-readings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetMeterReadings()
        {
            return Ok(_energyService.GetAllMeterReadings());
        }

        private readonly IEnergyService _energyService;
    }
}