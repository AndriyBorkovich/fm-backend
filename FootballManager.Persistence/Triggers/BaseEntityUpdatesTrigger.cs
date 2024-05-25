using EntityFrameworkCore.Triggered;
using FootballManager.Domain.Common;
using FootballManager.Persistence.DatabaseContext;

namespace FootballManager.Persistence.Triggers;

public class BaseEntityUpdatesTrigger : IBeforeSaveAsyncTrigger<BaseEntity>
{
    private readonly FootballManagerContext _context;

    public BaseEntityUpdatesTrigger(FootballManagerContext context)
    {
        _context = context;
    }

    public async Task BeforeSaveAsync(ITriggerContext<BaseEntity> context, CancellationToken cancellationToken)
    {
        switch (context.ChangeType)
        {
            case ChangeType.Added:
                context.Entity.CreatedDate = DateTime.UtcNow;
                break;
            case ChangeType.Modified:
                context.Entity.UpdatedDate = DateTime.UtcNow;
                break;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}