using MassTransit;
using Polly;

namespace Catalog.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost("plate")]
        public IActionResult GetPlates(Plate plate)
        {
            try
            {
                var plates = _context.Plates.Add(plate);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("plates")]
        public IActionResult GetPlates(int page, string sort_order = "default")
        {
            try
            {
                var query = _context.Plates.AsQueryable().Select(p => new
                {
                    p.Id,
                    p.Registration,
                    p.PurchasePrice,
                    p.SalePrice,
                    p.Status
                });

                //Determine sort order
                switch (sort_order)
                {
                    case "price_asc":
                        query = query.OrderBy(p => p.PurchasePrice);
                        break;
                    case "price_desc":
                        query = query.OrderByDescending(p => p.PurchasePrice);
                        break;
                    default:
                        query = query.OrderBy(p => p.Id);
                        break;
                }

                //Take an additional plate to check if there is a next page
                query = query.Skip((page - 1) * 20).Take(21);

                //Execute query
                var plates = query.ToList();

                return Ok(plates);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("plates/totalRevenue")]
        public IActionResult GetTotalRevenue()
        {
            try
            {
                var totalRevenue = _context.Plates
                  .Where(p => p.Status == PlateStatus.Sold)
                  .Sum(p => p.SalePrice);

                return Ok(totalRevenue);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //Thought this would be an admin operation and thus didn't hook this up to the UI. 
        //Would implement it in the UI with a User login system
        [HttpPut("plate/unreserve")]
        public IActionResult UnReservePlate(Guid plateId)
        {
            try
            {
                var plate = _context.Plates.Find(plateId);

                if(plate == null)
                {
                    return NotFound("Plate not found");
                }

                if(plate.Status != PlateStatus.Reserved)
                {
                    return BadRequest("Plate is not reserved");
                }

                if(plate.Status == PlateStatus.Sold)
                {
                    return BadRequest("Plate is already sold");
                }

                plate.Status = PlateStatus.Available;
                plate.ReservedBy = null;

                _context.SaveChanges();

                _publishEndpoint.Publish(new AuditMessage
                {
                    PlateIdReference = plateId,
                    AuditAction = AuditAction.Reserve,
                    Message = $"Plate unreserved successfully"
                });

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
