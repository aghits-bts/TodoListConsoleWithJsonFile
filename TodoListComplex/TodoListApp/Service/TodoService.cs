using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using TodoListComplex.TodoListApp.Model;

namespace TodoListComplex.TodoListApp.Service
{
    public class TodoService
    {
        private readonly List<Todo> todos;

        public TodoService()
        {
            todos = new List<Todo>();
        }

        public void AddTodo(Todo todo)
        {
            todos.Add(todo);
        }

        public bool RemoveTodo(int index)
        {
            var getIndex = index - 1;

            if (todos.Count >= 1 && getIndex < todos.Count)
            {
                todos.RemoveAt(getIndex);
                return true;
            }
            return false;
        }

        public List<Todo> GetTodos(bool showCompleted = true, bool showPending = true)
        {
            if (showCompleted && showPending)
            {
                return todos;
            }
            else if (showCompleted && !showPending)
            {
                return todos.Where(t => t.GetIsCompleted()).ToList();
            }
            else if (!showCompleted && showPending)
            {
                return todos.Where(t => !t.GetIsCompleted()).ToList();
            }
            else
            {
                return new List<Todo>();
            }
        }

        public void DisplayTodos(List<Todo> todos)
        {
            Console.WriteLine("");

            if (todos.Count > 0)
            {
                for (int i = 0; i < todos.Count; i++)
                {
                    string status = todos[i].GetIsCompleted() ? "Completed" : "Pending";

                    Console.WriteLine($"{i + 1}:  {todos[i].GetTitle().ToUpper()}-: " +
                                        $"{todos[i].GetDueDate()} {todos[i].Priority} " +
                                        $"{todos[i].RepeatInterval} {status}");
                }
                Console.WriteLine("------------------------------------------------------\n");

            }
            else
            {
                Console.WriteLine("TodoList is empty, please add todo first!.");
                Console.WriteLine("-------------------------------------\n");
            }

        }

        public void DisplayTodoSortedByDueDate(List<Todo> todos)
        {
            Console.Write("");
            Console.WriteLine("List of Todo Ordered by Due Date\n");

            DisplayTodos(todos);
        }

        public bool TodoIsComplete(int index)
        {
            var getIndex = index - 1;

            if (todos.Count >= 1 && getIndex < todos.Count)
            {
                todos[getIndex].SetIsCompleted(true);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SaveTodoToFile(string filePath)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save todo: {ex.Message}");
            }
        }

        public void LoadTodoFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var jsonData = File.ReadAllText(filePath);

                    Console.WriteLine("File Content:");
                    Console.WriteLine(jsonData);

                    var options = new JsonSerializerOptions
                    {
                        IncludeFields = true,
                        PropertyNameCaseInsensitive = true,
                    };
                    var loadData = JsonSerializer.Deserialize<List<Todo>>(jsonData, options) ?? new List<Todo>();

                    todos.Clear();
                    todos.AddRange(loadData);
                    DisplayTodos(todos);
                }
                else
                {
                    Console.WriteLine("File doesn't exist!.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Failed to Load File {ex.Message}");
            }
        }

        public List<Todo> SortTodosByDueDate()
        {
            return todos.OrderBy(t => t.GetDueDate()).ToList();
        }

        public bool UpdateDueDate(int index, DateTime newDueDate)
        {
            int getIndex = index - 1;

            if (todos.Count >= 1 && getIndex < todos.Count)
            {
                todos[getIndex].setDueDate(newDueDate);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Todo> OverDueTodos()
        {
            return todos.Where(t => t.GetDueDate() < DateTime.Now && t.GetIsCompleted().Equals(false)).ToList();
        }

        public List<Todo> TodosDueSoon(int days)
        {
            DateTime dateThreshold = DateTime.Now.AddDays(days);
            return todos.Where(t => t.GetDueDate() >= DateTime.Now && t.GetDueDate() <= dateThreshold).ToList();
        }

        public void UpdateRepeatTodos()
        {
            var todoList = todos.Where(t => t.GetIsCompleted().Equals(true) && t.RepeatInterval != "None").ToList();

            foreach (var todo in todoList)
            {
                if (todo.RepeatInterval.Equals("Daily"))
                {
                    todo.setDueDate(todo.GetDueDate().AddDays(1));
                }
                else if (todo.RepeatInterval.Equals("Weekly"))
                {
                    todo.setDueDate(todo.GetDueDate().AddDays(7));
                }
                else if (todo.RepeatInterval.Equals("Monthly"))
                {
                    todo.setDueDate(todo.GetDueDate().AddMonths(1));
                }
            }

        }

        public List<Todo> SortTodoByPriority()
        {
            return todos.OrderByDescending(t => t.Priority).ToList();
        }

        public List<Todo> SortTodoByRepeat()
        {
            return todos.OrderByDescending(t => t.RepeatInterval).ToList();
        }
    }
}
