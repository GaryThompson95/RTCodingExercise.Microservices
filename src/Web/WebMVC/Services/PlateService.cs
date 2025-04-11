using WebMVC.Interfaces;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class PlateService
    {
        private readonly IHttpClientWrapper _httpClientWrapper;

        public PlateService(IHttpClientWrapper httpClientWrapper)
        {
            _httpClientWrapper = httpClientWrapper;
        }

        public async Task<HomeViewModel> GetPlatesAsync(int page, string sortOrder)
        {
            try
            {
                var response = await _httpClientWrapper.GetAsync($"plates?page={page}&sort_order={sortOrder}");
                response.EnsureSuccessStatusCode();

                var foundPlates = await response.Content.ReadFromJsonAsync<IEnumerable<Plate>>();
                bool hasNext = false;

                if (foundPlates.Count() == 21)
                {
                    hasNext = true;
                    foundPlates = foundPlates.Take(20);
                }

                return new HomeViewModel
                {
                    Plates = foundPlates,
                    HasNext = hasNext,
                    CurrentPage = page,
                    SortOrder = sortOrder
                };
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public virtual async Task CreatePlateAsync(Plate plate)
        {
            try
            {
                var response = await _httpClientWrapper.PostAsync("plate", JsonContent.Create(plate));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
