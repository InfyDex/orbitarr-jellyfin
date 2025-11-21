#nullable enable

using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orbitarr.Jellyfin.Plugin.Configuration;

namespace Orbitarr.Jellyfin.Plugin.Api
{
    /// <summary>
    /// Client for communicating with the Orbitarr API.
    /// </summary>
    public class OrbitarrApiClient
    {
        private readonly ILogger<OrbitarrApiClient> _logger;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrbitarrApiClient"/> class.
        /// </summary>
        /// <param name="logger">Instance of the <see cref="ILogger{OrbitarrApiClient}"/> interface.</param>
        public OrbitarrApiClient(ILogger<OrbitarrApiClient> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        /// <summary>
        /// Tracks an episode watch event.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="tvShowId">The TV show ID.</param>
        /// <param name="seasonNumber">The season number.</param>
        /// <param name="episodeNumber">The episode number.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task TrackEpisodeAsync(string userId, int tvShowId, int seasonNumber, int episodeNumber)
        {
            try
            {
                var config = Plugin.Instance?.Configuration;
                if (config == null || !config.Enabled)
                {
                    _logger.LogDebug("Plugin is not enabled, skipping episode tracking");
                    return;
                }

                if (string.IsNullOrWhiteSpace(config.ApiBaseUrl))
                {
                    _logger.LogWarning("API Base URL is not configured");
                    return;
                }

                if (string.IsNullOrWhiteSpace(config.UserId))
                {
                    _logger.LogWarning("User ID is not configured");
                    return;
                }

                var payload = new
                {
                    user_id = config.UserId,
                    tv_show_id = tvShowId,
                    season_number = seasonNumber,
                    episode_number = episodeNumber
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{config.ApiBaseUrl.TrimEnd('/')}/v1/episodes";
                _logger.LogInformation("Tracking episode: Show={TvShowId}, Season={Season}, Episode={Episode} to {Url}", 
                    tvShowId, seasonNumber, episodeNumber, url);

                var response = await _httpClient.PostAsync(url, content);
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully tracked episode");
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to track episode. Status: {StatusCode}, Response: {Response}", 
                        response.StatusCode, responseBody);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking episode");
            }
        }
    }
}
