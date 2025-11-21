# Orbitarr Jellyfin Plugin

A Jellyfin plugin that automatically tracks episode watch events and sends them to your Orbitarr API.

## Features

- Automatically detects when a user finishes watching an episode (90% or more completed)
- **Also triggers when user manually marks an episode as watched**
- Sends episode information to your Orbitarr API endpoint
- Configurable user ID and API URL through the Jellyfin dashboard
- Uses TMDB or TVDB IDs for TV show identification
- Enable/disable tracking without uninstalling the plugin

## Installation

### Method 1: From Repository (Recommended - Easy Install)

1. Open Jellyfin Dashboard
2. Go to **Plugins** → **Repositories**
3. Click **"+"** to add a new repository
4. Enter:
   - **Repository Name:** `Orbitarr Plugins`
   - **Repository URL:** `https://raw.githubusercontent.com/InfyDex/orbitarr-jellyfin/main/manifest.json`
   
   *(Replace `InfyDex` with the actual GitHub username)*

5. Click **Save**
6. Go to **Catalog** tab
7. Find **"Orbitarr Episode Tracker"** in the list
8. Click **Install**
9. Restart Jellyfin server

### Method 2: Manual Installation

1. Download the latest `Orbitarr.Jellyfin.Plugin.zip` from [GitHub Releases](https://github.com/InfyDex/orbitarr-jellyfin/releases)
2. Go to Jellyfin Dashboard → Plugins
3. Click "Upload Plugin" and select the ZIP file
4. Restart Jellyfin server

### Method 3: Build from Source

```powershell
cd Orbitarr.Jellyfin.Plugin
.\build.ps1
```

The plugin will be packaged as `bin/Orbitarr.Jellyfin.Plugin.zip`

## Configuration

1. Go to Jellyfin Dashboard → Plugins
2. Click on "Orbitarr Episode Tracker"
3. Configure the following settings:
   - **Enable Episode Tracking**: Toggle to enable/disable the plugin
   - **API Base URL**: Your Orbitarr API base URL (e.g., `http://192.168.1.12:8080`)
   - **User ID**: The UUID to use for tracking episodes (e.g., `123e4567-e89b-12d3-a456-426614174000`)
4. Click "Save"

## How It Works

The plugin listens to two types of Jellyfin events:

1. **Playback Events**: When a user watches an episode and completes it (or watches at least 90%)
2. **User Data Events**: When a user manually marks an episode as watched

When either event occurs, the plugin:

1. Extracts the TV show ID (TMDB or TVDB), season number, and episode number
2. Sends a POST request to your Orbitarr API at `/v1/episodes` with the following payload:

```json
{
  "user_id": "123e4567-e89b-12d3-a456-426614174000",
  "tv_show_id": 1,
  "season_number": 1,
  "episode_number": 1
}
```

## Requirements

- Jellyfin 10.9.0 or higher
- .NET 8.0 runtime
- An Orbitarr API endpoint accepting episode tracking requests

## Logging

The plugin logs its activity to the Jellyfin server logs. Check the logs for:
- Plugin initialization
- Episode tracking events
- API communication errors
- Configuration issues

## Troubleshooting

**Episodes not being tracked:**
- Ensure the plugin is enabled in the configuration
- Verify the API Base URL and User ID are correctly set
- Check that your TV shows have TMDB or TVDB IDs
- Review Jellyfin logs for error messages

**API connection errors:**
- Verify the API endpoint is accessible from the Jellyfin server
- Check network/firewall settings
- Ensure the API is running and accepting requests

## Development

### Project Structure

```
Orbitarr.Jellyfin.Plugin/
├── Api/
│   └── OrbitarrApiClient.cs       # API client for Orbitarr
├── Configuration/
│   ├── PluginConfiguration.cs     # Plugin configuration model
│   └── configPage.html            # Web UI configuration page
├── Plugin.cs                      # Main plugin class
├── ServerEntryPoint.cs            # Playback event handler
├── build.ps1                      # Build script
└── Orbitarr.Jellyfin.Plugin.csproj
```

### Building

Requires:
- .NET 8.0 SDK
- PowerShell (for build script)

```powershell
dotnet build -c Release
```

## License

This plugin is provided as-is for use with Jellyfin and Orbitarr.

