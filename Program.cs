using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => 
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version (8,0,32)));
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

async Task<List<UsuarioModel>> GetUsuarios(AppDbContext context)
{
    return await context.usuarios.ToListAsync();
}

app.MapGet("/Usuarios", async (AppDbContext context) =>
{
    return await context.usuarios.ToListAsync();
}
);

app.MapPost("/Usuarios", async (AppDbContext context, UsuarioModel usuario) => 
{
    context.usuarios.Add(usuario);
    await context.SaveChangesAsync();

    return await GetUsuarios(context);
}
);

app.MapGet("/Usuario/{id}", async (AppDbContext context, int id) => 
{
    var usuario = await context.usuarios.FindAsync(id);
    
    if (usuario == null)
    {
        return Results.NotFound("Usuario não encontrado");
    }

    return Results.Ok(usuario);
}
);

app.MapPut("/Usuario", async (AppDbContext context, UsuarioModel usuario) => 
{
    var usuariodb = await context.usuarios.AsNoTracking().FirstOrDefaultAsync(usuariodb => usuariodb.Id == usuario.Id);

    if (usuariodb == null)
    {
        return Results.NotFound("Usuário não encontrado");
    }

    context.Update(usuario);
    await context.SaveChangesAsync();

    return Results.Ok(GetUsuarios);
}
);

app.MapDelete("/Usuario/{id}", async (AppDbContext context, int id) => 
{
    var usuariodb = await context.usuarios.FindAsync(id);

    if(usuariodb == null)
    {
        return Results.NotFound("Usuário não encontrado");
    }

    context.usuarios.Remove(usuariodb);
    await context.SaveChangesAsync();

    return Results.Ok(GetUsuarios);
}
);

app.Run();

