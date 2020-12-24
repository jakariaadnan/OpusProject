using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OPUSProject.Models;
using System.ComponentModel.DataAnnotations;
using ceTe.DynamicPDF.HtmlConverter;

namespace OPUSProject.Controllers
{
    public class CarDetailsController : Controller
    {
        private readonly dbModel _context;

        public CarDetailsController(dbModel context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.CarDetails.ToListAsync());
        }
        public async Task<IActionResult> UnAcceptedQ()
        {
            return View(await _context.CarDetails.Where(q=>q.IsAccepted==false).ToListAsync());
        }
        public async Task<IActionResult> Accept(int id)
        {
            var carDetails = await _context.CarDetails.FindAsync(id);

            carDetails.IsAccepted = true;

            _context.Entry(carDetails).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("GetChallan", new { Id = carDetails.Id });
        }
        public ActionResult PrintQuatation(int id)
        {
            try
            {
                var report = Converter.Convert(new Uri("https://localhost:44301/CarDetails/GetQuatation/" + id));
                return File(report, "application/pdf");
            }
            catch (Exception)
            {
                return RedirectToAction("GetQuatation",new { Id=id });
            }
        }
        public ActionResult PrintChallan(int id)
        {
            try
            {
                var report = Converter.Convert(new Uri("https://localhost:44301/CarDetails/GetQuatation/" + id));
                return File(report, "application/pdf");
            }
            catch (Exception)
            {
                return RedirectToAction("GetQuatation", new { Id = id });
            }
        }
        public async Task<ActionResult<IEnumerable<QuatationVM>>> GetQuatation(int id)
        {
            var bill = await (from b in _context.BillInfos
                              join c in _context.CarDetails
                              on b.CarId equals c.Id
                              join cus in _context.CustomerDetails
                              on b.CusId equals cus.Id
                              where b.Id == id
                              select new QuatationVM { 
                              CustomerName=cus.Name,CustomerAddress=cus.Address,TypeOfVehicl= _context.TypeOfVehicles.Where(a=>a.Id==c.TypeOfVehicle).Select(a=>a.Type).ToString(),
                                  ChessisNo=c.ChessisNo,EngineNo=c.EngineNo,ManfYear = _context.Years.Where(a => a.Id == c.ManufactureYear).Select(a => a.ManufactureYear).ToString(),
                                  CC=c.CC,Color=c.Color,Accessorie=c.Accessories,LoadCapacity=c.LoadCapacity,DeliveryDays=c.DeliveryDays,OfferDays=c.OfferDays,Price=c.Price,Date=DateTime.Now
                              }).FirstOrDefaultAsync();
            ViewBag.word= NumberToWords(Convert.ToInt32(bill.Price));
            return View(bill);
        }
        public async Task<ActionResult<IEnumerable<QuatationVM>>> GetChallan(int id)
        {
            var challan = await (from c in _context.CarDetails
                              join b in _context.BillInfos
                              on c.Id equals b.CarId
                              join cus in _context.CustomerDetails
                              on b.CusId equals cus.Id
                              where c.Id == id
                              select new QuatationVM
                              {
                                  ChallanNo=b.ChallanNo,
                                  Date=b.Date,
                                  Accessorie=c.Accessories,
                                  CustomerName=cus.Name,
                                  CustomerAddress=cus.Address,
                                  BillId=b.Id,
                                  PaymentType=b.PaymentType,
                                  BankDetails=b.BankDetails,
                                  MoneyReceiptNo=b.MoneyReceiptNo,
                                  Price=c.Price
                              }).FirstOrDefaultAsync();
            return View(challan);
        }        
        public IActionResult Create()
        {
            var item = _context.Accessories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Name.ToString()
            }).ToList();
            ViewBag.TypeOfVehicle = new SelectList(_context.TypeOfVehicles, "Id", "Type");
            ViewBag.ManufactureYear = new SelectList(_context.Years, "Id", "ManufactureYear");
            var vm = new QuatationVM()
            {
                Accessories = item
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuatationVM VM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var car = new CarDetails()
                    {
                        Company=VM.Company,
                        TypeOfVehicle=Convert.ToInt32( VM.TypeOfVehicle),
                        ModelName=VM.ModelName,
                        ChessisNo=VM.ChessisNo,
                        EngineNo=VM.EngineNo,
                        ManufactureYear= Convert.ToInt32(VM.ManufactureYear),
                        CC=VM.CC,
                        Color=VM.Color,
                        LoadCapacity=VM.LoadCapacity,                        
                        DeliveryDays=VM.DeliveryDays,
                        Price=VM.Price,
                        OfferDays=VM.OfferDays,
                        IsAccepted=false
                    };
                    var acc = VM.Accessories.Where(x => x.Selected).Select(y => y.Value).ToList();
                    car.Accessories = string.Join(", ", acc);                    
                    var cus = new CustomerDetails()
                    {
                        Name=VM.CustomerName,
                        Address=VM.CustomerAddress,
                        ContactNumber=VM.CustomerContactNumber,
                        Date=DateTime.Now
                    };                    
                    var cn = _context.BillInfos.Select(cn=>cn.ChallanNo).Count();
                    var mn=_context.BillInfos.Select(cn => cn.MoneyReceiptNo).Count();
                    var c = 0;
                    var m = 0;
                    if (cn!=0 && mn !=0)
                    {
                        c= _context.BillInfos.Max(c=>c.ChallanNo);
                        m= _context.BillInfos.Max(c => c.MoneyReceiptNo);
                    }
                    else
                    {
                        c = 10000;
                        m = 20000;
                    }
                    var bill = new BillInfo()
                    {
                        ChallanNo = c + 1,
                        MoneyReceiptNo = m + 1,
                        Date = DateTime.Now,
                        PaymentType = VM.PaymentType,
                        BankDetails = VM.BankDetails,
                        IsPaid=false

                    };
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            await _context.CarDetails.AddAsync(car);
                            await _context.SaveChangesAsync();
                            await _context.CustomerDetails.AddAsync(cus);
                            await _context.SaveChangesAsync();
                            int cusId = cus.Id;
                            int carId = car.Id;
                            bill.CarId = carId;
                            bill.CusId = cusId;
                            await _context.BillInfos.AddAsync(bill);
                            await _context.SaveChangesAsync();
                            transaction.Commit();
                            return RedirectToAction("PrintQuatation", new { Id =bill.Id });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }

                    }
                    
                }
                catch(Exception ex)
                {

                    throw ex;
                }
                
            }
            return NoContent();
            //return View(carDetails);
        }
        private string NumberToWords(int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        // GET: CarDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carDetails = await _context.CarDetails.FindAsync(id);
            if (carDetails == null)
            {
                return NotFound();
            }
            return View(carDetails);
        }

        // POST: CarDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Company,TypeOfVehicle,ModelName,ChessisNo,EngineNo,ManufactureYear,CC,LoadCapacity,Accessories,DeliveryDays,Price,OfferDays")] CarDetails carDetails)
        {
            if (id != carDetails.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarDetailsExists(carDetails.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(carDetails);
        }

        // GET: CarDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carDetails = await _context.CarDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carDetails == null)
            {
                return NotFound();
            }

            return View(carDetails);
        }

        // POST: CarDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carDetails = await _context.CarDetails.FindAsync(id);
            _context.CarDetails.Remove(carDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarDetailsExists(int id)
        {
            return _context.CarDetails.Any(e => e.Id == id);
        }
    }
}
