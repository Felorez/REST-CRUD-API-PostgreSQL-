using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RestApi.Data
{
    public class Product
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public int CompanyId { get; set; }
        public Company? Company { get; set; } = null!;
    }

}
