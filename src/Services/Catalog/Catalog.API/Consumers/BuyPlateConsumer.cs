using MassTransit;

namespace Catalog.API.Consumers
{
    public class BuyPlateConsumer : IConsumer<BuyPlateMessage>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public BuyPlateConsumer(ApplicationDbContext context, IPublishEndpoint publishEndpoint) 
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<BuyPlateMessage> messageContext)
        {
            var message = messageContext.Message;
            var plate = _context.Plates.Find(message.PlateId);
            if (plate == null)
            {
                await _publishEndpoint.Publish(new AuditMessage
                {
                    PlateIdReference = message.PlateId,
                    AuditAction = AuditAction.Error,
                    Message = "Plate not found trying to Buy"
                });
                await messageContext.RespondAsync(new ConsumerResponse
                {
                    Success = false,
                    Message = "Plate not found"
                });
            }

            //Check that the plate is not already sold
            if (plate.Status == PlateStatus.Sold)
            {
                await _publishEndpoint.Publish(new AuditMessage
                {
                    PlateIdReference = message.PlateId,
                    AuditAction = AuditAction.Error,
                    Message = "Plate could not be Bought because it is already sold"
                });
                await messageContext.RespondAsync(new ConsumerResponse
                {
                    Success = false,
                    Message = "Plate could not be Bought because it is already sold"
                });
            }

            //Check that the plate is not reservered
            if (plate.Status == PlateStatus.Reserved && message.BuyerName != plate.ReservedBy)
            {
                await _publishEndpoint.Publish(new AuditMessage
                {
                    PlateIdReference = message.PlateId,
                    AuditAction = AuditAction.Error,
                    Message = "Plate could not be Bought because it is reserved by another user"
                });
                await messageContext.RespondAsync(new ConsumerResponse
                {
                    Success = false,
                    Message = "Plate could not be Bought because it is reserved by another user"
                });
            }

            if (string.IsNullOrWhiteSpace(message.BuyerName))
            {
                await _publishEndpoint.Publish(new AuditMessage
                {
                    PlateIdReference = message.PlateId,
                    AuditAction = AuditAction.Error,
                    Message = "Plate could not be Bought because BuyerName is null or empty"
                });
                await messageContext.RespondAsync(new ConsumerResponse
                {
                    Success = false,
                    Message = "Plate could not be Bought because BuyerName is null or empty"
                });
            }

            plate.Status = PlateStatus.Sold;
            plate.BoughtBy = message.BuyerName;
            _context.SaveChanges();

            await _publishEndpoint.Publish(new AuditMessage
            {
                PlateIdReference = message.PlateId,
                AuditAction = AuditAction.Buy,
                Message = $"Plate bought by {message.BuyerName}"
            });
            await messageContext.RespondAsync(new ConsumerResponse
            {
                Success = true,
                Message = "Plate successfully bought"
            }); 
        }
    }
}
