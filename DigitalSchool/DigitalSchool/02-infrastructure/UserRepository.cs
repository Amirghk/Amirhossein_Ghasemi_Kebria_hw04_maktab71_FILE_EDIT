public class UserRepositrory : IUserRepositiry
{
    public int Authenticate(string email, string password, int role)
    {
        switch (role)
        {
            case 0:
                // admin authentication
                if (DataStore.admin.Email == email && DataStore.admin.Password == password)
                    return -1; // returns -1 if admin is found 
                else
                    return -2;
            case 1:
                // teacher authentication
                foreach (var teacher in DataStore.teachers)
                {
                    if (teacher.Email == email && teacher.Password == password)
                    {
                        return DataStore.teachers.IndexOf(teacher);
                    }
                }
                return -2;
            case 2:
                // student authentication
                foreach (var student in DataStore.students)
                {
                    if (student.Email == email && student.Password == password)
                        return DataStore.students.IndexOf(student);
                }
                return -2;
            default:
                return -3; // invalid input role
        }
    }

    public Course GetCourse(string name)
    {
        return DataStore.courses.FirstOrDefault(n => n.name == name);
    }

    public List<Course> GetCourses()
    {
        return DataStore.courses;
    }

    public Student GetStudent(string email)
    {
        return DataStore.students.FirstOrDefault(n => n.Email == email);
    }

    public List<Student> GetStudents()
    {
        return DataStore.students;
    }

    public Teacher GetTeacher(string email)
    {
        return DataStore.teachers.FirstOrDefault(n => n.Email == email);
    }

    public List<Teacher> GetTeachers()
    {
        return DataStore.teachers;
    }

    public void RegisterCourse(Course course)
    {
        DataStore.courses.Add(course);
    }

    public void RegisterStudent(Student student)
    {
        DataStore.students.Add(student);
    }

    public void RegisterTeacher(Teacher teacher)
    {
        DataStore.teachers.Add(teacher);
    }

    public void Validate(Teacher teacher)
    {
        DataStore.teachers.FirstOrDefault(teacher).validated = true;
    }

    public void Validate(Student student)
    {
        DataStore.students.FirstOrDefault(student).validated = true;
    }

    public void SaveChanges(List<Student> students, List<Teacher> teachers, List<Course> courses)
    {
        DataStore.students = students;
        DataStore.teachers = teachers;       
        DataStore.courses = courses;
    }
}