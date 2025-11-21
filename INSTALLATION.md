# Orbitarr Jellyfin Plugin - Installation Guide

## ✅ Build Complete!

Your plugin has been successfully built and packaged at:
```
C:\Users\anwar\IdeaProjects\orbitarr-jellyfin\Orbitarr.Jellyfin.Plugin\bin\Orbitarr.Jellyfin.Plugin.zip
```

## 📦 Installation Steps

### 1. Upload to Jellyfin

1. Open your Jellyfin web interface
2. Go to **Dashboard** → **Plugins** → **Catalog**
3. Click the **"..."** menu in the top right
4. Select **"Upload Plugin"** or **"Manual Install"**
5. Browse and select `Orbitarr.Jellyfin.Plugin.zip`
6. Click **Upload**

### 2. Restart Jellyfin

After uploading, **restart your Jellyfin server** for the plugin to load.

### 3. Configure the Plugin

1. Go to **Dashboard** → **Plugins** → **My Plugins**
2. Find **"Orbitarr Episode Tracker"** in the list
3. Click on it to open settings
4. Configure:
   - ✅ **Enable Episode Tracking**: Check this box
   - 🌐 **API Base URL**: `http://192.168.1.12:8080`
   - 👤 **User ID**: Your UUID (e.g., `123e4567-e89b-12d3-a456-426614174000`)
5. Click **Save**

## 🎬 How It Works

The plugin will now automatically track episodes when:

1. ✅ **User watches an episode** (90%+ completion)
2. ✅ **User manually marks episode as watched**

### API Call Example

When triggered, the plugin sends:
```bash
POST http://192.168.1.12:8080/v1/episodes
Content-Type: application/json

{
  "user_id": "123e4567-e89b-12d3-a456-426614174000",
  "tv_show_id": 1234,
  "season_number": 1,
  "episode_number": 5
}
```

## 🔍 Verifying It Works

Check Jellyfin logs for messages like:
```
[INF] Orbitarr Episode Tracker plugin started
[INF] Episode watched: Breaking Bad S01E01
[INF] Tracking episode: Show=1396, Season=1, Episode=1 to http://192.168.1.12:8080/v1/episodes
[INF] Successfully tracked episode
```

## 📝 Notes

- The plugin uses **TMDB** or **TVDB** IDs for TV shows
- Make sure your shows have proper metadata
- The User ID in the config is what gets sent to your API (not Jellyfin's user ID)
- Check that your Orbitarr API is accessible from the Jellyfin server

## 🐛 Troubleshooting

**Episodes not being tracked?**
- Verify plugin is enabled in settings
- Check API URL is correct and accessible
- Ensure User ID is configured
- Review Jellyfin logs for errors

**Need to rebuild?**
```powershell
cd C:\Users\anwar\IdeaProjects\orbitarr-jellyfin\Orbitarr.Jellyfin.Plugin
.\build.ps1
```

---

**Plugin Version**: 1.0.0  
**Jellyfin Compatibility**: 10.9.0+  
**Framework**: .NET 8.0
