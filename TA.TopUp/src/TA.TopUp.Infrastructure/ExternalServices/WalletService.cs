using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TA.TopUp.Shared.DTOs.Response;
using TA.TopUp.Shared.Options;

namespace TA.TopUp.Infrastructure.ExternalServices
{
    public class WalletService
    {
        private readonly HttpClient _httpClient;
        private readonly WalletServiceConfig _walletServiceConfig;
        public WalletService(HttpClient httpClient, IOptionsMonitor<WalletServiceConfig> walletServiceConfig) 
        {
            _httpClient = httpClient;
            _walletServiceConfig = walletServiceConfig.CurrentValue;
        }

        public async Task<WalletBalanceSuccessResponse> GetWalletBalance(int userId)
        {
            WalletBalanceSuccessResponse responseContent = new WalletBalanceSuccessResponse();
            try
            {
                using var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(_walletServiceConfig.WalletBalanceUrl),
                    Headers =
                {
                    {"userId", userId.ToString()},
                    {"Accept", "application/json"}

                }
                };

                var response = await _httpClient.SendAsync(httpRequest);
                if (!response.IsSuccessStatusCode)
                {
                    var failedResponse = await response.Content.ReadFromJsonAsync<WalletBalanceFailureResponse>();
                    throw new Exception(failedResponse.Message);
                }

                responseContent = await response.Content.ReadFromJsonAsync<WalletBalanceSuccessResponse>();

            }
            catch(Exception ex)
            {
                throw;
            }


            return responseContent;
        }
    }
}
