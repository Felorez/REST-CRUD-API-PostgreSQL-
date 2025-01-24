namespace RestApi.Data
{
    public class User
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
        public int? Age { get; set; }

        public UserIIN? UserIIN { get; set; }    
    }
}
