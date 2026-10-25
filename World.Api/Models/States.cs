namespace World.Api.Models
{
    public class States
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // CountryId should match the primary key type of Country (int)
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
