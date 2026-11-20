namespace StudentManagementApi.Models;

public class Student
{
    public int Id { get; set; }

    public string EnrollmentNo { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public int Semester { get; set; }

    public DateTime CreatedOn { get; set; }
}
