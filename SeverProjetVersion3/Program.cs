using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SeverProjetVersion3.Data;
using SeverProjetVersion3.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<UtilisateurDaplication>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

//technolgie de page dans la technologie asp.net (rezore melange les pages html a du code c#)
builder.Services.AddRazorPages();

// Ajoute et configure le service IdentityServer
builder.Services.AddIdentityServer(options =>
      options.Authentication.CoordinateClientLifetimesWithUserSession = true)

    // Cr�e des identit�s
    .AddInMemoryIdentityResources(new IdentityResource[] {
         new IdentityResources.OpenId(),
         new IdentityResources.Profile(),
    })

    // Configure une appli cliente
    .AddInMemoryClients(new Client[] {
         new Client
         {
            ClientId = "Client1",
            ClientSecrets = { new Secret("Secret1".Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
                  // Urls auxquelles envoyer les jetons
            RedirectUris = { "https://localhost:7189/signin-oidc" },
            // Urls de redirection apr�s d�connexion
            PostLogoutRedirectUris = { "https://localhost:7189/signout-callback-oidc" },
             // Url pour envoyer une demande de d�connexion au serveur d'identit�
            FrontChannelLogoutUri = "https://localhost:7189/signout-oidc",

            // Etendues d'API autoris�es
            AllowedScopes = { "openid", "profile", "entreprise" },

            // Autorise le client � utiliser un jeton d'actualisation
            AllowOfflineAccess = true
         }
    })
    // Indique d'utiliser ASP.Net Core Identity pour la gestion des profils et revendications
    .AddAspNetIdentity<UtilisateurDaplication>();

// Ajoute la journalisation au niveau debug des �v�nements �mis par Duende
builder.Services.AddLogging(options =>
{
    options.AddFilter("Duende", LogLevel.Debug);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
