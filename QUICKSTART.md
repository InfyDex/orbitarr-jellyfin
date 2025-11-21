# Quick Start - Publish to GitHub

## 🚀 Super Fast Setup (5 minutes)

### Step 1: Run Setup Script

```powershell
.\setup-repo.ps1 -GitHubUsername "YOUR_GITHUB_USERNAME"
```

This will:
- ✅ Update all files with your GitHub username
- ✅ Build the plugin
- ✅ Calculate checksum
- ✅ Prepare everything for GitHub

### Step 2: Create GitHub Repository

1. Go to https://github.com/new
2. Repository name: `orbitarr-jellyfin`
3. Make it **Public**
4. Click **Create repository**

### Step 3: Push to GitHub

```powershell
git init
git add .
git commit -m "Initial commit - Orbitarr Jellyfin Plugin"
git remote add origin https://github.com/YOUR_USERNAME/orbitarr-jellyfin.git
git branch -M main
git push -u origin main
```

### Step 4: Create First Release

```powershell
git tag v1.0.0
git push origin v1.0.0
```

**OR** manually:
1. Go to your repo → **Releases** → **Create a new release**
2. Tag: `v1.0.0`
3. Upload `Orbitarr.Jellyfin.Plugin\bin\Orbitarr.Jellyfin.Plugin.zip`
4. Click **Publish**

### Step 5: Install in Jellyfin

1. Open Jellyfin → **Dashboard** → **Plugins** → **Repositories**
2. Click **"+"**
3. Add repository:
   ```
   https://raw.githubusercontent.com/YOUR_USERNAME/orbitarr-jellyfin/main/manifest.json
   ```
4. Go to **Catalog**
5. Install **"Orbitarr Episode Tracker"**
6. Restart Jellyfin
7. Configure with your API URL and User ID

## ✅ Done!

Your plugin is now installable just like Skin Manager!

Users can add your repository URL and install with one click.

---

### 📦 Share With Others

Just give them this URL to add to Jellyfin:
```
https://raw.githubusercontent.com/YOUR_USERNAME/orbitarr-jellyfin/main/manifest.json
```

### 🔄 Release Updates

To release v1.1.0:
```powershell
# Make your changes, then:
git add .
git commit -m "Version 1.1.0 - Your changes"
git tag v1.1.0
git push origin main
git push origin v1.1.0
```

Update `manifest.json` with the new version and checksum. Users will see the update automatically in Jellyfin!
