using TodoApi.Models;

public interface ITodoItemService
{
    Task<bool> CreateTodoItem(TodoItem employee);
    Task<List<TodoItem>> GetTodoItemList();
    Task<TodoItem> GetTodoItem(int id);
    Task<TodoItem> UpdateTodoItem(TodoItem employee);
    Task<bool> DeleteTodoItem(int key);
}