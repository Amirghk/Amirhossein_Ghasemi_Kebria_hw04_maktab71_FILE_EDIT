using Newtonsoft.Json;
public class Student : User
{
    public Grade grade;

    public bool validated;

    public Dictionary<string, int> scores; // where each course and score are saved

    public List<Course> courses;
    
    [JsonConstructor]
    public Student(string name, string email, string password, int grade)
    {
        Name = name;
        Email = email;
        Password = password;
        if (grade == 1)
            this.grade = Grade.MiddleSchool;
        else
            this.grade = Grade.HighSchool;
    }

    public void ShowScores()
    {
        double sum = 0;
        int units = 0;
        int i = 0;
        foreach (var entry in (scores ??= new Dictionary<string, int>()))
        {
            Console.WriteLine($"{entry.Key} -- {entry.Value}");
            sum += courses[i].units * entry.Value;
            units += courses[i].units;
            i++;
        }
        if (sum == 0 && units == 0)
            Console.WriteLine("No scores yet!");
        else
            Console.WriteLine($"Average -- {(double) sum / units}");
    }

    
}