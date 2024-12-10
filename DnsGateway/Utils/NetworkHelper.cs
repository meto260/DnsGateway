using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DnsGateway.Utils {
    public static class NetworkHelper {
        /// <summary>
        /// Dış IP adresini otomatik olarak öğrenir.
        /// </summary>
        /// <returns>Dış IP adresi (string) veya null (hata durumunda).</returns>
        public static async Task<string> GetExternalIpAddressAsync() {
            using var httpClient = new HttpClient();
            try {
                var response = await httpClient.GetStringAsync("https://api.ipify.org");
                return response.Trim();
            }
            catch (Exception ex) {
                Console.WriteLine($"Dış IP adresi alınamadı: {ex.Message}");
                return null;
            }
        }
    }
}