using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.DTOs;
using StudentManagementApi.Models;
using StudentManagementApi.Services;

namespace StudentManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service)
    {
        _service = service;
    }

    // GET: api/students?search=aarav&department=Computer Engineering
    [HttpGet]
    public ActionResult<IEnumerable<Student>> GetAll([FromQuery] string? search, [FromQuery] string? department)
    {
        return Ok(_service.GetAll(search, department));
    }

    // GET: api/students/1
    [HttpGet("{id:int}")]
    public ActionResult<Student> GetById(int id)
    {
        var student = _service.GetById(id);
        return student is null ? NotFoundProblem(id) : Ok(student);
    }

    // POST: api/students
    [HttpPost]
    public ActionResult<Student> Create(StudentRequestDto request)
    {
        // [ApiController] already returns 400 automatically when the DTO is invalid.
        if (_service.EnrollmentExists(request.EnrollmentNo))
        {
            return DuplicateProblem(request.EnrollmentNo);
        }

        var created = _service.Add(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/students/1
    [HttpPut("{id:int}")]
    public ActionResult<Student> Update(int id, StudentRequestDto request)
    {
        if (_service.EnrollmentExists(request.EnrollmentNo, id))
        {
            return DuplicateProblem(request.EnrollmentNo);
        }

        var updated = _service.Update(id, request);
        return updated is null ? NotFoundProblem(id) : Ok(updated);
    }

    // DELETE: api/students/1
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        return _service.Delete(id) ? NoContent() : NotFoundProblem(id);
    }

    // GET: api/students/test-error  (throws on purpose to demonstrate the exception middleware)
    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new InvalidOperationException("Demo exception thrown to test the exception handling middleware.");
    }

    private ObjectResult NotFoundProblem(int id) =>
        Problem(statusCode: StatusCodes.Status404NotFound, title: "Student not found.", detail: $"No student exists with id {id}.");

    private ObjectResult DuplicateProblem(string enrollmentNo) =>
        Problem(statusCode: StatusCodes.Status409Conflict, title: "Duplicate enrollment number.", detail: $"A student with enrollment number {enrollmentNo} already exists.");
}
