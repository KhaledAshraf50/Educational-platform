using System.Text;
using System.Text.Json;

namespace Luno_platform.Service
{
    public class PaymobService
    {
        private readonly string _apiKey;
        private readonly int _integrationIdCard;
        private readonly int _integrationIdWallet;
        private readonly int _iframeId;
        private readonly HttpClient _httpClient;
        private readonly string _callbackUrl;

        public PaymobService(IConfiguration configuration, HttpClient httpClient)
        {
            _apiKey = configuration["Paymob:ApiKey"];
            _integrationIdCard = int.Parse(configuration["Paymob:IntegrationIdCard"]);
            _integrationIdWallet = int.Parse(configuration["Paymob:IntegrationIdWallet"]);
            _iframeId = int.Parse(configuration["Paymob:IframeId"]);
            _httpClient = httpClient;
            _callbackUrl = configuration["Paymob:CallbackUrl"] ?? "https://yourdomain.com/Payment/PaymentCallback";
        }

        // 1️⃣ الحصول على Token
        public async Task<string> GetAuthTokenAsync()
        {
            try
            {
                var request = new { api_key = _apiKey };

                var response = await _httpClient.PostAsync(
                    "https://accept.paymob.com/api/auth/tokens",
                    new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
                );

                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"فشل الحصول على Token من Paymob: {content}");
                }

                var result = JsonSerializer.Deserialize<JsonElement>(content);

                if (!result.TryGetProperty("token", out var tokenProperty))
                {
                    throw new Exception("Paymob API لم يعيد Token صحيح");
                }

                return tokenProperty.GetString();
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في الاتصال بـ Paymob: {ex.Message}");
            }
        }

        // 2️⃣ إنشاء Order
        private async Task<int> CreateOrderAsync(string token, decimal amount, string orderId)
        {
            try
            {
                var request = new
                {
                    auth_token = token,
                    delivery_needed = "false",
                    amount_cents = (int)(amount * 100),
                    currency = "EGP",
                    merchant_order_id = orderId,
                    items = new[]
                    {
                        new
                        {
                            name = "item",
                            amount_cents = (int)(amount * 100),
                            description = "Payment",
                            quantity = 1
                        }
                    }

                };

                var response = await _httpClient.PostAsync(
                    "https://accept.paymob.com/api/ecommerce/orders",
                    new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
                );

                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"فشل إنشاء Order في Paymob: {content}");
                }

                var result = JsonSerializer.Deserialize<JsonElement>(content);
                return result.GetProperty("id").GetInt32();
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في إنشاء الطلب: {ex.Message}");
            }
        }

        // 3️⃣ الحصول على Payment Key
        private async Task<string> GetPaymentKeyAsync(
            string token,
            int orderId,
            decimal amount,
            int integrationId,
            string firstName,
            string lastName,
            string email,
            string phone)
        {
            var request = new
            {
                auth_token = token,
                amount_cents = (int)(amount * 100),
                expiration = 3600,
                order_id = orderId,
                billing_data = new
                {
                    first_name = firstName,
                    last_name = lastName,
                    email = email,
                    phone_number = phone,
                    apartment = "NA",
                    floor = "NA",
                    street = "NA",
                    building = "NA",
                    shipping_method = "NA",
                    postal_code = "NA",
                    city = "Cairo",
                    country = "EG",
                    state = "NA"
                },
                currency = "EGP",
                integration_id = integrationId
            };

            var response = await _httpClient.PostAsync(
                "https://accept.paymob.com/api/acceptance/payment_keys",
                new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
                throw new Exception("فشل الحصول على Payment Key");

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            return result.GetProperty("token").GetString();
        }

        // 4️⃣ إنشاء رابط دفع بالبطاقة
        public async Task<string> CreateCardPaymentLinkAsync(
            decimal amount,
            string orderId,
            string firstName,
            string lastName,
            string email,
            string phone)
        {
            var token = await GetAuthTokenAsync();
            var paymobOrderId = await CreateOrderAsync(token, amount, orderId);
            var paymentKey = await GetPaymentKeyAsync(
                token, paymobOrderId, amount, _integrationIdCard,
                firstName, lastName, email, phone
            );

            return $"https://accept.paymob.com/api/acceptance/iframes/{_iframeId}?payment_token={paymentKey}";
        }

        // 5️⃣ إنشاء رابط دفع بالمحفظة
        public async Task<string> CreateWalletPaymentLinkAsync(
            decimal amount,
            string orderId,
            string firstName,
            string lastName,
            string email,
            string phone)
        {
            var token = await GetAuthTokenAsync();
            var paymobOrderId = await CreateOrderAsync(token, amount, orderId);
            var paymentKey = await GetPaymentKeyAsync(
                token, paymobOrderId, amount, _integrationIdWallet,
                firstName, lastName, email, phone
            );

            return $"https://accept.paymob.com/api/acceptance/payments/pay?payment_token={paymentKey}";
        }
    }
}