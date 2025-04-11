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
        public IActionResult GetPlates(int page)
        {
            try
            {
                var plates = _context.Plates
                    .Select(p => new
                    {
                        p.Id,
                        p.Registration,
                        p.PurchasePrice,
                        p.SalePrice
                    })
                    .OrderBy(p => p.Id)
                    .Skip((page - 1) * 20)
                    //Take an additional plate to check if there is a next page
                    .Take(21)
                    .ToList();
                return Ok(plates);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
