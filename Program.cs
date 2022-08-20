using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
//app.UseAuthorization();
//app.MapRazorPages();
app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();

/*
        static void Main(string[] args)
        {
            var loader = new LoadPuzzle();

            var puzzle = loader.LoadFromInputTxt(args[0]);

            puzzle.WriteToConsole();

            int previousNumberFilledIn = puzzle.NumbersFilledIn;
            while (puzzle.ApplyAllStrats())
            {
                int currentNumberFilledIn = puzzle.NumbersFilledIn;
                if (currentNumberFilledIn > previousNumberFilledIn)
                {
                    puzzle.WriteToConsole();
                }
                if (currentNumberFilledIn == 81)
                {
                    puzzle.EndSolutionStats();
                    return;
                }
                previousNumberFilledIn = currentNumberFilledIn;
            }
            puzzle.EndSolutionStats();
        }
    }
}
*/