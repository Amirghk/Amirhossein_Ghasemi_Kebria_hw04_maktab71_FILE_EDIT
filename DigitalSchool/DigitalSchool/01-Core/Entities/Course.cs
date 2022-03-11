using Newtonsoft.Json;
public class Course
{
    public readonly int id;
    public readonly String name;
    public readonly Grade grade;

    public List<Student> students;
    public Teacher teacher;
    public int units = 1;
    public Course(int id, string name) // middle school courses
    {
        this.id = id;
        this.name = name;
        grade = Grade.MiddleSchool;
    }

    [JsonConstructor]
    public Course(int id, string name, int units) // high school courses
    {
        this.id = id;
        this.name = name;
        this.units = units;
        grade = Grade.HighSchool;
    }

}



public enum Grade { MiddleSchool = 1, HighSchool = 2};