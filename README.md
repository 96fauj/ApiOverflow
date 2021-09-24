# ApiOverflow / CsvApi

## Requirements
- Visual Studio 2019/2022
- .Net Core 3.1
- Set CsvApi app to startup
- Site should automatically navigate to localhost:port/swagger 

## User Story // Criteria
As an Energy Company Account Manager, I want to be able to 
load a CSV file of Customer Meter Readings So that we can 
monitor their energy consumption and charge them accordingl

#### MUST HAVE
- Create the following endpoint: POST => /meter-reading-uploads
- The endpoint should be able to process a CSV of meter readings. An example CSV file has 
been provided (Meter_reading.csv)
- Each entry in the CSV should be validated and if valid, stored in a DB.
- After processing, the number of  successful/failed readings should be returned.
#### ACCEPTANCE CRITERIA
Validation: 
- You should not be able to load the same entry twice
- A meter reading must be associated with an Account ID to be deemed valid
- Reading values should be in the format NNNNN

## Api

- WebApi 2 with a Swagger Ui for testing and consuming the api

## Business layer

- Re-usable implementation of CsvParser for records that need to be unique with 2 concrete implementations
one for accounts and one for meter readings (reflection used for re-usability CsvIdentifierAttribute)
- Validation of columns using CsvReader inbuilt validation. It would have been better to use a different validation library instead
so that when testing we don't have to setup streams to read/write - making the tests simpler and decoupled from the reading library

## Database

- Assumption: We only want to be able to enter a single MeterReading for an account that has the same date
(Hence the primary key on the meterreading table is composite of AccountId and Date)
- Entity Framework (InMemory implementation)
- Service pattern used so that we can switch between different databases
- Seeds the initial data on load of application by reading/parsing from CSV the same way we parse through webapi
- Repository over Entity Framework database context to allow continuation when bad data is provided

## Further development

- Correct usage of cancellation token on meter-reading-uploads endpoint 
so that the user can cancel/abort and the background operation disposes gracefully. 
- Logging so that we can view the logs in something visual instead of relying on App logs
- Consider if some/all api calls can be made asynchronous and if they provide
a performance benefit 
- On the csv output a proper validation error return to the consumer (e.g. the rawRecord string & 
what value was not parsed or failed validation)
- Meaningful EF error representation bubble up to the user (api)
- Decouple the ef db context (data layer) from the web api completely
avoiding services.AddDbContext<EnergyDbContext>(....
- Api level exception filtering, don't want to be returning a full call stack to the consumer 
when something goes wrong
- Review TEntity Repository's TryAttach performance and review if it makes a single call
when adding a range of entities 
- Implement conditionally including (.Include(x => x.Account etc..) related entities on meterrow and account
