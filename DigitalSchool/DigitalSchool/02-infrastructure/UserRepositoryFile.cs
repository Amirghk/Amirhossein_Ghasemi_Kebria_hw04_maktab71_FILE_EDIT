using System.IO;
using Newtonsoft.Json;


public class UserRepositoryFile : IUserRepositiry
{
    string address = @"G:\MAKTAB\WEEK 3";
    public int Authenticate(string email, string password, int role)
    {
            
            switch (role)
        {
            case 0:
                // admin authentication
                if (email == "@" && password == "1234")
                    return -1; // returns -1 if admin is found 
                else
                    return -2;
            case 1:
                // teacher authentication
                if (!File.Exists(address + @"\teacherList.txt"))
                    return -2;

                //FileStream fs = new FileStream(address + @"\teacherList.txt", FileMode.Open, FileAccess.Read);
                //StreamReader sr = new StreamReader(fs);

                //string teachersList = sr.ReadToEnd();
                string teachersList = File.ReadAllText(address + @"\teacherList.txt");
                List<Teacher>? teachers = JsonConvert.DeserializeObject<List<Teacher>>(teachersList);

                foreach (var teacher in teachers)
                {
                    if (teacher.Email == email && teacher.Password == password)
                    {
                        return teachers.IndexOf(teacher);
                    }
                }
                return -2;
            case 2:
                // student authentication
                if (!File.Exists(address + @"\studentList.txt"))
                    return -2;

                string studentsList = File.ReadAllText(address + @"\studentList.txt");
                List<Student>? students = JsonConvert.DeserializeObject<List<Student>>(studentsList);

                foreach (var student in students)
                {
                    if (student.Email == email && student.Password == password)
                        return students.IndexOf(student);
                }
                return -2;
            default:
                return -3; // invalid input role
        }
    } // +

    public Course GetCourse(string name)
    {
        if (!File.Exists(address + @"\courseList.txt"))
            return null;
        string courseL = File.ReadAllText(address + @"\courseList.txt");
        List<Course>? courses = JsonConvert.DeserializeObject<List<Course>>(courseL);
        foreach (var course in courses)
        {
            if (courses == null)
                break;
            else if (course.name == name)
            {
                return course;
            }
        }
        return null;
    } // +

    public List<Course> GetCourses() // +
    {
        // if file doesn't exist make new file
        List<Course>? courses = new List<Course>();
        if (!File.Exists(address + @"\courseList.txt"))
        {
            File.Create(address + @"\courseList.txt");
            courses.Add(new Course(1, "Default"));
            // now make the list into a json file
            string output = JsonConvert.SerializeObject(courses);
            // write the json into file
            File.WriteAllText(address + @"\courseList.txt", output);
            return courses;
        }
        else
        {
            // make the json in file into a list
            string courseL = File.ReadAllText(address + @"\courseList.txt");
            // deserialize json to list
            courses = JsonConvert.DeserializeObject<List<Course>>(courseL);

            return courses;
        }
    }

    public Student GetStudent(string email) // +
    {
        if (!File.Exists(address + @"\studentList.txt"))
            return null;


        string studentL = File.ReadAllText(address + @"\studentList.txt");
        List<Student>? students = JsonConvert.DeserializeObject<List<Student>>(studentL);
        foreach (var student in students)
        {
            if (students == null)
                break;
            else if (student.Email == email)
            {
                return student;
            }
        }
        return null;
    }

    public List<Student> GetStudents()
    {
        // if file doesn't exist make new file
        List<Student>? students = new List<Student>();

        if (!File.Exists(address + @"\studentList.txt"))
        {
            students.Add(new Student("Default", "Default@", "123", 1));
            // now make the list into a json file
            string output = JsonConvert.SerializeObject(students);
            // write the json into file
            File.WriteAllText(address + @"\studentList.txt", output);

            return students;
        }
        else
        {
            // make the json in file into a list
            string studentL = File.ReadAllText(address + @"\studentList.txt");
            // deserialize json to list
            students = JsonConvert.DeserializeObject<List<Student>>(studentL);

            return students;
        }
    } // +

    public Teacher GetTeacher(string email) // +
    {
        if (!File.Exists(address + @"\teacherList.txt"))
            return null;

        string teacherL = File.ReadAllText(address + @"\teacherList.txt");
        List<Teacher>? teachers = JsonConvert.DeserializeObject<List<Teacher>>(teacherL);
        foreach (var teacher in teachers)
        {
            if (teacher.Email == email)
            {
                return teacher;
            }
        }
        return null;       
    }

