using SME_API_GDX.Models;

using SME_API_GDX.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SME_API_GDX.Services
{
    public class CallAPIService : ICallAPIService
    {
        private readonly HttpClient _httpClient;
        private readonly string Api_ErrorLog;
        private readonly string _FlagDev;
        private readonly IApiInformationRepository _repositoryApi;
        private readonly string Api_SysCode;

        public CallAPIService(HttpClient httpClient, IConfiguration configuration, IApiInformationRepository repositoryApi)
        {
            _httpClient = httpClient;
            Api_ErrorLog = configuration["Information:Api_ErrorLog"] ?? throw new ArgumentNullException("Api_ErrorLog is missing in appsettings.json");
            _FlagDev = configuration["Devlopment:FlagDev"] ?? throw new ArgumentNullException("FlagDev is missing in appsettings.json");
            Api_SysCode = configuration["Information:Api_SysCode"] ?? throw new ArgumentNullException("Api_SysCode is missing in appsettings.json");
            _repositoryApi = repositoryApi ?? throw new ArgumentNullException(nameof(repositoryApi));
        }

        public async Task<string> GetDataApiAsync_Login(MapiInformationModels apiModels)
        {
            if (string.IsNullOrEmpty(apiModels.Urlproduction))
                throw new ArgumentException("Urlproduction cannot be null or empty.");
            if (string.IsNullOrEmpty(apiModels.Username) || string.IsNullOrEmpty(apiModels.Password))
                throw new ArgumentException("Username and Password cannot be null or empty.");

            string requestJson = "";
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, apiModels.Urlproduction);

                // Prepare the payload for the token request
                var payload = new
                {
                    username = apiModels.Username,
                    password = apiModels.Password
                };

                // Serialize the payload to JSON and set it as the request content
                requestJson = JsonSerializer.Serialize(payload, options);
                request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                // Send the request using the existing _httpClient
                var response = await _httpClient.SendAsync(request);

                // Ensure the response is successful
                response.EnsureSuccessStatusCode();

                // Read the response content
                var content = await response.Content.ReadAsStringAsync();

                // Parse the JSON response to extract the token
                using var doc = JsonDocument.Parse(content);
                if (!doc.RootElement.TryGetProperty("access_token", out var tokenElement))
                    throw new Exception("Token not found in response");

                // update token
                var token = tokenElement.GetString();
                _repositoryApi.UpdateAllBearerTokensAsync(token);

                return tokenElement.GetString() ?? throw new Exception("Token is null or empty");
            }
            catch (Exception ex)
            {
                var errorLog = new ErrorLogModels
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    TargetSite = ex.TargetSite?.ToString(),
                    ErrorDate = DateTime.Now,
                    UserName = apiModels.Username,
                    Path = apiModels.Urlproduction,
                    HttpMethod = apiModels.MethodType,
                    RequestData = requestJson,
                    InnerException = ex.InnerException?.ToString(),
                   SystemCode = Api_SysCode,
                    CreatedBy = "system"
                };
                await RecErrorLogApiAsync(apiModels, errorLog);
                throw new Exception("Error in GetDataApiAsync_Login: " + ex.Message, ex);
            }
        }

        public async Task RecErrorLogApiAsync(MapiInformationModels apiModels, ErrorLogModels eModels)
        {

            // ✅ เลือก URL ตาม _FlagDev
            string? apiUrl = Api_ErrorLog;



            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);

                // ✅ ใส่ API Key ถ้ามี
                if (!string.IsNullOrEmpty(apiModels.ApiKey))
                    request.Headers.Add("X-Api-Key", apiModels.ApiKey);

                // ✅ ใส่ Basic Authentication ถ้ามี Username & Password
                if (!string.IsNullOrEmpty(apiModels.Username) && !string.IsNullOrEmpty(apiModels.Password))
                {
                    var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiModels.Username}:{apiModels.Password}"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
                }

                // ✅ แปลง SendData เป็น JSON และแนบไปกับ Body ของ Request
                var jsonData = JsonSerializer.Serialize(eModels);
                request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // ✅ Call API และรอผลลัพธ์
                using var response = await _httpClient.SendAsync(request);
                string responseData = await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {

            }
        }
        public async Task<T?> ReadJsonFileAsync<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{filePath}' was not found.");
            }

            var jsonContent = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<T>(jsonContent);
        }
        public bool IsTokenExpired(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            if (jwtToken == null) return true;

            var expiry = jwtToken.ValidTo;
            return expiry < DateTime.UtcNow;
        }
        public async Task<string> GetTokenAsync(string username, string password, string tokenUrl)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl);

            // Prepare the payload for the token request
            var payload = new
            {
                username = username,
                password = password
            };

            // Serialize the payload to JSON and set it as the request content
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            try
            {
                // Send the request using the existing _httpClient
                var response = await _httpClient.SendAsync(request);

                // Ensure the response is successful
                response.EnsureSuccessStatusCode();

                // Read the response content
                var content = await response.Content.ReadAsStringAsync();

                // Parse the JSON response to extract the token
                using var doc = JsonDocument.Parse(content);
                var token = doc.RootElement.GetProperty("access_token").GetString();

                return token ?? throw new Exception("Token not found in response");
            }
            catch (Exception ex)
            {
                // Log the error or handle it as needed
                throw new Exception($"Failed to retrieve token: {ex.Message}", ex);
            }
        }
        public async Task<string> GetDataApiAsync(MapiInformationModels apiModels, object xdata)
        {
            string requestJson = "";
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            try
            {
                using var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                using var httpClient = new HttpClient(handler);

                // Construct URL
                var url = apiModels.Urlproduction ?? throw new Exception("URL is missing.");
                requestJson = url;

                // Create request
                var request = new HttpRequestMessage(
                    apiModels.MethodType == "POST" ? HttpMethod.Post : HttpMethod.Get,
                    url
                );

                // Set Authorization Headers
                if (apiModels.AuthorizationType == "Basic")
                {
                    var byteArray = System.Text.Encoding.ASCII.GetBytes($"{apiModels.Username}:{apiModels.Password}");
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                }
                else if (apiModels.AuthorizationType == "Bearer")
                {
                    string? token = apiModels.Bearer;

                    // Check if token is expired
                    if (!string.IsNullOrEmpty(token) && IsTokenExpired(token))
                    {
                        var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "user-api" });

                        var apiParamx = LApi.Select(x => new MapiInformationModels
                        {
                            ServiceNameCode = x.ServiceNameCode,
                            ApiKey = x.ApiKey,
                            AuthorizationType = x.AuthorizationType,
                            ContentType = x.ContentType,
                            CreateDate = x.CreateDate,
                            Id = x.Id,
                            MethodType = x.MethodType,
                            ServiceNameTh = x.ServiceNameTh,
                            Urldevelopment = x.Urldevelopment,
                            Urlproduction = x.Urlproduction,
                            Username = x.Username,
                            Password = x.Password,
                            UpdateDate = x.UpdateDate,
                            Bearer = x.Bearer,
                            AccessToken = x.AccessToken,
                            ConsumerKey = x.ConsumerKey,
                            ConsumerSecret = x.ConsumerSecret,
                            AgentId = x.AgentId
                        }).FirstOrDefault(); // Use FirstOrDefault to handle empty lists


                        var result = await GetDataApiAsync_Login(apiParamx);
                        token = result;
                      var sendReply =await  GetDataApiAsync( apiModels, xdata);
                    }

                    if (string.IsNullOrEmpty(token))
                    {
                        throw new Exception("Bearer token is missing or expired.");
                    }

                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                else if (apiModels.AuthorizationType == "ApiKey")
                {
                    request.Headers.Add("X-Api-Key", apiModels.ApiKey);
                }
                else if (apiModels.AuthorizationType == "Token")
                {
                    string strcuskey= DecodeBase64(apiModels.ConsumerKey);
                   string strToken = DecodeBase64(apiModels.Token);

                    request.Headers.Add("Consumer-Key", strcuskey);
                    request.Headers.Add("Token", strToken);
                }
                else
                {
                    throw new Exception("Authorization type not supported");
                }

                // Add JSON body for POST requests
                if (apiModels.MethodType == "POST")
                {
                    if (xdata != null)
                    {
                        var jsonBody = JsonSerializer.Serialize(xdata, options);
                        requestJson = jsonBody; // For logging
                        request.Content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
                    }
                    else
                    {
                        throw new Exception("POST request must have a JSON body.");
                    }
                }

                // Send Request
                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var jsonNode = JsonNode.Parse(content);

      
                return jsonNode?.ToJsonString(new JsonSerializerOptions { WriteIndented = true }) ?? "{}";
            }
            catch (Exception ex)
            {
                if (ex.Message == "Response status code does not indicate success: 401 (Unauthorized).")
                {
                    var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "Refresh-Token" });

                    var apiParamx = LApi.Select(x => new MapiInformationModels
                    {
                        ServiceNameCode = x.ServiceNameCode,
                        ApiKey = x.ApiKey,
                        AuthorizationType = x.AuthorizationType,
                        ContentType = x.ContentType,
                        CreateDate = x.CreateDate,
                        Id = x.Id,
                        MethodType = x.MethodType,
                        ServiceNameTh = x.ServiceNameTh,
                        Urldevelopment = x.Urldevelopment,
                        Urlproduction = x.Urlproduction,
                        Username = x.Username,
                        Password = x.Password,
                        UpdateDate = x.UpdateDate,
                        Bearer = x.Bearer,
                        AccessToken = x.AccessToken,
                        ConsumerKey = x.ConsumerKey,
                        ConsumerSecret = x.ConsumerSecret,
                        AgentId = x.AgentId
                    }).FirstOrDefault(); // Use FirstOrDefault to handle empty lists
                                         // To do refresho token
                    var getRefresh = await GetGDXRefreshtokenAsync_Login(apiParamx);

                    return null; // หรือจัดการกับการรีเฟรชโทเคนตามที่คุณต้องการ
                }
                else 
                {
                    var errorLog = new ErrorLogModels
                    {
                        Message = "Function " + apiModels.ServiceNameTh + " " + ex.Message,
                        StackTrace = ex.StackTrace,
                        Source = ex.Source,
                        TargetSite = ex.TargetSite?.ToString(),
                        ErrorDate = DateTime.Now,
                        UserName = apiModels.Username, // ดึงจาก context หรือ session
                        Path = apiModels.Urlproduction,
                        HttpMethod = apiModels.MethodType,
                        RequestData = requestJson, // serialize เป็น JSON
                        InnerException = ex.InnerException?.ToString(),
                        SystemCode = Api_SysCode,

                        CreatedBy = "system"
        ,
                        HttpCode = "401"
                    };
                    await RecErrorLogApiAsync(apiModels, errorLog);
                    throw new Exception("Error in GetData: " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message);
                }

            }
        }
        public async Task<string> GetGDXRefreshtokenAsync_Login(MapiInformationModels apiModels)
        {
            if (string.IsNullOrEmpty(apiModels.Urlproduction))
                throw new ArgumentException("Urlproduction cannot be null or empty.");
            if (string.IsNullOrEmpty(apiModels.ConsumerSecret) || string.IsNullOrEmpty(apiModels.AgentId))
                throw new ArgumentException("ConsumerSecret and AgentId cannot be null or empty.");

            string requestJson = "";
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            string customerKey = DecodeBase64(apiModels.ConsumerKey);
            string agentId = DecodeBase64(apiModels.AgentId);
            string consumerSecret = DecodeBase64(apiModels.ConsumerSecret);

            string strUrl = string.IsNullOrEmpty(apiModels.Urlproduction)
    ? throw new ArgumentNullException("Urlproduction is null")
    : apiModels.Urlproduction.Replace("{ConsumerSecret}", consumerSecret).Replace("{AgentID}", agentId)
            ;

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, strUrl);

                // Prepare the payload for the token request
                request.Headers.Add("Consumer-Key", customerKey);
                request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                // Send the request using the existing _httpClient
                var response = await _httpClient.SendAsync(request);

                // Ensure the response is successful
                response.EnsureSuccessStatusCode();

                // Read the response content
                var content = await response.Content.ReadAsStringAsync();

                // Parse the JSON response to extract the token
                using var doc = JsonDocument.Parse(content);
                if (!doc.RootElement.TryGetProperty("Result", out var tokenElement))
                    throw new Exception("Token not found in response");

                // update token
                var token = tokenElement.GetString();
             await   _repositoryApi.UpdateGDXTokensAsync(token);

                return tokenElement.GetString() ?? throw new Exception("Token is null or empty");
            }
            catch (Exception ex)
            {
                var errorLog = new ErrorLogModels
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    TargetSite = ex.TargetSite?.ToString(),
                    ErrorDate = DateTime.Now,
                    UserName = apiModels.Username,
                    Path = apiModels.Urlproduction,
                    HttpMethod = apiModels.MethodType,
                    RequestData = requestJson,
                    InnerException = ex.InnerException?.ToString(),
                   SystemCode = Api_SysCode,
                    CreatedBy = "system"
                };
                await RecErrorLogApiAsync(apiModels, errorLog);
                throw new Exception("Error in GetDataApiAsync_Login: " + ex.Message, ex);
            }
        }

        public static string EncodeBase64(string plainText)
        {
            if (plainText == null) return string.Empty;
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainBytes);
        }

        public static string DecodeBase64(string base64Text)
        {
            if (string.IsNullOrEmpty(base64Text)) return string.Empty;
            var base64Bytes = Convert.FromBase64String(base64Text);
            return Encoding.UTF8.GetString(base64Bytes);
        }
    }
}
