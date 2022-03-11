using System.Collections;
using Newtonsoft.Json;

IUserRepositiry _UserRepo = new UserRepositoryFile();

List<Teacher> teachers = _UserRepo.GetTeachers();
List<Student> students = _UserRepo.GetStudents();
List<Course> courses = _UserRepo.GetCourses();
if (teachers == null)
    teachers = new List<Teacher>();
if (students == null)
    students = new List<Student>();
if (courses == null)
    courses = new List<Course>();

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    Formatting = Newtonsoft.Json.Formatting.Indented,
    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
};

string userName;
int auth;
string[] credentials;
bool con = true;
while (con)
{
    bool back = false;
    Console.Clear();
    Console.WriteLine("-----------------Welcome-----------------");
    Console.WriteLine("Please select your role: ");

    Console.WriteLine("1) Admin.");
    Console.WriteLine("2) Teacher.");
    Console.WriteLine("3) Student.");
    Console.WriteLine("4) Exit.");
    var answer = Console.ReadKey(true);

    switch (answer.Key)
    {
        case ConsoleKey.D1: // if user is an admin

            credentials = Login();
            auth = _UserRepo.Authenticate(credentials[0], credentials[1], 0);
            if (auth == -1) // if admin is found
            {
                AdminMenu(DataStore.admin);
                _UserRepo.SaveChanges(students, teachers, courses);
                back = true;
            } 
                
                
            else
                Console.WriteLine("Invalid email/password!");
            break;
        case ConsoleKey.D2: // if user is a teacher
            Console.WriteLine("Have you signed up before? (Y/N)");
            var choice = Console.ReadKey(true);
            switch (choice.Key)
            {
                case ConsoleKey.Y:// checks to see if teacher's credentials match records 
                    credentials = Login();
                    auth = _UserRepo.Authenticate(credentials[0], credentials[1], 1);
                    if (auth == -2)
                    {
                        Console.WriteLine("Username/Password is incorrect!");
                        break;
                    }
                    else if (!teachers[auth].validated) // if admin hasn't validated teacher yet
                    {
                        Console.WriteLine("You haven't been validated by the admin yet!");
                        break;
                    }   
                    TeacherMenu(teachers[auth]);
                    back = true;
                    break;
                default: // if teacher hasn's signed up already
                    credentials = SignUp(false);
                    Teacher tmp = new Teacher(credentials[0], credentials[1], credentials[2]);
  
                    teachers.Add(tmp);
                    break;                                
            }
            _UserRepo.SaveChanges(students, teachers, courses);
            break;
        case ConsoleKey.D3:
            // TODO : make that sign up / login process a method
            Console.WriteLine("Have you signed up before? (Y/N)");
            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.Y:// checks to see if student's credentials match records 
                    credentials = Login();
                    auth = _UserRepo.Authenticate(credentials[0], credentials[1], 2);
                    if (auth == -2)
                    {
                        Console.WriteLine("Username/Password is incorrect!");
                        break;
                    }
                    else if (!students[auth].validated) // if admin hasn't validated student yet
                    {
                        Console.WriteLine("You haven't been validated by the admin yet!");
                        break;
                    }   
                    if (students[auth].courses == null)
                    {
                        SelectCourse(auth);
                    }
                    StudentMenu(students[auth]);
                    back = true;
                    break;
                default: // if student hasn's signed up already
                    credentials = SignUp(true);
                    
                    Student tmp = new Student(credentials[0], credentials[1], credentials[2], Convert.ToInt32(credentials[3]));
                    students.Add(tmp);
                    Console.WriteLine("Select the id of the course you are currently in: ");
                    SelectCourse(students.IndexOf(tmp));
                    break;                                
            }
            _UserRepo.SaveChanges(students, teachers, courses);
            break;
        case ConsoleKey.D4:
            con = false;
            back = true;
            break;
    }
    if (!back)
    {
        Console.WriteLine("Press any key to continue");
        Console.ReadKey(true);
    }
    teachers = _UserRepo.GetTeachers();
    students = _UserRepo.GetStudents();
    courses = _UserRepo.GetCourses();
}




