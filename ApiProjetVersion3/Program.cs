
using Microsoft.EntityFrameworkCore;
using ApiProjetVersion3.Data;
//using Microsoft.Extensions.DependencyInjection;
using ApiProjetVersion3.Services;
using System.Text.Json.Serialization;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;

namespace ApiProjetVersion3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            // R�cup�re la cha�ne de connexion � la base dans les param�tres
            string? connect = builder.Configuration.GetConnectionString("ApiDbangolav2");
            //ajoute le context dans le conteneur IoC
            // Enregistre la classe de contexte de donn�es comme service (dans le conteneur IoC)
            // en lui indiquant la connexion � utiliser
            builder.Services.AddDbContext<ApiDbaprojetngolav2>(opt => opt.UseSqlServer(connect)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking) // interdir la suivi de modification
            .EnableSensitiveDataLogging()); // activer la journalisation des infos sensible


            //enregistre le service metier
            builder.Services.AddScoped<IServiceEmployes, ServiceEmployes>();
            // Utilise Serilog comme unique fournisseur de journalisation
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);



            // builder.Services.AddDbContext<Northwind3Context>(options =>
            // options.UseSqlServer(builder.Configuration.GetConnectionString("Northwind3Context") ?? throw new InvalidOperationException("Connection string 'Northwind3Context' not found.")));


            //Paramétrer le sérialiseur pour qu’il détecte et interrompe les références
            //circulaires en ajoutant le code suivant dans Program 
            builder.Services.AddControllers()
             // .AddNewtonsoftJson(opt =>
              // opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();//point de terminaison pour api minimal
            //builder.Services.AddSwaggerGen(); remplacer par builder.Services.AddOpenApiDocument();
            builder.Services.AddOpenApiDocument(options => {

                options.Title = "API SmartEmploye";
                options.Description = "<strong>Projet Gestion Employes par Tresor Ngola <br/><a href='https://github.com/donngola/gestion_employeV2'>voir code sur GitHub</a></strong>";
              
                options.Version = "V_Ngola";
                options.DocumentName = "SmartEmployeBy Tresor_Ngola_v1";
            });// methode utiliser par Nswag




            //configuration de l'authentification 

            // Ajoute le service d'authentification par porteur de jetons JWT
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   // url d'accès au serveur d'identité
                   options.Authority = builder.Configuration["IdentityServerUrl"];
                   options.TokenValidationParameters.ValidateAudience = false;

                   // Tolérance sur la durée de validité du jeton
                   options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
               });

            //configuration de l'autorisation
            // Ajoute le service d'autorisation
            builder.Services.AddAuthorization(options =>
            {
                // Spécifie que tout utilisateur de l'API doit être authentifié
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                   .RequireAuthenticatedUser()
                   .Build();
            });

            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();//   app.UseSwagger();
                app.UseSwaggerUi();// permet de construire l'interface web (graphique)
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            //app.MapControllers();
            //ce code desactive les autorisations de facon anonyme
            var endpointBuilder = app.MapControllers();
            if (app.Environment.IsDevelopment())
                endpointBuilder.AllowAnonymous();

            app.Run();

            app.Run();
        }
    }
}
