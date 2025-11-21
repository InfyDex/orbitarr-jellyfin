using MediaBrowser.Model.Plugins;

namespace Orbitarr.Jellyfin.Plugin.Configuration
{
    /// <summary>
    /// Plugin configuration.
    /// </summary>
    public class PluginConfiguration : BasePluginConfiguration
    {
        /// <summary>
        /// Gets or sets the API base URL (hardcoded).
        /// </summary>
        public string ApiBaseUrl { get; set; } = "http://192.168.1.12:8080";

        /// <summary>
        /// Gets or sets the user ID to track episodes for.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the plugin is enabled.
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
}
