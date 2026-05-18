using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using BeeSIS.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BeeSIS.API.Services.Implementations
{
    /// <summary>
    /// Implements CSV data access operations.
    /// Repository Pattern: Abstracts all CSV read/write operations.
    /// SRP: Only handles CSV data access — no business logic here.
    /// Adapter Pattern: CsvHelper maps CSV rows to C# objects.
    /// </summary>
    public class CsvDataService : ICsvDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CsvDataService> _logger;
        private readonly string _gitHubRawUrl;
        private readonly string _localDataPath;

        public CsvDataService(HttpClient httpClient, ILogger<CsvDataService> logger, IConfiguration config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _gitHubRawUrl = config["GitHub:RawContentUrl"]
                ?? "https://raw.githubusercontent.com/vtniscoding/beeSIS/main/Data";
            _localDataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(_localDataPath);
        }

        /// <summary>
        /// Reads CSV from GitHub raw URL.
        /// Falls back to local file if GitHub is unreachable.
        /// </summary>
        public async Task<List<T>> ReadCsvFromGitHubAsync<T>(string fileName) where T : class
        {
            try
            {
                var url = $"{_gitHubRawUrl}/{fileName}";
                _logger.LogInformation("Fetching CSV from GitHub: {Url}", url);

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return ParseCsv<T>(content);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("GitHub unavailable ({Message}). Falling back to local file: {File}", ex.Message, fileName);
                return await ReadCsvFromLocalAsync<T>(fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading CSV from GitHub: {File}", fileName);
                return await ReadCsvFromLocalAsync<T>(fileName);
            }
        }

        /// <summary>
        /// Reads CSV from a local Data/ folder.
        /// </summary>
        public async Task<List<T>> ReadCsvFromLocalAsync<T>(string fileName) where T : class
        {
            var filePath = Path.Combine(_localDataPath, fileName);

            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Local CSV file not found: {FilePath}", filePath);
                return new List<T>();
            }

            var content = await File.ReadAllTextAsync(filePath);
            return ParseCsv<T>(content);
        }

        /// <summary>
        /// Writes records to a local CSV file.
        /// </summary>
        public async Task WriteCsvToLocalAsync<T>(List<T> records, string fileName) where T : class
        {
            var filePath = Path.Combine(_localDataPath, fileName);

            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                };

                using var writer = new StreamWriter(filePath, append: false);
                using var csv = new CsvWriter(writer, config);
                await csv.WriteRecordsAsync(records);

                _logger.LogInformation("CSV saved to: {FilePath}", filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error writing CSV to: {FilePath}", filePath);
                throw;
            }
        }

        /// <summary>
        /// Converts a list of records to a CSV formatted string (for download endpoint).
        /// </summary>
        public string ConvertToCsvString<T>(List<T> records) where T : class
        {
            using var writer = new StringWriter();
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(records);
            return writer.ToString();
        }

        // --- Private Helpers ---

        private List<T> ParseCsv<T>(string content) where T : class
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null, // Ignore missing fields
                HeaderValidated = null,   // Skip header validation
            };

            using var reader = new StringReader(content);
            using var csv = new CsvReader(reader, config);
            return csv.GetRecords<T>().ToList();
        }
    }
}
