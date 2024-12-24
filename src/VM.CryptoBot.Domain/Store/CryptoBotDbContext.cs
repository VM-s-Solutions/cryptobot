using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using VM.CryptoBot.Domain.Configurations.Common;
using VM.CryptoBot.Domain.Entities.Common;
using VM.CryptoBot.Domain.Repositories.Interfaces;

namespace VM.CryptoBot.Domain.Store;

public class CryptoBotDbContext : DbContext, IUnitOfWork
{
    private readonly IUserSessionProvider _userSessionProvider;

    public CryptoBotDbContext(IUserSessionProvider userSessionProvider)
    {
        _userSessionProvider = userSessionProvider;
    }

    public CryptoBotDbContext(DbContextOptions dbContextOptions, IUserSessionProvider userSessionProvider)
        : base(dbContextOptions)
    {
        _userSessionProvider = userSessionProvider;
    }

    public void Migrate()
    {
        Database.Migrate();
    }

    public void Seed()
    {
        SaveChanges();
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        var fullUserName = _userSessionProvider.GetTypedUserClaim(ClaimTypes.Name)?.Value;
        var stateUser = string.IsNullOrWhiteSpace(fullUserName) ? "System" : fullUserName;
        foreach (var entity in ChangeTracker.Entries<Auditable<Guid>>())
        {
            if (entity.State == EntityState.Added)
            {
                entity.Entity.Create(stateUser);
            }
            else if (entity.State == EntityState.Modified)
            {
                entity.Entity.Update(stateUser);

                if (!entity.Entity.IsActive)
                {
                    entity.Entity.Deactivate(stateUser);
                }
            }
        }
        return await SaveChangesAsync(cancellationToken);
    }

    public void Rollback()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            entry.State = EntityState.Unchanged;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseEntityConfiguration<,>).Assembly);
    }

    // Entities
}