void SelectCourse(int idx)
{
    int id = 0;
    if (students[idx].grade == Grade.MiddleSchool)
    {
        Console.WriteLine("Middle school courses: ");
        foreach (var course in courses)
        {
            if (course.grade == Grade.MiddleSchool)
                Console.WriteLine($"- {course.name} -- id: {course.id}");
        }
        id = Convert.ToInt32(Console.ReadLine());
    }
    else
    {
        Console.WriteLine("High school courses: ");
        foreach (var course in courses)
        {
            if (course.grade == Grade.HighSchool)
                Console.WriteLine($"- {course.name} -- id: {course.id}");
        }
        id = Convert.ToInt32(Console.ReadLine());
    }

    if (id != 0)
    {
        foreach (var course in courses)
        {
            if (course.id == id)
            {
                (course.students ??= new List<Student>()).Add(students[idx]); // add a student to the course // TODO : Maybe add course to student asweell
                (students[idx].courses ??= new List<Course>()).Add(course);
                Console.WriteLine($"course {course.name} added successfully!");
            }
        }
    }
    _UserRepo.SaveChanges(students, teachers, courses);

}

string[] SignUp(bool std)
{
    string grade;
    string pass;
    string passwordConfirmation;
    Console.WriteLine("Enter your name: ");
    string name = Console.ReadLine();

    Console.WriteLine("Please enter the email you wish to sign up with: ");
    string mail = Console.ReadLine();
    do
    {
        Console.WriteLine("Enter your password: ");
        pass = Console.ReadLine();

        Console.WriteLine("Enter your password again: ");
        passwordConfirmation = Console.ReadLine();

    } while (pass != passwordConfirmation);
    if (std)
    {
        Console.WriteLine("What grade are you in? (1/2)");
        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.D1)
            grade = "1";
        else
            grade = "2";
        return new string[] { name, mail, pass, grade};
    }
    _UserRepo.SaveChanges(students, teachers, courses);
    teachers = _UserRepo.GetTeachers();
    students = _UserRepo.GetStudents();
    courses = _UserRepo.GetCourses();

    return new string[] { name, mail, pass };
}



string[] Login()
{
    string password;
    string email;

    Console.WriteLine("Enter your Email: ");
    email = Console.ReadLine();

    Console.WriteLine("Enter your Password: ");
    password = Console.ReadLine();

    return new string[] { email, password };
}

