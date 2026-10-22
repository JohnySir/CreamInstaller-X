using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using CreamInstaller.Forms;
using CreamInstaller.Resources;
using static CreamInstaller.Resources.Resources;

namespace CreamInstaller.Utility;

internal static class BypassArchive
{
    // created inside each game folder to keep things organized and out of the game's way
    internal const string ArchiveFolderName = "_DLC Unlocker";

    internal static async Task CreateFor(Selection selection, InstallForm installForm = null)
        => await Task.Run(() =>
        {
            string root = Path.GetFullPath(selection.RootDirectory);
            HashSet<string> directories = [root];
            foreach (string directory in selection.DllDirectories
                         .Union(selection.ExecutableDirectories.Select(e => e.directory)))
                _ = directories.Add(Path.GetFullPath(directory));

            ProgramData.Log.Info(
                $"[BypassArchive] Collecting unlocker files for {selection.Name} ({selection.Id}) across {directories.Count} directories",
                LogDestination.Unlocker);
            Dictionary<string, string> entries = [];
            foreach (string directory in directories)
            {
                if (Program.Canceled)
                    return;
                CollectComponents(selection, directory, root, entries);
            }

            if (entries.Count < 1)
            {
                ProgramData.Log.Info(
                    $"[BypassArchive] No unlocker files found, archive not created | Game: {selection.Name} ({selection.Id})",
                    LogDestination.Unlocker);
                return;
            }

            try
            {
                char[] invalidCharacters = Path.GetInvalidFileNameChars();
                string fileName = string.Join("_",
                    selection.Name.Split(invalidCharacters, StringSplitOptions.RemoveEmptyEntries)).Trim();
                if (fileName.Length < 1)
                    fileName = selection.Id;
                string archiveDirectory = Path.Combine(root, ArchiveFolderName);
                string archivePath = Path.Combine(archiveDirectory, fileName + ".zip");
                archiveDirectory.CreateDirectory();
                using FileStream stream = new(archivePath, FileMode.Create, FileAccess.Write);
                using ZipArchive archive = new(stream, ZipArchiveMode.Create);
                int count = 0;
                foreach ((string entryName, string filePath) in entries)
                    try
                    {
                        _ = archive.CreateEntryFromFile(filePath, entryName, CompressionLevel.Optimal);
                        ++count;
                    }
                    catch (Exception exception)
                    {
                        ProgramData.Log.Error($"[BypassArchive] Could not archive file: {filePath}", exception);
                        installForm?.UpdateUser(
                            $"Could not archive file \"{Path.GetFileName(filePath)}\": {exception.Message}",
                            LogTextBox.Error, false);
                    }

                installForm?.UpdateUser($"Created bypass archive: {archivePath} ({count} file(s))", LogTextBox.Success);
                ProgramData.Log.Info(
                    $"[BypassArchive] Created {archivePath} with {count} of {entries.Count} files | Game: {selection.Name} ({selection.Id})",
                    LogDestination.Unlocker);
            }
            catch (Exception exception)
            {
                ProgramData.Log.Error(
                    $"[BypassArchive] Archive creation failed | Game: {selection.Name} ({selection.Id})", exception);
                installForm?.UpdateUser("Bypass archive creation failed: " + exception, LogTextBox.Error, false);
            }
        });

    private static void CollectComponents(Selection selection, string directory, string root,
        Dictionary<string, string> entries)
    {
        // Unlocker DLLs are archived only when present and matching our resource hashes; original *_o backups
        // only alongside the replacement DLL they belong to; generated configs whenever present.
        if (selection.Platform is Platform.Steam or Platform.Paradox)
        {
            if (Program.UseSmokeAPI)
            {
                directory.GetSmokeApiComponents(out string api32, out string api32_o, out string api64,
                    out string api64_o, out _, out string config, out _, out _, out _);
                AddComponent(entries, root, api32, api32_o);
                AddComponent(entries, root, api64, api64_o);
                AddConfig(entries, root, config);
            }
            else
            {
                directory.GetCreamApiComponents(out string api32, out string api32_o, out string api64,
                    out string api64_o, out string config);
                AddComponent(entries, root, api32, api32_o);
                AddComponent(entries, root, api64, api64_o);
                AddConfig(entries, root, config);
            }
        }

        if (selection.Platform is Platform.Epic or Platform.Paradox)
        {
            directory.GetScreamApiComponents(out string api32, out string api32_o, out string api64,
                out string api64_o, out _, out string config, out _, out _);
            AddComponent(entries, root, api32, api32_o);
            AddComponent(entries, root, api64, api64_o);
            AddConfig(entries, root, config);
        }

        if (selection.Platform is Platform.Ubisoft)
        {
            directory.GetUplayR1Components(out string r1_api32, out string r1_api32_o, out string r1_api64,
                out string r1_api64_o, out string r1_config, out _);
            AddComponent(entries, root, r1_api32, r1_api32_o);
            AddComponent(entries, root, r1_api64, r1_api64_o);
            AddConfig(entries, root, r1_config);
            directory.GetUplayR2Components(out string old_api32, out string old_api64, out string api32,
                out string api32_o, out string api64, out string api64_o, out string config, out _);
            AddComponent(entries, root, old_api32, null);
            AddComponent(entries, root, old_api64, null);
            AddComponent(entries, root, api32, api32_o);
            AddComponent(entries, root, api64, api64_o);
            AddConfig(entries, root, config);
        }

        foreach (string proxy in directory.GetKoaloaderProxies())
            AddResource(entries, root, proxy);
        foreach ((string _, string dll) in Koaloader.AutoLoadDLLs)
            AddResource(entries, root, directory + @"\" + dll);
        directory.GetKoaloaderComponents(out _, out string koaloaderConfig, out _);
        AddConfig(entries, root, koaloaderConfig);
    }

    private static void AddComponent(Dictionary<string, string> entries, string root, string replacement,
        string backup)
    {
        if (!replacement.FileExists() || !replacement.IsResourceFile())
            return;
        Add(entries, root, replacement);
        Add(entries, root, backup); // original file backed up by the app, kept next to its replacement
    }

    private static void AddResource(Dictionary<string, string> entries, string root, string filePath)
    {
        if (!filePath.FileExists() || !filePath.IsResourceFile())
            return;
        Add(entries, root, filePath);
    }

    private static void AddConfig(Dictionary<string, string> entries, string root, string configPath)
    {
        if (!configPath.FileExists())
            return;
        Add(entries, root, configPath);
    }

    private static void Add(Dictionary<string, string> entries, string root, string filePath)
    {
        if (filePath is null || !filePath.FileExists())
            return;
        string relativePath;
        try
        {
            relativePath = Path.GetRelativePath(root, Path.GetFullPath(filePath));
        }
        catch
        {
            return;
        }

        if (relativePath is "." or ".." || relativePath.StartsWith(@"..\", StringComparison.OrdinalIgnoreCase)
            || relativePath.StartsWith("../", StringComparison.OrdinalIgnoreCase))
        {
            ProgramData.Log.Info($"[BypassArchive] Skipping file outside game directory: {filePath}",
                LogDestination.Unlocker);
            return;
        }

        entries[relativePath.Replace('\\', '/')] = filePath;
    }
}
