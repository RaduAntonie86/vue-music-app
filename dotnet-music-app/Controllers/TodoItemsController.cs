using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

[ApiController]
[Route("[controller]")]
public class TodoItemsController : Controller
{
    private readonly ITodoItemService _todoItemService;
    
    public TodoItemsController(ITodoItemService todoItemService)
    {
        _todoItemService = todoItemService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result =  await _todoItemService.GetTodoItemList();

        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTodoItem(int id)
    {
        var result =  await _todoItemService.GetTodoItem(id);

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddTodoItem([FromBody]TodoItem todoItem)
    {
        var result =  await _todoItemService.CreateTodoItem(todoItem);

        return Ok(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateTodoItem([FromBody]TodoItem todoItem)
    {
        var result =  await _todoItemService.UpdateTodoItem(todoItem);

        return Ok(result);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        var result =  await _todoItemService.DeleteTodoItem(id);

        return Ok(result);
    }
}