using System;
using System.Threading.Tasks;
using TodoListComplex.TodoListApp.Model;
using TodoListComplex.TodoListApp.Service;
using static System.Collections.Specialized.BitVector32;

namespace TodoListComplex.TodoListApp.View
{
    public class UserInput
    {
        public (string Title, string Description, DateTime DueDate, string Priority, string RecurrenceInterval) GetUserInput()
        {
            Console.Write("\nAdd New Todo");

            Console.Write("\nTitle: ");
            string title = Console.ReadLine();

            Console.Write("Description: ");
            string description = Console.ReadLine();

            Console.Write("Due Date (YYYY-mm-dd): ");
            DateTime dueDate;

            while (!DateTime.TryParse(Console.ReadLine(), out dueDate))
            {
                Console.WriteLine("Wrong date format, please enter correct format!.");
            }

            Console.Write("Priority (Low, Medium, High): ");
            string priority = Console.ReadLine();

            Console.Write("Interval (None, Daily, Weekly, Monthly): ");
            string interval = Console.ReadLine();

            return (title, description, dueDate, priority, interval);
        }

        public int GetTodoIndex(string action)
        {
            Console.Write($"Please enter todo id to be {action}: ");
            int todoIndex;

            while (!int.TryParse(Console.ReadLine(), out todoIndex) || todoIndex < 1)
            {
                Console.WriteLine("Please enter correct todo id (e.g. 1,2,3...)");
            }
            return todoIndex;
        }

        public DateTime GetTodoNewDueDate()
        {
            Console.Write("\nEnter a New Due-Date (YYYY-mm-dd): ");
            DateTime newDueDate;

            while (!DateTime.TryParse(Console.ReadLine(), out newDueDate))
            {
                Console.WriteLine("Please enter correct date format (YYYY-mm-dd)");
            }

            return newDueDate;
        }

        public int GetNumberOfDays()
        {
            Console.Write("\nEnter the number of days to check for upcoming due todos: ");
            int days;

            while (!int.TryParse(Console.ReadLine(), out days) || days < 1)
            {
                Console.WriteLine("\nPlease enter a specific number of day(s) (e.g. 1,2,3...)");
            }
            return days;
        }
    }
}
