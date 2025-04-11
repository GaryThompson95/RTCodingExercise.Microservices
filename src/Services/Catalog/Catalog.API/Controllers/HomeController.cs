namespace Catalog.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
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
                    p.SalePrice
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
    }
}
