using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using MyProyect_Granja.Models;
using MyProyect_Granja.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyProyect_Granja
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            // Configuración de Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // Configuración de JWT en Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Por favor ingrese el token JWT en este formato: Bearer {token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Configuración de la base de datos
            services.AddDbContext<GranjaAres1Context>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("GranjaAres1Database")));

            // Configuración de servicios específicos
            services.AddScoped<IClasificacionHuevoService, ClasificacionHuevoService>();
            services.AddScoped<ICorralService, CorralService>();
            services.AddScoped<IEstadoLoteService, EstadoLoteService>();
            services.AddScoped<IProduccionService, ProduccionService>();
            services.AddScoped<IRazaGService, RazaGService>();
            services.AddScoped<ILoteService, LoteService>();

            // Configuración de CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyApp",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                               .AllowAnyHeader()
                               .AllowAnyMethod(); // Permite todos los métodos
                    });
            });

            // Configuración de autenticación básica
            var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowMyApp");

            // Middleware personalizado para validación de JWT
            app.UseMiddleware<JwtMiddleware>();

            // Añadir autenticación
            app.UseAuthentication();

            // Añadir autorización
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
