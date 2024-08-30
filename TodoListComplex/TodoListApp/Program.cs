using System;
using System.Threading.Channels;
using TodoListComplex.TodoListApp.Model;
using TodoListComplex.TodoListApp.Service;
using TodoListComplex.TodoListApp.View;

namespace TodoListComplex.TodoListApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("WELCOME TO TODOLIST CONSOLE");

            TodoService todoService = new();
            UserInput myInput = new();

            todoService.UpdateRepeatTodos();

            string path = "myFile.json";
            todoService.LoadTodoFromFile(path);

            bool exit = false;

            while (!exit)
            {
                Console.Write("\n1: Add Todo \n2: Remove Todo " +
                    "\n3: Update Todo Due-Date \n4: Mark Todo as Completed" +
                    "\n5: Update Todo \n6: View TodoList \n7: Sort TodoList" +
                    "\n8: View Overdue TodoList \n9: View TodoList Due Soon" +
                    "\n10: Exit\n\n");
                Console.Write("Choose an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        var (Title, Description, DueDate, Priority, Interval) = myInput.GetUserInput();

                        Todo newTodo = new(Title, Description, DueDate, Priority, Interval);

                        todoService.AddTodo(newTodo);

                        Console.WriteLine("Todo added successfully!.\n");
                        break;

                    case "2":
                        var getIndex = myInput.GetTodoIndex("Removed");

                        if (todoService.RemoveTodo(getIndex))
                        {
                            //Print message upon successful addition of a new event
                            Console.WriteLine("Todo removed successfully!.\n");
                        }
                        else
                        {
                            Console.WriteLine("Todo Not Found\n");
                        }

                        break;

                    case "3":
                        int indexOfTodoToBeUpdated = myInput.GetTodoIndex("Updated");
                        DateTime newDueDate = myInput.GetTodoNewDueDate();

                        if (todoService.UpdateDueDate(indexOfTodoToBeUpdated, newDueDate))
                        {
                            Console.WriteLine("\nTodo due date updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to update todo due date!.");
                        }

                        break;

                    case "4":
                        int taskIndexToBeCompleted = myInput.GetTodoIndex("Completed");

                        if (todoService.TodoIsComplete(taskIndexToBeCompleted))
                        {
                            Console.WriteLine("Todo Completed Successfully\n");
                        }
                        else
                        {
                            Console.WriteLine("Todo not Found\n");
                        }

                        break;

                    case "5":


                    case "6":
                        bool viewList = false;
                        List<Todo> todoList = [];

                        bool complete = true;
                        bool pending = true;

                        Console.WriteLine("");
                        Console.Write("List Of All Added Todo");

                        while (!viewList)
                        {
                            Console.WriteLine("\n");

                            Console.Write("1: View All Todos \n2: View Completed Todos" +
                                "\n3: View Pending Todos \n4: Cancel\n\n");
                            Console.Write("Choose an Option: ");
                            Console.Write("");

                            switch (Console.ReadLine())
                            {
                                case "1":
                                    todoList = todoService.GetTodos();
                                    viewList = true;
                                    break;

                                case "2":
                                    todoList = todoService.GetTodos(complete, !pending);
                                    viewList = true;
                                    break;

                                case "3":
                                    todoList = todoService.GetTodos(!complete, pending);
                                    viewList = true;
                                    break;

                                case "4":
                                    viewList = true;
                                    break;

                                default:
                                    Console.WriteLine("Invalid Option! Try Again!!!");
                                    viewList = true;
                                    break;

                            }

                            todoService.DisplayTodos(todoList);

                        }

                        break;

                    case "7":
                        bool stop = false;

                        Console.WriteLine("");
                        while (!stop)
                        {
                            Console.WriteLine("1. Sort By Duedate \n2. Sort By Priority" +
                                "\n3. Sort By Repeat Interval\n");
                            Console.Write("Choose an Option: ");
                            Console.Write("");

                            switch (Console.ReadLine())
                            {
                                case "1":
                                    var sortedList = todoService.SortTodosByDueDate();
                                    todoService.DisplayTodoSortedByDueDate(sortedList);

                                    stop = true;
                                    break;

                                case "2":
                                    var priorityList = todoService.SortTodoByPriority();
                                    todoService.DisplayTodos(priorityList);

                                    stop = true;
                                    break;

                                case "3":
                                    var recurrenceList = todoService.SortTodoByRepeat();
                                    todoService.DisplayTodos(recurrenceList);

                                    stop = true;
                                    break;
                            }
                        }

                        break;

                    case "8":
                        var overdueTodos = todoService.OverDueTodos();
                        Console.WriteLine("\nOverdue Todos:");
                        Console.WriteLine("----------------------\n");

                        if (overdueTodos.Count < 1)
                        {
                            Console.WriteLine("No Overdue Todos");
                        }
                        else
                        {
                            todoService.DisplayTodos(overdueTodos);
                        }

                        break;

                    case "9":
                        int days = myInput.GetNumberOfDays();
                        var tasksDueSoon = todoService.TodosDueSoon(days);

                        Console.WriteLine($"Todos due within the next {days} days:\n");

                        if (tasksDueSoon.Count < 1)
                        {
                            Console.WriteLine("No tasks are due within the specified time frame.");
                        }
                        else
                        {
                            todoService.DisplayTodos(tasksDueSoon);
                        }

                        break;

                    case "10":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid Input! Please Try Again...\n\n");
                        break;
                }

            }

            todoService.SaveTodoToFile(path); // Save list of task into File

        }
    }
}
