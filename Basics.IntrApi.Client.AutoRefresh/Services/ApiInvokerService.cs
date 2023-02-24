using Basics.IntrApi.Client.AutoRefresh.Models;

namespace Basics.IntrApi.Client.AutoRefresh.Services
{
    public class ApiInvokerService
    {
        private readonly HttpClient _httpClient;
        public ApiInvokerService(
                HttpClient httpClient
            )
        {
            _httpClient = httpClient;
        }


        public async Task<DataResponse?> Get(string path)
        {
            DataResponse? response = await _httpClient.GetFromJsonAsync<DataResponse>(path);
            return response;
        }

    }
}