void AdminMenu(Admin admin)
{
    bool found;
    bool con = true;
    string answer;
    Course course = new Course(0, "null", 1);
    while (con)
    {
        Console.Clear();
        Console.WriteLine("1. Add a course.");
        Console.WriteLine("2. Validate registers.");
        Console.WriteLine("3. Assign courses to teachers.");
        Console.WriteLine("4. Edit user's info.");
        Console.WriteLine("5. Log out.");

        var choice = Console.ReadKey(true);
        switch (choice.Key)
        {
            case ConsoleKey.D1:
                Console.WriteLine("Enter course id: ");
                int id = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter the name of the course: ");
                string name = Console.ReadLine();

                Console.WriteLine("What grade is the course in? (1/2)");
                int grade = Convert.ToInt32(Console.ReadLine());
                if (grade == 2) // if it's a highschool course
                {
                    Console.WriteLine("How many units does this course have?");
                    int units = Convert.ToInt32(Console.ReadLine());
                    Course? tmpg = new Course(id, name, units); // makes a new course and adds it to the courses

                    _UserRepo.RegisterCourse(tmpg);
                }
                else if (grade == 1) // if it's a middle school course
                {
                    Course tmpg = new Course(id, name); // makes a new course and adds it to the courses

                    _UserRepo.RegisterCourse(tmpg);
                }
                else
                    Console.WriteLine("Wrong input!");
                break;
            case ConsoleKey.D2:
                // TODO
                found = false;
                Console.WriteLine("Which user would you like to validate? ");

                ShowUsers("teachers");
                ShowUsers("students");                

                answer = Console.ReadLine();

                Teacher? tmp = teachers.FirstOrDefault(n => n.Name == answer);
                if (tmp != null)
                {
                    teachers.FirstOrDefault(n => n.Name == answer).validated = true;
                    Console.WriteLine($"Teacher {tmp.Name} Successfuly validated.");                    
                    found = true;
                }


                Student? tmps = students.FirstOrDefault(n => n.Name == answer);
                if (tmps != null)
                {
                    students.FirstOrDefault(n => n.Name == answer).validated = true;
                    Console.WriteLine($"Student {tmps.Name} Successfuly validated.");
                    found = true;
                }

                if (!found)
                    Console.WriteLine("User doesn't exist!");
                break;
            case ConsoleKey.D3:
                found = false;
                Console.WriteLine("Enter the course's name: ");
                foreach (var crs in courses)
                    Console.WriteLine("- " + crs.name);

                answer = Console.ReadLine();

                Course? tmpc = courses.FirstOrDefault(n => n.name == answer);
                if (tmpc != null)
                {
                    Console.WriteLine($"Course {tmpc.name} Successfuly added.");
                                   
                    
                    Console.WriteLine("Choose a teacher: ");
                    foreach (var teacher in teachers)
                        Console.WriteLine((teachers.IndexOf(teacher) + 1) + "- " + teacher.Name);
                    int no = Convert.ToInt32(Console.ReadLine());
                    if ((no - 1) > teachers.Count || (no - 1) < 0)
                    {
                        Console.WriteLine("Invalid input!");
                        break;
                    }
                    courses.FirstOrDefault(n => n.name == answer).teacher = teachers[no - 1]; // assign teacher to a course's teacher
                    teachers[no - 1].courses.Add(tmpc); // adds the course to teachers list of courses
                    teachers[no - 1].salary +=  tmpc.units * 1000000;
                    Console.WriteLine($"Teacher {teachers[no - 1].Name} assigned to {tmpc.name} Succesfully!");
                    break;
                }
                else 
                { 
                    Console.WriteLine("Invalid course name!");
                    break;
                }
            case ConsoleKey.D4:
                // TODO 
                Student std;
                Teacher tch;
                found = false;
                ShowUsers("teachers");
                ShowUsers("students");
                answer = Console.ReadLine();
                foreach(var user in students)
                {
                    if (user.Name == answer)
                    {
                        found = true;
                        EditStudentInfo(user); // function to let admin see and edit info of students
                    }
                        
                }
                foreach (var user in teachers)
                {
                    if (user.Name == answer)
                    {
                        found = true;
                        EditTeacherInfo(user);
                    }
                        
                }
                if (!found)
                {
                    Console.WriteLine("Wrong input!");
                    break;
                }
                break;
            case ConsoleKey.D5:
                con = false;
                break;
                
        }
        _UserRepo.SaveChanges(students, teachers, courses);
        teachers = _UserRepo.GetTeachers();
        students = _UserRepo.GetStudents();
        courses = _UserRepo.GetCourses();

        if (con)
        {
            Console.WriteLine("Do you wish to continue? (Y/N)");
            var ch = Console.ReadKey(true);
            if (!(ch.Key == ConsoleKey.Y))
                con = false;
        }     
        
    }
}



