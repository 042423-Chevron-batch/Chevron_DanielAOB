using Microsoft.AspNetCore.HttpsPolicy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});





// Add session services
builder.Services.AddDistributedMemoryCache(); // Add a distributed memory cache
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "YourSessionCookieName"; // Set a unique cookie name for the session
    options.IdleTimeout = TimeSpan.FromMinutes(1000); // Set the session timeout
    options.Cookie.HttpOnly = true; // Ensure the session cookie is accessible only through HTTP
    options.Cookie.IsEssential = true; // Mark the session cookie as essential
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

// Enable session middleware
app.UseSession();

// Enable CORS
app.UseCors();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
