using System;

namespace ConsoleApp
{
    public class Employee
    {
        public string Name { get; set; }

        public int GetYearsEmployed()
        {
            return 5;
        }

        public virtual string GenProgressReport()
        {
            Console.WriteLine(Name + "Employee");
            return Name + "Employee";
        }

        public static Employee Lookup(string name)
        {
            return new Employee { Name = name };
        }
    }

    public sealed class Manager : Employee
    {
        public override string GenProgressReport()
        {
            Console.WriteLine(Name + "Manager");
            return Name + "Manager";
        }
    }
}