void TeacherMenu(Teacher teacher)
{
    List<Student> tstudents = teacher.GetStudents(teacher);
    bool con = true;
    while (con)
    {
        Console.Clear();
        Console.WriteLine($"WELCOME {teacher.Name} Your monthly salary is -----> {teacher.salary} Toman");
        Console.WriteLine("1. List of students in your classes.");
        Console.WriteLine("2. Courses that you teach.");
        Console.WriteLine("3. Grade your students.");        
        Console.WriteLine("4. Exit.");

        var answer = Console.ReadKey(true);
        switch (answer.Key)
        {
            case ConsoleKey.D1:
                if (teacher.courses.Count == 0)
                {
                    Console.WriteLine("You dont teach any courses!");
                    break;
                }

                foreach (var course in teacher.courses)
                {
                    Console.WriteLine($"Course name: {course.name}.");
                    foreach (var student in course.students)
                        Console.WriteLine($"-- {student.Name}");
                }
                break;
            case ConsoleKey.D2:
                if (teacher.courses.Count == 0)
                {
                    Console.WriteLine("You dont teach any courses!");
                    break;
                }

                foreach (var course in teacher.courses)
                {
                        Console.WriteLine($"-- {course.name}");
                }
                break;
            case ConsoleKey.D3:
                Console.WriteLine("Choose a student: ");
                int i = 1;
                if (students.Count == 0)
                {
                    Console.WriteLine("You don't have any students!");
                    break;
                }
                foreach (var student in tstudents)
                {
                    Console.WriteLine($"{i} - {student.Name}");
                    i++;
                }
                int choice = Convert.ToInt32(Console.ReadLine());

                if ((choice - 1) > tstudents.Count)
                {
                    Console.WriteLine("Wrong input!");
                    break;
                }
                
                Console.WriteLine("Choose which course you would like to grade the student in: ");
                Course crs = null;
                foreach (var course in teacher.courses)
                {
                    foreach (var student in course.students)
                    {
                        if (student.Email == tstudents[choice - 1].Email)
                        {
                            Console.WriteLine($"- {course.name}");
                        }
                    }
                }

                string lesson = Console.ReadLine();
                foreach (var course in courses)
                {
                    if (course.name == lesson)
                    {
                        crs = course;
                    }
                }
                if (crs == null)
                {
                    Console.WriteLine("wrong input!");
                    break;
                }
                Console.WriteLine("Enter the score: ");
                int score = Convert.ToInt32(Console.ReadLine());
                (students.FirstOrDefault(e => e.Email == tstudents[choice - 1].Email).scores ??= new Dictionary<string, int>()).Add(crs.name, score);               
                Console.WriteLine("Grading done successfuly!");
                break;
            case ConsoleKey.D4:
                con = false;
                break;                   
        }
        _UserRepo.SaveChanges(students, teachers, courses);
        teachers = _UserRepo.GetTeachers();
        students = _UserRepo.GetStudents();
        courses = _UserRepo.GetCourses();

        if (con)
        {
            Console.WriteLine("Press any key to continue.");
            var ch = Console.ReadKey(true);
        }
    }
}

void StudentMenu(Student student)
{
    int sum = 0;
    int units = 0;
    bool con = true;
    // TODO
    while (con)
    {
        Console.Clear();
        Console.WriteLine("1. Courses.");
        Console.WriteLine("2. Summary of grades.");
        Console.WriteLine("3. Log out.");
        var choice = Console.ReadKey(true);
        switch (choice.Key)
        {
            case ConsoleKey.D1:
                foreach (var course in student.courses) 
                {
                    if (course.teacher != null)                       
                        Console.WriteLine($"{course.name} -- Teacher --> {course.teacher.Name}");
                    Console.WriteLine($"--{course.name}");
                }
                break; 
            case ConsoleKey.D2:
                student.ShowScores();
                break;
            case ConsoleKey.D3:
                con = false;
                break;
                    
        }

        teachers = _UserRepo.GetTeachers();
        students = _UserRepo.GetStudents();
        courses = _UserRepo.GetCourses();

        if (con)
        {
            Console.WriteLine("Press any key to continue.");
            var ch = Console.ReadKey(true);
        }
    }
}


void ShowUsers(string role) // lists names of teachers or students depending on input
{

    teachers = _UserRepo.GetTeachers();
    students = _UserRepo.GetStudents();
    courses = _UserRepo.GetCourses();
    if (role == "teachers")
    {
        Console.WriteLine("---Teachers---");
        foreach (var teacher in teachers)
            Console.WriteLine("- " + teacher.Name);
    }
    else
    {
        Console.WriteLine("---Students---");
        foreach (var student in students)
            Console.WriteLine("- " + student.Name);
    }
}

