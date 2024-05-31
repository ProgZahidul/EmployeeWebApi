using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeWebApi.Models;

namespace EmployeeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeInfoesController : ControllerBase
    {
        private readonly EmployeeDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmployeeInfoesController(EmployeeDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/EmployeeInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeInfo>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/EmployeeInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeInfo>> GetEmployeeInfo(int id)
        {
            var employeeInfo = await _context.Employees.FindAsync(id);

            if (employeeInfo == null)
            {
                return NotFound();
            }

            return employeeInfo;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeInfo(int id, EmployeeInfo employeeInfo)
        {
            if (id != employeeInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(employeeInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        //[HttpPost]
        //public async Task<ActionResult<EmployeeInfo>> PostEmployeeInfo([FromForm] EmployeeInfo employeeInfo, IFormFile? image)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    string uniqueFileName = null;

        //    if (image != null)
        //    {
        //        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "employee");
        //        uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await image.CopyToAsync(fileStream);
        //        }

        //        employeeInfo.ImgPath = Path.Combine("images", "employee", uniqueFileName);
        //    }

        //    _context.Employees.Add(employeeInfo);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetEmployeeInfo", new { id = employeeInfo.Id }, employeeInfo);
        //}

        [HttpPost]
        public async Task<ActionResult<EmployeeInfo>> PostEmployeeInfo([FromForm] EmployeeInfo employeeInfo, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Employees.Add(employeeInfo);
            await _context.SaveChangesAsync();

            if (image != null)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "employee");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    employeeInfo.ImgPath = Path.Combine("images", "employee", uniqueFileName);
                    _context.Entry(employeeInfo).Property(e => e.ImgPath).IsModified = true;
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Optionally handle file upload failure (e.g., log error, return a specific response)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Image upload failed.");
                }
            }

            return CreatedAtAction("GetEmployeeInfo", new { id = employeeInfo.Id }, employeeInfo);
        }

        // Assume there's a GetEmployeeInfo method to retrieve an employee by ID
    



    // DELETE: api/EmployeeInfoes/5
    [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeInfo(int id)
        {
            var employeeInfo = await _context.Employees.FindAsync(id);
            if (employeeInfo == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employeeInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeInfoExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
