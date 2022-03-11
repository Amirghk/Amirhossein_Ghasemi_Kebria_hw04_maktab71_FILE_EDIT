public static class DataStore
{    
    public static List<Course> courses = new List<Course> 
    { 
        new Course(1, "chemistry", 3),
        new Course(2, "physics", 4),
        new Course(3, "farsi") 
    };
    public static Admin admin = new Admin();

    public static List<Teacher> teachers = new List<Teacher>
    {
        new Teacher("tom", "tom@", "123")
    };
    public static List<Student> students = new List<Student>
    {
        new Student("bob", "bob@", "123", 2),
        new Student("jack", "jack@", "123", 1)
    };

}