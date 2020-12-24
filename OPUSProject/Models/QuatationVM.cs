using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSProject.Models
{
    public class QuatationVM
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public int TypeOfVehicle { get; set; }
        public string TypeOfVehicl { get; set; }
        public string ModelName { get; set; }
        public color Color { get; set; }
        public string ChessisNo { get; set; }
        public string EngineNo { get; set; }
        public DateTime Date { get; set; }
        public int ManufactureYear { get; set; }
        public string ManfYear { get; set; }
        public string CC { get; set; }
        public string LoadCapacity { get; set; }
        public string Accessorie { get; set; }
        public IList<SelectListItem> Accessories { get; set; }
        public int DeliveryDays { get; set; }
        public decimal Price { get; set; }
        public int OfferDays { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerContactNumber { get; set; }
        public billType PaymentType { get; set; }
        public string BankDetails { get; set; }
        public int BillId { get; set; }
        public bool IsPaid { get; set; }
        public bool IsAccepted { get; set; }
        public int ChallanNo { get; set; }
        public int MoneyReceiptNo { get; set; }
    }
}
