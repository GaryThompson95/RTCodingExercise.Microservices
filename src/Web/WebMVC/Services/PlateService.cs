using MassTransit;
using WebMVC.Interfaces;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class PlateService
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IRequestClient<ReservePlateMessage> _reserveEndpoint;
        private readonly IRequestClient<BuyPlateMessage> _buyEndpoint;

        public PlateService(IHttpClientWrapper httpClientWrapper, IRequestClient<ReservePlateMessage> reserveEndpoint, IRequestClient<BuyPlateMessage> buyEndpoint)
        {
            _httpClientWrapper = httpClientWrapper;
            _reserveEndpoint = reserveEndpoint;
            _buyEndpoint = buyEndpoint;
        }

        public async Task<HomeViewModel> GetPlatesAsync(int page, string sortOrder, bool onlyForSale)
        {
            try
            {
                var response = await _httpClientWrapper.GetAsync($"plates?page={page}&sort_order={sortOrder}&only_for_sale={onlyForSale}");
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
                    SortOrder = sortOrder,
                    OnlyShowForSale = onlyForSale
                };
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<ConsumerResponse> ReservePlateAsync(Guid plateId, string reservedBy)
        {
            try
            {
                var response = await _reserveEndpoint.GetResponse<ConsumerResponse>(new ReservePlateMessage
                {
                    PlateId = plateId,
                    ReservedBy = reservedBy,
                });

                return response.Message;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ConsumerResponse> BuyPlateAsync(Guid plateId, string buyerName)
        {
            try
            {
                var response = await _buyEndpoint.GetResponse<ConsumerResponse>(new BuyPlateMessage
                {
                    PlateId = plateId,
                    BuyerName = buyerName,
                });

                return response.Message;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<decimal> GetTotalRevenue()
        {
            try
            {
                var response = await _httpClientWrapper.GetAsync("plates/totalRevenue");
                response.EnsureSuccessStatusCode();
                var resultString = await response.Content.ReadAsStringAsync();

                var result = decimal.Parse(resultString);

                return result;
            }
            catch (Exception ex)
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
