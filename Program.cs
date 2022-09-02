using MiniTodo.Data;
using MiniTodo.ViewModels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

//app.MapGet("/", () => {
//    //var todo = new Todo(Guid.NewGuid(), "Ir fazar caf�", false);
//    Todo todo = new(Guid.NewGuid(), "Arrumar a cama", false);
//    return Results.Ok(todo);
//});
app.MapGet("v1/todos", (AppDbContext context) => {
    //var todo = new Todo(Guid.NewGuid(), "Ir fazar caf�", false);
    var todos = context.Todos.ToList();
    return Results.Ok(todos);
}).Produces<Todo>();

app.MapPost("v1/todos", (
    AppDbContext context,
    CreateTodoViewModel model) => {

        var todo = model.MapTo();
        if(!model.IsValid)
            return Results.BadRequest(model.Notifications);

        context.Todos.Add(todo);
        context.SaveChanges();
        return Results.Created($"/v1/todos/{todo.Id}", todo);
    
    });

app.Run();
