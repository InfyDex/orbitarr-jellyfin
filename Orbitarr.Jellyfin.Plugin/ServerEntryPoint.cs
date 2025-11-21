#nullable enable

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Session;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orbitarr.Jellyfin.Plugin.Api;

namespace Orbitarr.Jellyfin.Plugin
{
    /// <summary>
    /// Server entry point for the plugin that listens to playback events.
    /// </summary>
    public class ServerEntryPoint : IHostedService, IDisposable
    {
        private readonly ISessionManager _sessionManager;
        private readonly IUserDataManager _userDataManager;
        private readonly ILogger<ServerEntryPoint> _logger;
        private readonly OrbitarrApiClient _apiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerEntryPoint"/> class.
        /// </summary>
        /// <param name="sessionManager">Instance of the <see cref="ISessionManager"/> interface.</param>
        /// <param name="userDataManager">Instance of the <see cref="IUserDataManager"/> interface.</param>
        /// <param name="logger">Instance of the <see cref="ILogger{ServerEntryPoint}"/> interface.</param>
        /// <param name="apiLogger">Instance of the <see cref="ILogger{OrbitarrApiClient}"/> interface.</param>
        public ServerEntryPoint(
            ISessionManager sessionManager,
            IUserDataManager userDataManager,
            ILogger<ServerEntryPoint> logger,
            ILogger<OrbitarrApiClient> apiLogger)
        {
            _sessionManager = sessionManager;
            _userDataManager = userDataManager;
            _logger = logger;
            _apiClient = new OrbitarrApiClient(apiLogger);
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _sessionManager.PlaybackStopped += OnPlaybackStopped;
            _userDataManager.UserDataSaved += OnUserDataSaved;
            _logger.LogInformation("Orbitarr Episode Tracker plugin started");
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _sessionManager.PlaybackStopped -= OnPlaybackStopped;
            _userDataManager.UserDataSaved -= OnUserDataSaved;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Handles the playback stopped event.
        /// </summary>
        private async void OnPlaybackStopped(object? sender, PlaybackStopEventArgs e)
        {
            try
            {
                // Only track episodes
                if (e.Item is not Episode episode)
                {
                    return;
                }

                // Check if the episode was actually watched (at least 90% completed)
                if (e.PlayedToCompletion || (e.PlaybackPositionTicks.HasValue && 
                    episode.RunTimeTicks.HasValue && 
                    episode.RunTimeTicks > 0 &&
                    (double)e.PlaybackPositionTicks.Value / episode.RunTimeTicks.Value >= 0.9))
                {
                    await TrackEpisode(episode, e.Session.UserId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling playback stopped event");
            }
        }

        /// <summary>
        /// Handles the user data saved event (when user manually marks as watched).
        /// </summary>
        private async void OnUserDataSaved(object? sender, UserDataSaveEventArgs e)
        {
            try
            {
                // Only track episodes that were marked as played
                if (e.Item is not Episode episode || !e.UserData.Played)
                {
                    return;
                }

                await TrackEpisode(episode, e.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling user data saved event");
            }
        }

        /// <summary>
        /// Tracks an episode to the Orbitarr API.
        /// </summary>
        private async Task TrackEpisode(Episode episode, Guid userId)
        {
            var series = episode.Series;
            if (series == null)
            {
                _logger.LogWarning("Episode has no parent series");
                return;
            }

            // Extract TV show ID from provider IDs (TMDB, TVDB, etc.)
            int tvShowId = 0;
            if (series.ProviderIds.TryGetValue("Tmdb", out var tmdbId) && int.TryParse(tmdbId, out var parsedTmdbId))
            {
                tvShowId = parsedTmdbId;
            }
            else if (series.ProviderIds.TryGetValue("Tvdb", out var tvdbId) && int.TryParse(tvdbId, out var parsedTvdbId))
            {
                tvShowId = parsedTvdbId;
            }
            else
            {
                _logger.LogWarning("No TMDB or TVDB ID found for series: {SeriesName}", series.Name);
                return;
            }

            var seasonNumber = episode.ParentIndexNumber ?? 0;
            var episodeNumber = episode.IndexNumber ?? 0;

            _logger.LogInformation("Episode watched: {SeriesName} S{Season}E{Episode}", 
                series.Name, seasonNumber, episodeNumber);

            await _apiClient.TrackEpisodeAsync(
                userId.ToString(),
                tvShowId,
                seasonNumber,
                episodeNumber);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Cleanup if needed
            }
        }
    }
}
