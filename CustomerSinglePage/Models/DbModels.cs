using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerSinglePage.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; } = default!;
        [Required, StringLength(150)]
        public string Address { get; set; } = default!;
        [Required, Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseDate { get; set; }
        [Required, StringLength(50)]
        public string Phone { get; set; } = default!;
        [Required, StringLength(50)]
        public string CustomerType { get; set; } = default!;
        
        [Required, Column(TypeName = "money")]
        public decimal CreditLimit { get; set; }
        public string? Photo { get; set; } = default!;
        //nev
        public virtual ICollection<DeliveryAddress> DeliveryAddresses { get; set; } = new List<DeliveryAddress>();
    }
    public class DeliveryAddress
    {
        public int DeliveryAddressId { get; set; }
        [Required, ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [Required]
        public string ContactPerson { get; set; } = default!;
        [Required, Phone]
        public string Phone { get; set; } = default!;
        public string Address { get; set; } = default!;
        [Column(TypeName = "money")]
        public decimal DueAmount { get; set; } = 0;
        public virtual Customer? Customer { get; set; }

    }

    public class Dealer
    {
        public int DealerId { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public decimal DueAmount { get; set; }
        public int MarketingId { get; set; } // Foreign key
        public Marketing? Marketing { get; set; } // Navigation property
        public DateTime Date { get; set; } = DateTime.Now;
    }
    public class Marketing
    {
        public int MarketingId { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Color { get; set; }
        public ICollection<Dealer>? Dealers { get; set; }
    }

    public class Shop
    {
        public int Id { get; set; }

        [Required]
        public string ShopName { get; set; } = "";
        public string OwnerName { get; set; } = "";

        public string Phone { get; set; } = "";

        public string Address { get; set; } = "";
        public DateTime Date { get; set; }
        // 🔥 Foreign Key
        public int MarketingId { get; set; }

        // 🔥 Navigation Property
        public Marketing? Marketing { get; set; }
    }
  
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DeliveryAddress> DeliveryAddresses { get; set; }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Marketing> Marketings { get; set; }
        public DbSet<Shop> Shops { get; set; }
        
    }
}
