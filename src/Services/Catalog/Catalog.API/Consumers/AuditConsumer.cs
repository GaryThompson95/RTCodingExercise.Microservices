using MassTransit;

namespace Catalog.API.Consumers
{
    public class AuditConsumer : IConsumer<AuditMessage>
    {
        private readonly ApplicationDbContext _context;
        public AuditConsumer(ApplicationDbContext context) 
        {
            _context = context;
        }

        public Task Consume(ConsumeContext<AuditMessage> messageContext)
        {
            var message = messageContext.Message;
            var audit = new Audit
            {
                Id = Guid.NewGuid(),
                PlateIdReference = message.PlateIdReference,
                AuditAction = message.AuditAction,
                Message = message.Message,
                DateTime = DateTime.UtcNow,
            };

            _context.Audits.Add(audit);
            _context.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
