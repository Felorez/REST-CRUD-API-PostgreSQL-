using RestApi.Data;

namespace RestApi.Function
{
    public class GenerateIIN
    {
        public static string GenerateNumberUser(User user)
        {
            var random = new Random();
            var randomPart = random.Next(100000, 999999);
            return $"{user.City.Length * (randomPart%100)}{user.Name.Length * (randomPart % 100)}{user.Id}{randomPart}";
        }
    }
}
