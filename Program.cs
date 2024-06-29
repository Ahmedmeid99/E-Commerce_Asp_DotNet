using E_Commerce_DataAccessLayer.Globals;

namespace E_Commerce_API_Layer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Read (Get) ConnectionString 
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
           
            // Configuration ingection  in ConfigurationHelper class
            ConfigurationHelper.Initialize(builder.Configuration);

            // Add services to the container.
            builder.Services.AddCors();   // add cors to allow frontend to access API

            //------------------------------------------------

            builder.Services.AddControllers();   // add Controllers to us it`s end points in api  

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors((c) => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()); // AllowAnyOrigin(Origin : "") for close end point

            app.UseRouting(); //
            app.UseAuthorization();


            app.UseEndpoints(Endpoint =>
            {
                Endpoint.MapControllers();
            });


            //app.MapControllers();

            app.Run();
        }
    }
}