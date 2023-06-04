using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc;
using Sample.Models;

namespace Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        private readonly IDynamoDBContext _context;
        public StudentsController(IDynamoDBContext context)
        {
            _context = context;
        }
        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetById(string studentId)
        {
            var student = await _context.LoadAsync<Student>(studentId);
            if (student == null) return NotFound();
            return Ok(student);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var student = await _context.ScanAsync<Student>(default).GetRemainingAsync();
            return Ok(student);
        }
        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student studentRequest)
        {
            var student = await _context.LoadAsync<Student>(studentRequest.Id);
            if (student != null) return BadRequest($"Student with Id {studentRequest.Id} Already Exists");
            await _context.SaveAsync(studentRequest);
            return Ok(studentRequest);
        }
        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(string studentId)
        {
            var student = await _context.LoadAsync<Student>(studentId);
            if (student == null) return NotFound();
            await _context.DeleteAsync(student);
            return NoContent();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateStudent(Student studentRequest)
        {
            var student = await _context.LoadAsync<Student>(studentRequest.Id);
            if (student == null) return NotFound();
            await _context.SaveAsync(studentRequest);
            return Ok(studentRequest);
        }
    }
}