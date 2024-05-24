using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.TicketRepository
{
    public class TicketRepository: Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(AppDBContext dbContext) : base(dbContext) { }
    }
}
