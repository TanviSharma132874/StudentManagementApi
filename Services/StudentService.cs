using StudentManagementApi.DTOs;
using StudentManagementApi.Models;

namespace StudentManagementApi.Services;

/// <summary>Simple thread-safe in-memory store (data resets when the app restarts).</summary>
public class StudentService : IStudentService
{
    private readonly List<Student> _students = new();
    private readonly object _lock = new();
    private int _nextId = 1;

    public StudentService()
    {
        // Sample data so that GET returns something on the first run.
        Add(new StudentRequestDto { EnrollmentNo = "92400133001", FullName = "Aarav Mehta", Email = "aarav.mehta@example.com", Department = "Computer Engineering", Semester = 5 });
        Add(new StudentRequestDto { EnrollmentNo = "92400133002", FullName = "Priya Sharma", Email = "priya.sharma@example.com", Department = "Computer Engineering", Semester = 5 });
        Add(new StudentRequestDto { EnrollmentNo = "92400134003", FullName = "Rohan Patel", Email = "rohan.patel@example.com", Department = "Information Technology", Semester = 3 });
        Add(new StudentRequestDto { EnrollmentNo = "92400135004", FullName = "Neha Joshi", Email = "neha.joshi@example.com", Department = "Mechanical Engineering", Semester = 7 });
    }

    public IReadOnlyList<Student> GetAll(string? search, string? department)
    {
        lock (_lock)
        {
            IEnumerable<Student> query = _students;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var text = search.Trim();
                query = query.Where(s =>
                    s.FullName.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                    s.EnrollmentNo.Contains(text, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(department))
            {
                query = query.Where(s =>
                    s.Department.Equals(department.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            return query.OrderBy(s => s.Id).ToList();
        }
    }

    public Student? GetById(int id)
    {
        lock (_lock)
        {
            return _students.FirstOrDefault(s => s.Id == id);
        }
    }

    public Student Add(StudentRequestDto request)
    {
        lock (_lock)
        {
            var student = new Student
            {
                Id = _nextId++,
                EnrollmentNo = request.EnrollmentNo.Trim(),
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                Department = request.Department.Trim(),
                Semester = request.Semester,
                CreatedOn = DateTime.UtcNow
            };

            _students.Add(student);
            return student;
        }
    }

    public Student? Update(int id, StudentRequestDto request)
    {
        lock (_lock)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student is null)
            {
                return null;
            }

            student.EnrollmentNo = request.EnrollmentNo.Trim();
            student.FullName = request.FullName.Trim();
            student.Email = request.Email.Trim().ToLowerInvariant();
            student.Department = request.Department.Trim();
            student.Semester = request.Semester;
            return student;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            return student is not null && _students.Remove(student);
        }
    }

    public bool EnrollmentExists(string enrollmentNo, int? excludeId = null)
    {
        lock (_lock)
        {
            return _students.Any(s =>
                s.EnrollmentNo == enrollmentNo.Trim() && s.Id != excludeId);
        }
    }
}
