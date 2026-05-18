namespace BeeSIS.API.Services.Interfaces
{
    /// <summary>
    /// Interface for CSV data access operations.
    /// ISP: Segregated to only CSV-specific operations.
    /// Repository Pattern: Abstracts the data source from the business logic.
    /// </summary>
    public interface ICsvDataService
    {
        /// <summary>Reads records from a CSV file on GitHub (falls back to local if unavailable).</summary>
        Task<List<T>> ReadCsvFromGitHubAsync<T>(string fileName) where T : class;

        /// <summary>Writes records to a local CSV file.</summary>
        Task WriteCsvToLocalAsync<T>(List<T> records, string fileName) where T : class;

        /// <summary>Reads records from a local CSV file.</summary>
        Task<List<T>> ReadCsvFromLocalAsync<T>(string fileName) where T : class;

        /// <summary>Converts a list of records to a CSV string.</summary>
        string ConvertToCsvString<T>(List<T> records) where T : class;
    }
}