    public List<Teacher> GetTeachers()
    {
        // if file doesn't exist make new file
        List<Teacher>? teachers = new List<Teacher>();
        if (!File.Exists(address + @"\teacherList.txt"))
        {
            File.Create(address + @"\teacherList.txt");
            teachers.Add(new Teacher("Default", "Default@", "123"));
            // now make the list into a json file
            string output = JsonConvert.SerializeObject(teachers);
            // write the json into file
            File.WriteAllText(address + @"\teacherList.txt", output);
            return teachers;
        }
        else
        {
            // make the json in file into a list
            string teacherL = File.ReadAllText(address + @"\teacherList.txt");
            // deserialize json to list
            teachers = JsonConvert.DeserializeObject<List<Teacher>>(teacherL);

            // close streams
            return teachers;
        }
    } // +

    public void RegisterCourse(Course course)
    {
        List<Course>? courses = new List<Course>();
        if (!File.Exists(address + @"\courseList.txt"))
        {
            File.Create(address + @"\courseList.txt");
            courses.Add(course);// add course
            // now make the list into a json file
            string output = JsonConvert.SerializeObject(courses);
            // write the json into file
            File.WriteAllText(address + @"\courseList.txt", output);
        }
        else
        {
            // make the json in file into a list
            string courseL = File.ReadAllText(address + @"\courseList.txt");
            courses = JsonConvert.DeserializeObject<List<Course>>(courseL);
            // append that course to the list
            courses.Add(course);
            string output = JsonConvert.SerializeObject(courses);
            // write the list back into the file
            File.WriteAllText(address + @"\courseList.txt", output);
        }
    } // +

    public void RegisterStudent(Student student)
    {
        List<Student>? students = new List<Student>();
        if (!File.Exists(address + @"\studentList.txt"))
        {
            File.Create(address + @"\studentList.txt");
            students.Add(student);// add student
            // now make the list into a json file
            string output = JsonConvert.SerializeObject(students);
            // write the json into file
            File.WriteAllText(address + @"\studentList.txt", output);
        }
        else
        {
            // make the json in file into a list
            string studentL = File.ReadAllText(address + @"\studentList.txt");
            students = JsonConvert.DeserializeObject<List<Student>>(studentL);
            // append that student to the list
            students.Add(student);
            string output = JsonConvert.SerializeObject(students);
            // write the list back into the file
            File.WriteAllText(address + @"\studentList.txt", output);
        }
    } // +

    public void RegisterTeacher(Teacher teacher)
    {
        List<Teacher>? teachers = new List<Teacher>();

        if (!File.Exists(address + @"\teacherList.txt"))
        {
            File.Create(address + @"\teacherList.txt");
            teachers.Add(teacher);// add teacher
            // now make the list into a json file
            string output = JsonConvert.SerializeObject(teachers);
            // write the json into file
            File.WriteAllText(address + @"\teacherList.txt", output);
        }
        else
        {
            // make the json in file into a list
            string teacherL = File.ReadAllText(address + @"\teacherList.txt");
            teachers = JsonConvert.DeserializeObject<List<Teacher>>(teacherL);
            // append that teacher to the list
            teachers.Add(teacher);
            string output = JsonConvert.SerializeObject(teachers);
            // write the list back into the file
            File.WriteAllText(address + @"\teacherList.txt", output);
        }
    } // +

    public void SaveChanges(List<Student> students, List<Teacher> teachers, List<Course> courses)
    {
        string output = JsonConvert.SerializeObject(teachers);
        File.WriteAllText(address + @"\teacherList.txt", output);

        output = JsonConvert.SerializeObject(students , new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        });
        File.WriteAllText(address + @"\studentList.txt", output);

        output = JsonConvert.SerializeObject(courses , new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        });
        File.WriteAllText(address + @"\courseList.txt", output);


    } // +

    public void Validate(Teacher teacher)
    {
        // if file exists open file
        if(!File.Exists(address + @"\teacherList.txt")) { throw new Exception("NO TEACHERS HAVE SIGNED UP YET!"); }

        // read file into string

        string tL = File.ReadAllText(address + @"\teacherList.txt");
        // deserialize json into list
        List<Teacher>? teachers = JsonConvert.DeserializeObject<List<Teacher>>(tL);
        // find teacher in list and validate them
        teachers.FirstOrDefault(teacher).validated = true;
        // serialize new list back to json and add back to file
        string output = JsonConvert.SerializeObject(teachers);
        File.WriteAllText(address + @"\teacherList.txt", output);
    } // +

    public void Validate(Student student)
    {
        // if file exists open file
        if (!File.Exists(address + @"\studentList.txt")) { throw new Exception("NO STUDENTS HAVE SIGNED UP YET!"); }
        // read file into string
        string tL = File.ReadAllText(address + @"\studentList.txt");
        // deserialize json into list
        List<Student>? students = JsonConvert.DeserializeObject<List<Student>>(tL);
        // find student in list and validate them
        students.FirstOrDefault(student).validated = true;
        // serialize new list back to json and add back to file
        string output = JsonConvert.SerializeObject(students);
        File.WriteAllText(address + @"\studentList.txt", output);
    } // +
}