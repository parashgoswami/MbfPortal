using Domain.Entities;
using Infrastucture.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;

namespace Infrastucture.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var adminRole = new IdentityRole { Name = "Administrator" };
        var memberRole = new IdentityRole { Name = "Member" };

        if (_roleManager.Roles.All(r => r.Name != adminRole.Name))
        {
            await _roleManager.CreateAsync(adminRole);
        }

        if (_roleManager.Roles.All(r => r.Name != memberRole.Name))
        {
            await _roleManager.CreateAsync(memberRole);
        }



        // Default users
        var administrator = new AppUser { UserName = "admin@localhost", Email = "admin@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(adminRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { adminRole.Name });
            }
        }

        // Seed data for Location entity
        if (!_context.Locations.Any())
        {
            var locations = new List<Location>
        {
            new Location { Name = "Shillong", CreatedBy ="admn"},
            new Location { Name = "Guwahati" , CreatedBy ="admn"},
            new Location { Name = "AGBPS" , CreatedBy ="admn" },
            new Location { Name = "New Delhi", CreatedBy = "admn"}
        };

            _context.Locations.AddRange(locations);
            await _context.SaveChangesAsync();
        }

        if (!_context.Members.Any())
        {
            var members = new List<Member>
        {
            new Member { EmpNo = "6001", FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", DOJ = DateTime.UtcNow.AddYears(-5), LocationId = 1, Share = 50, CreatedBy="admn" },
            new Member { EmpNo = "6002", FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", DOJ = DateTime.UtcNow.AddYears(-4), LocationId = 2, Share = 75, CreatedBy="admn" },
            new Member { EmpNo = "6003", FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com", DOJ = DateTime.UtcNow.AddYears(-3), LocationId = 3, Share = 100, CreatedBy="admn" },
            new Member { EmpNo = "6004", FirstName = "Bob", LastName = "Brown", Email = "bob.brown@example.com", DOJ = DateTime.UtcNow.AddYears(-2), LocationId = 4, Share = 25, CreatedBy="admn" },
            new Member { EmpNo = "6005", FirstName = "Charlie", LastName = "Davis", Email = "charlie.davis@example.com", DOJ = DateTime.UtcNow.AddYears(-1), LocationId = 1, Share = 60 , CreatedBy = "admn"},
            new Member { EmpNo = "6006", FirstName = "Emily", LastName = "Wilson", Email = "emily.wilson@example.com", DOJ = DateTime.UtcNow.AddYears(-6), LocationId = 2, Share = 80 , CreatedBy = "admn"},
            new Member { EmpNo = "6007", FirstName = "Frank", LastName = "Taylor", Email = "frank.taylor@example.com", DOJ = DateTime.UtcNow.AddYears(-7), LocationId = 3, Share = 40 , CreatedBy = "admn"},
            new Member { EmpNo = "6008", FirstName = "Grace", LastName = "Anderson", Email = "grace.anderson@example.com", DOJ = DateTime.UtcNow.AddYears(-8), LocationId = 4, Share = 90 , CreatedBy = "admn"},
            new Member { EmpNo = "6009", FirstName = "Hank", LastName = "Thomas", Email = "hank.thomas@example.com", DOJ = DateTime.UtcNow.AddYears(-9), LocationId = 1, Share = 30 , CreatedBy = "admn"},
            new Member { EmpNo = "6010", FirstName = "Ivy", LastName = "Moore", Email = "ivy.moore@example.com", DOJ = DateTime.UtcNow.AddYears(-10), LocationId = 2, Share = 70 , CreatedBy = "admn"},
            new Member { EmpNo = "6011", FirstName = "Jack", LastName = "White", Email = "jack.white@example.com", DOJ = DateTime.UtcNow.AddYears(-11), LocationId = 3, Share = 55 , CreatedBy = "admn"},
            new Member { EmpNo = "6012", FirstName = "Karen", LastName = "Harris", Email = "karen.harris@example.com", DOJ = DateTime.UtcNow.AddYears(-12), LocationId = 4, Share = 65 , CreatedBy = "admn"},
            new Member { EmpNo = "6013", FirstName = "Leo", LastName = "Martin", Email = "leo.martin@example.com", DOJ = DateTime.UtcNow.AddYears(-13), LocationId = 1, Share = 85 , CreatedBy = "admn"},
            new Member { EmpNo = "6014", FirstName = "Mia", LastName = "Garcia", Email = "mia.garcia@example.com", DOJ = DateTime.UtcNow.AddYears(-14), LocationId = 2, Share = 95 , CreatedBy = "admn"},
            new Member { EmpNo = "6015", FirstName = "Nina", LastName = "Martinez", Email = "nina.martinez@example.com", DOJ = DateTime.UtcNow.AddYears(-15), LocationId = 3, Share = 45 , CreatedBy = "admn"},
            new Member { EmpNo = "6016", FirstName = "Oscar", LastName = "Robinson", Email = "oscar.robinson@example.com", DOJ = DateTime.UtcNow.AddYears(-16), LocationId = 4, Share = 35 , CreatedBy = "admn"},
            new Member { EmpNo = "6017", FirstName = "Paul", LastName = "Clark", Email = "paul.clark@example.com", DOJ = DateTime.UtcNow.AddYears(-17), LocationId = 1, Share = 50 , CreatedBy = "admn"},
            new Member { EmpNo = "6018", FirstName = "Quinn", LastName = "Rodriguez", Email = "quinn.rodriguez@example.com", DOJ = DateTime.UtcNow.AddYears(-18), LocationId = 2, Share = 75 , CreatedBy = "admn"},
            new Member { EmpNo = "6019", FirstName = "Rachel", LastName = "Lewis", Email = "rachel.lewis@example.com", DOJ = DateTime.UtcNow.AddYears(-19), LocationId = 3, Share = 85 , CreatedBy = "admn"},
            new Member { EmpNo = "6020", FirstName = "Steve", LastName = "Walker", Email = "steve.walker@example.com", DOJ = DateTime.UtcNow.AddYears(-20), LocationId = 4, Share = 95 , CreatedBy = "admn"}
        };

            _context.Members.AddRange(members);
            await _context.SaveChangesAsync();
        }
    }
}


