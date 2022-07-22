using EmployeePayrollAjax.Models;
using jQuery_Ajax_CRUD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeePayrollAjax.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly EmployeeDbContext _context;

        public EmployeeController(EmployeeDbContext context)
        {
            _context = context;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        // GET: Transaction/AddOrEdit(Insert)
        // GET: Transaction/AddOrEdit/5(Update)
        [HttpGet]
        public async Task<IActionResult> AddEmployee(int id = 0)
        {
            if (id == 0)
                return View(new EmployeeModel());
            else
            {
                var employeeModel = await _context.Employees.FindAsync(id);
                if (employeeModel == null)
                {
                    return NotFound();
                }
                return View(employeeModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(int id, [Bind("EmployeeId,Name,Gender,Department,Notes")] EmployeeModel employeeModel)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    
                    _context.Add(employeeModel);
                    await _context.SaveChangesAsync();

                }
                //Update9
                else
                {
                    try
                    {
                        _context.Update(employeeModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (EmployeeModelExists(employeeModel.EmployeeId))
                        { return NotFound(); }
                        else
                        { throw; }
                    }
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Employees.ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddEmployee", employeeModel) });
        }

        private bool EmployeeModelExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }

        // GET: Employee/DeleteEmployee
        public async Task<IActionResult> DeleteEmployee(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empl = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (empl == null)
            {
                return NotFound();
            }

            return View(empl);
        }

        // POST: Employee/DeleteEmployee/5
        [HttpPost, ActionName("DeleteEmployee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empl = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(empl);
            await _context.SaveChangesAsync();
            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Employees.ToList()) });
        }

        
    }
}
        


    
