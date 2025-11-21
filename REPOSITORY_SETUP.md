# Setting Up GitHub Repository for Easy Installation

Follow these steps to enable easy plugin installation directly from GitHub.

## 1. Create GitHub Repository

1. Go to https://github.com/new
2. Create a repository named `orbitarr-jellyfin` (or your preferred name)
3. Set it to **Public** (required for Jellyfin plugin repositories)
4. Don't initialize with README (we already have files)

## 2. Push Your Code to GitHub

```powershell
cd C:\Users\anwar\IdeaProjects\orbitarr-jellyfin

# Initialize git if not already done
git init

# Add all files
git add .

# Commit
git commit -m "Initial commit - Orbitarr Jellyfin Plugin"

# Add remote (replace YOUR_USERNAME with your GitHub username)
git remote add origin https://github.com/YOUR_USERNAME/orbitarr-jellyfin.git

# Push to GitHub
git branch -M main
git push -u origin main
```

## 3. Create First Release

### Option A: Automatic (Recommended)

1. Create and push a tag:
```powershell
git tag v1.0.0
git push origin v1.0.0
```

2. GitHub Actions will automatically:
   - Build the plugin
   - Create a release
   - Upload the ZIP file
   - Calculate checksum

### Option B: Manual

1. Run the build script:
```powershell
cd Orbitarr.Jellyfin.Plugin
.\build.ps1
```

2. Go to your GitHub repository
3. Click **Releases** → **Create a new release**
4. Tag: `v1.0.0`
5. Title: `Orbitarr Episode Tracker v1.0.0`
6. Upload `bin/Orbitarr.Jellyfin.Plugin.zip`
7. Publish release

## 4. Update manifest.json

1. After creating the release, get the download URL
2. Calculate MD5 checksum:
```powershell
Get-FileHash -Path "Orbitarr.Jellyfin.Plugin\bin\Orbitarr.Jellyfin.Plugin.zip" -Algorithm MD5
```

3. Edit `manifest.json` and update:
   - Replace `YOUR_USERNAME` with your GitHub username
   - Replace `CHECKSUM_WILL_BE_GENERATED` with the MD5 hash

Example:
```json
"sourceUrl": "https://github.com/anwar/orbitarr-jellyfin/releases/download/v1.0.0/Orbitarr.Jellyfin.Plugin.zip",
"checksum": "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6"
```

4. Commit and push:
```powershell
git add manifest.json
git commit -m "Update manifest with release URL and checksum"
git push
```

## 5. Add Repository to Jellyfin

Now users (including you) can install the plugin easily:

### For Users:

1. Open Jellyfin Dashboard
2. Go to **Plugins** → **Repositories**
3. Click **"+"** to add a repository
4. Enter:
   - **Repository Name:** `Orbitarr Plugins`
   - **Repository URL:** `https://raw.githubusercontent.com/YOUR_USERNAME/orbitarr-jellyfin/main/manifest.json`
5. Click **Save**
6. Go to **Catalog** tab
7. Find **"Orbitarr Episode Tracker"**
8. Click **Install**
9. Restart Jellyfin
10. Configure the plugin with your API settings

## 6. Update README.md

Add installation instructions to your README:

```markdown
## Installation

### From Repository (Recommended)

1. In Jellyfin, go to **Dashboard** → **Plugins** → **Repositories**
2. Click **"+"** and add:
   - Repository URL: `https://raw.githubusercontent.com/YOUR_USERNAME/orbitarr-jellyfin/main/manifest.json`
3. Go to **Catalog**, find **Orbitarr Episode Tracker**, and click **Install**
4. Restart Jellyfin
5. Configure with your API URL and User ID

### Manual Installation

Download the latest `Orbitarr.Jellyfin.Plugin.zip` from [Releases](https://github.com/YOUR_USERNAME/orbitarr-jellyfin/releases) and upload it manually.
```

## 7. Future Updates

To release a new version:

1. Make your changes
2. Update version in `Orbitarr.Jellyfin.Plugin.csproj` if needed
3. Create a new tag:
```powershell
git tag v1.1.0
git push origin v1.1.0
```

4. GitHub Actions builds automatically
5. Add new version entry to `manifest.json`:
```json
{
    "version": "1.1.0.0",
    "changelog": "- Bug fixes\n- New features",
    "targetAbi": "10.9.0.0",
    "sourceUrl": "https://github.com/YOUR_USERNAME/orbitarr-jellyfin/releases/download/v1.1.0/Orbitarr.Jellyfin.Plugin.zip",
    "checksum": "NEW_CHECKSUM",
    "timestamp": "2025-11-22T00:00:00Z"
}
```

6. Users will see the update in Jellyfin automatically!

## Quick Reference

**Your Repository URL:**
```
https://raw.githubusercontent.com/YOUR_USERNAME/orbitarr-jellyfin/main/manifest.json
```

**Manual Download:**
```
https://github.com/YOUR_USERNAME/orbitarr-jellyfin/releases/latest
```

---

**Note:** Remember to replace `YOUR_USERNAME` with your actual GitHub username throughout all files!