void EditStudentInfo(Student student)
{
    string answer;
    bool con = true;
    while (con)
    {
        Console.Clear();
        Console.WriteLine($"1 - Name : {student.Name}");
        Console.WriteLine($"2 - Email : {student.Email}");
        Console.WriteLine($"3 - Passwpord : {student.Password}");
        Console.WriteLine($"4 - Grade : {student.grade}");
        Console.WriteLine("5 - Add a course.");
        Console.WriteLine("6 - Exit.");
        if (student.courses != null)
        {
            Console.Write($"Courses : ");
            foreach (Course course in student.courses)
                Console.Write($" {course.name} --");
        }
        Console.WriteLine();
        var choice = Console.ReadKey(true);
        switch (choice.Key)
        {
            case ConsoleKey.D1:
                Console.WriteLine("Enter new name: ");
                answer = Console.ReadLine();
                student.Name = answer;
                Console.WriteLine("Name changed successfully!");
                break;
            case ConsoleKey.D2:
                Console.WriteLine("Enter new email: ");
                answer = Console.ReadLine();
                student.Email = answer;
                Console.WriteLine("Email changed successfully!");
                break;
            case ConsoleKey.D3:
                Console.WriteLine("Enter new password: ");
                answer = Console.ReadLine();
                student.Password = answer;
                Console.WriteLine("Password changed successfully!");
                break;
            case ConsoleKey.D4:
                Console.WriteLine("Select grade: (1/2)");
                choice = Console.ReadKey(true);
                switch (choice.Key)
                {
                    case (ConsoleKey.D1):
                        student.grade = Grade.MiddleSchool;
                        Console.WriteLine("grade changed successfully!");
                        break;
                    default:
                        student.grade = Grade.HighSchool;
                        Console.WriteLine("grade changed successfully!");
                        break;
                }
                break;
            case ConsoleKey.D5:
                if (student.grade == Grade.HighSchool)
                {
                    SelectCourse(students.IndexOf(student));
                }
                break;
            case ConsoleKey.D6:
                con = false;
                break;
        }
        _UserRepo.SaveChanges(students, teachers, courses);
        teachers = _UserRepo.GetTeachers();
        students = _UserRepo.GetStudents();
        courses = _UserRepo.GetCourses();

        if (con)
        {
            Console.WriteLine("Press any key to continue.");
            var ch = Console.ReadKey(true);
        }
    }
}

void EditTeacherInfo(Teacher teacher)
{
    string answer;
    bool con = true;
    while (con)
    {
        Console.Clear();
        Console.WriteLine($"1 - Name : {teacher.Name}");
        Console.WriteLine($"2 - Email : {teacher.Email}");
        Console.WriteLine($"3 - Passwpord : {teacher.Password}");
        Console.WriteLine($"4 - Monthly Salary : {teacher.salary}");
        Console.WriteLine("5 - Exit.");

        var choice = Console.ReadKey(true);
        switch (choice.Key)
        {
            case ConsoleKey.D1:
                Console.WriteLine("Enter new name: ");
                answer = Console.ReadLine();
                teacher.Name = answer;
                Console.WriteLine("Name changed successfully!");
                break;
            case ConsoleKey.D2:
                Console.WriteLine("Enter new email: ");
                answer = Console.ReadLine();
                teacher.Email = answer;
                Console.WriteLine("Email changed successfully!");
                break;
            case ConsoleKey.D3:
                Console.WriteLine("Enter new password: ");
                answer = Console.ReadLine();
                teacher.Password = answer;
                Console.WriteLine("Password changed successfully!");
                break;
            case ConsoleKey.D4:
                Console.WriteLine("Enter new salary");
                int salary = Convert.ToInt32(Console.ReadLine());
                teacher.salary = salary;
                Console.WriteLine("Salary changed successfully!");
                break;
            case ConsoleKey.D5:
                con = false;
                break;
        }

        _UserRepo.SaveChanges(students, teachers, courses);
        teachers = _UserRepo.GetTeachers();
        students = _UserRepo.GetStudents();
        courses = _UserRepo.GetCourses();

        if (con)
        {
            Console.WriteLine("Press any key to continue.");
            var ch = Console.ReadKey(true);
        }
    }
}