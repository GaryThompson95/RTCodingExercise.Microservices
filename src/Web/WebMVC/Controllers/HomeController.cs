using RTCodingExercise.Microservices.Models;
using System.Diagnostics;
using WebMVC.Models;
using WebMVC.Services;

namespace RTCodingExercise.Microservices.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PlateService _plateService;
        private static HomeViewModel viewModel;

        public HomeController(ILogger<HomeController> logger, PlateService plateService)
        {
            _logger = logger;
            _plateService = plateService;
        }

        public async Task<IActionResult> Index(int page = 1, string sortOrder = "default")
        {
            if (sortOrder == "continue")
            {
                sortOrder = viewModel.SortOrder;
            }
            else if(sortOrder != "default")
            {
                //Param sort order will act like a switch, if current desc or default set to asc
                if (viewModel.SortOrder == "price_desc" || viewModel.SortOrder == "default")
                {
                    sortOrder = "price_asc";
                }
                else
                {
                    sortOrder = "price_desc";
                }
            }
            
            viewModel = await _plateService.GetPlatesAsync(page, sortOrder);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Plate plate)
        {
            var characters = plate.Registration?.ToCharArray();
            var numbers = new List<char>();
            var letters = new List<char>();

            if (characters.Length < 1 || characters.Length > 7)
            {
                _logger.LogError("Add Plate Failed - The registration must be between 1 and 7 characters");
                TempData["Message"] = "Add Plate Failed - The registration must be between 1 and 7 characters";
                return RedirectToAction("Index");
            }

            foreach(var character in characters)
            {
                if (!char.IsLetterOrDigit(character))
                {
                    _logger.LogError("Add Plate Failed - The registration can only contain letters and digits");
                    TempData["Message"] = "Add Plate Failed - The registration can only contain letters and digits";
                    return RedirectToAction("Index");
                }
                if(char.IsDigit(character))
                {
                    numbers.Add(character);
                }
                else
                {
                    letters.Add(character);
                }
            }

            if(numbers.Count() == 0)
            {
                _logger.LogError("Add Plate Failed - The registration must contain at least one digit");
                TempData["Message"] = "Add Plate Failed - The registration must contain at least one digit";
                return RedirectToAction("Index");
            }

            plate.Id = Guid.NewGuid();
            if (letters.Count() == 0)
            {
                plate.Letters = null;
            }
            else
            {
                plate.Letters = new string(letters.ToArray());
            }
            plate.Numbers = int.Parse(new string(numbers.ToArray()));
            plate.SalePrice = plate.PurchasePrice * 1.2m;

            await _plateService.CreatePlateAsync(plate);

            TempData["Message"] = "Plate saved successfully!";
            return RedirectToAction("Index");
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}