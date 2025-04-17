using MassTransit;

namespace Catalog.API.Consumers
{
    public class ReservePlateConsumer : IConsumer<ReservePlateMessage>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public ReservePlateConsumer(ApplicationDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<ReservePlateMessage> messageContext)
        {
            var message = messageContext.Message;
            var plate = _context.Plates.Find(message.PlateId);

            if (plate == null)
            {
                await _publishEndpoint.Publish(new AuditMessage
                {
                    PlateIdReference = message.PlateId,
                    AuditAction = AuditAction.Error,
                    Message = "Plate not found trying to Reserve"
                });

                await messageContext.RespondAsync(new ConsumerResponse
                {
                    Success = false,
                    Message = "Plate not found"
                });
            }

            if (plate.Status == PlateStatus.Reserved || plate.Status == PlateStatus.Sold)
            {
                await _publishEndpoint.Publish(new AuditMessage
                {
                    PlateIdReference = message.PlateId,
                    AuditAction = AuditAction.Error,
                    Message = "Plate could not be Rereved because of its current status"
                });
                await messageContext.RespondAsync(new ConsumerResponse
                {
                    Success = false,
                    Message = "Plate could not be Rereved because of its current status"
                });
            }

            if (string.IsNullOrWhiteSpace(message.ReservedBy))
            {
                await _publishEndpoint.Publish(new AuditMessage
                {
                    PlateIdReference = message.PlateId,
                    AuditAction = AuditAction.Error,
                    Message = "Plate could not be Rereved because ReservedBy is null or empty"
                });
                await messageContext.RespondAsync(new ConsumerResponse
                {
                    Success = false,
                    Message = "Plate could not be Rereved because ReservedBy is null or empty"
                });
            }

            plate.Status = PlateStatus.Reserved;
            plate.ReservedBy = message.ReservedBy;
            _context.SaveChanges();
            await _publishEndpoint.Publish(new AuditMessage
            {
                PlateIdReference = message.PlateId,
                AuditAction = AuditAction.Reserve,
                Message = $"Plate reserved successfully by {message.ReservedBy}"
            });
            await messageContext.RespondAsync(new ConsumerResponse
            {
                Success = true,
                Message = "Plate Reserved Successfully"
            });
        }
    }
}
