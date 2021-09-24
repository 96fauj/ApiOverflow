namespace CsvApp.Business.Interfaces
{
    public interface IAccount : IUniqueCsvEntity
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}