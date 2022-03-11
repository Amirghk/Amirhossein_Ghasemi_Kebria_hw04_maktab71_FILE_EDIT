using Newtonsoft.Json;
public class Teacher : User
{
    public List<Course> courses;

    public bool validated;
    public int salary = 0;

    [JsonConstructor]
    public Teacher(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
        if ((courses ??= new List<Course>()).Count != 0)
        {
            foreach (var course in courses)
            {
                salary += course.units * 1000000;
            }
        }
    }

    public List<Student> GetStudents(Teacher teacher) // returns students of this teacher
    {
        List<Student> stds = new List<Student>();
        foreach (var course in courses)
        {
            foreach (var student in (course.students ??= new List<Student>()))
                stds.Add(student);
        }
        return stds.Distinct().ToList<Student>(); // to avoid duplicates
    }
}