using StudentManagementApi.DTOs;
using StudentManagementApi.Models;

namespace StudentManagementApi.Services;

public interface IStudentService
{
    IReadOnlyList<Student> GetAll(string? search, string? department);

    Student? GetById(int id);

    Student Add(StudentRequestDto request);

    Student? Update(int id, StudentRequestDto request);

    bool Delete(int id);

    bool EnrollmentExists(string enrollmentNo, int? excludeId = null);
}
