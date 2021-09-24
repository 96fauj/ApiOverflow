# The Plan

## Api

- Apply correct markdown for this database structural document
- Swagger API Documentation (partly done, needs configuring)
- CsvParser library - lets not reinvent the wheel, use their mapping and validation functionality

- todo: stop returning the good / bad row domain models

## Business layer

- Validation of columns using CsvReader inbuilt validation. It would have been better to use a different validation library instead
so that when testing we don't have to setup streams to read/write - making the tests simpler and decoupled from the reading library

## Database

- Assumption: We only want to be able to enter a single MeterReading for an account that has the same date
(Hence the primary key on the meterreading table is composite of AccountId and Date)

- entity framework in memory database
- seeds the initial data on load of application

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
- Automapper

## TODO
- Review TEntity Repository's TryAttach performance and review if it makes a single call
when adding a range of entities 
- Implement conditionally including (.Include(x => x.Account etc..) related entities on meterrow and account