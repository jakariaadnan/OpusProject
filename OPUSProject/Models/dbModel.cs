using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using OPUSProject.Models;

namespace OPUSProject.Models
{
    public class dbModel : DbContext
    {
        public dbModel(DbContextOptions<dbModel> op) : base(op)
        {

        }
        public DbSet<CarDetails> CarDetails { get; set; }
        public DbSet<CustomerDetails> CustomerDetails { get; set; }
        public DbSet<BillInfo> BillInfos { get; set; }
        //////
        public DbSet<CompanyDetails> CompanyDetails { get; set; }
        public DbSet<TypeOfVehicle> TypeOfVehicles { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<OPUSProject.Models.Accessories> Accessories { get; set; }
    }
    public enum color { Red = 1, Black }
    public class CarDetails
    {
        [Key]
        public int Id { get; set; }
        public string Company { get; set; }
        [ForeignKey("TypeOfVehicle")]
        public int TypeOfVehicle { get; set; }
        public string ModelName { get; set; }
        public string ChessisNo { get; set; }
        public string EngineNo { get; set; }

        [ForeignKey("Year")]
        public int ManufactureYear { get; set; }
        public string CC { get; set; }
        [EnumDataType(typeof(color))]
        public color Color { get; set; }
        public string LoadCapacity { get; set; }
        public string Accessories { get; set; }
        public int DeliveryDays { get; set; }
        public decimal Price { get; set; }
        public int OfferDays { get; set; }
        public bool IsAccepted { get; set; }
        public ICollection<BillInfo> BillInfos { get; set; }
        public virtual TypeOfVehicle TypeOfVehicles { get; set; }
        public virtual Year Years { get; set; }

    }
    public class CustomerDetails
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public DateTime Date { get; set; }
        public ICollection<BillInfo> BillInfos { get; set; }

    }
    public enum billType { Cash = 1, Check }
    public class BillInfo
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CustomerDetails")]
        public int CusId { get; set; }
        [ForeignKey("CarDetails")]
        public int CarId { get; set; }
        public int ChallanNo { get; set; }
        public int MoneyReceiptNo { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public billType PaymentType { get; set; }
        public string BankDetails { get; set; }
        public bool IsPaid { get; set; }

        public virtual CarDetails CarDetails { get; set; }
        public virtual CustomerDetails CustomerDetails { get; set; }
    }

    ////////
    public class CompanyDetails
    {
        [Key]
        public int Id { get; set; }
        [Required, DisplayName("Showroom Name")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
    }
    public class TypeOfVehicle
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public ICollection<CarDetails> CarDetails { get; set; }
    }
    public class Year
    {
        [Key]
        public int Id { get; set; }
        public string ManufactureYear { get; set; }
        public ICollection<CarDetails> CarDetails { get; set; }
    }
    public class Accessories
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CarDetails> CarDetails { get; set; }
    }

}
