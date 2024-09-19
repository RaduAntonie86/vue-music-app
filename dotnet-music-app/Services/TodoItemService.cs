using TodoApi.Models;

public class TodoItemService : ITodoItemService
{
    private readonly IDbService _dbService;

    public TodoItemService(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<bool> CreateTodoItem(TodoItem todoItem)
    {
        var result =
            await _dbService.EditData(
                "INSERT INTO public.to_do_item (id, name, is_complete, secret) VALUES (@Id, @Name, @IsComplete, @Secret)",
                todoItem);
        return true;
    }

    public async Task<List<TodoItem>> GetTodoItemList()
    {
        var todoItemList = await _dbService.GetAll<TodoItem>("SELECT * FROM public.to_do_item", new { });
        return todoItemList;
    }


    public async Task<TodoItem> GetTodoItem(int id)
    {
        var todoItemList = await _dbService.GetAsync<TodoItem>("SELECT * FROM public.to_do_item where id=@id", new {id});
        return todoItemList;
    }

    public async Task<TodoItem> UpdateTodoItem(TodoItem todoItem)
    {
        var updateTodoItem =
            await _dbService.EditData(
                "Update public.to_do_item SET name=@Name, is_complete=@IsComplete, secret=@Secret WHERE id=@Id",
                todoItem);
        return todoItem;
    }

    public async Task<bool> DeleteTodoItem(int id)
    {
        var deleteTodoItem = await _dbService.EditData("DELETE FROM public.to_do_item WHERE id=@Id", new {id});
        return true;
    }
}