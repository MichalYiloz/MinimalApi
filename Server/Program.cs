using TodoApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//הזרקת הסרוויס
builder.Services.AddDbContext<ToDoDbContext>();
//swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//cors
builder.Services.AddCors();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder =>
        {
            builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

var app = builder.Build();

//swagger
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//הצגת כל המשימות
app.MapGet("/", async (ToDoDbContext db) =>
{
    var data = await db.Items.ToListAsync();
    return data;
});
//הוספת משימה
app.MapPost("/ToAdd/{Name}", async (string Name, ToDoDbContext db) =>
{
    Item i = new Item();
    i.Name=Name;
    
    db.Items.Add(i);

    await db.SaveChangesAsync();
     return Results.Ok(db.Items);
});

//עדכון משימה אם בוצעה או לא
app.MapPut("/ToPut/{id}/{isComplete}", async (int id, ToDoDbContext db, bool IsComplete) =>
{
    var todo = await db.Items.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.IsComplete = IsComplete;
    

    await db.SaveChangesAsync();

    return Results.Ok(db.Items);
});

//מחיקת משימה לפי קוד
app.MapDelete("/ToDelete/{id}", async (int id, ToDoDbContext db) =>
{
    if (await db.Items.FindAsync(id) is Item todo)
    {
        db.Items.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowOrigin");

// app.MapGet("/", () => "server API is running!");


app.Run();
