namespace World.Api.DTO.States
{
    public class CreateStatesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // CountryId should match the primary key type of Country (int)
        public int CountryId { get; set; }
    }
}
