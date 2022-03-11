public interface IUserRepositiry
{
    public List<Teacher> GetTeachers();
    public List<Student> GetStudents();
    public List<Course> GetCourses();
    public Student GetStudent(string email);
    public Teacher GetTeacher(string email);
    public Course GetCourse(string name);
    public int Authenticate(string email, string password, int role);
    public void RegisterCourse(Course course);
    public void RegisterStudent(Student student);
    public void RegisterTeacher(Teacher teacher);
    public void Validate(Teacher teacher);
    public void Validate(Student student);


    //Save changes
    public void SaveChanges(List<Student> students, List<Teacher> teachers, List<Course> courses);
    
}
