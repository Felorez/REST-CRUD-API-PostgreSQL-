namespace RestApi.Data
{
    public class UserIIN
    {
        public int? Id { get; set; }
        
        public string? IIN { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
