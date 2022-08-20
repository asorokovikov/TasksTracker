namespace TasksTracker.Common;

public static class Directories {

    public static bool
    DirectoryExists(this string directory) => Directory.Exists(directory);

    public static string
    CreateDirectoryIfNotExists(this string directory) {
        Directory.CreateDirectory(directory);
        return directory;
    }

    public static string
    DeleteDirectoryIfExists(this string directory) {
        if (Directory.Exists(directory))
            Directory.Delete(directory, true);
        return directory;
    }

    public static string
    CreateParentDirectory(this string file) {
        Directory.GetParent(file)?.Create();
        return file;
    }

    public static string
    GetDirectoryName(this string directoryPath) => new DirectoryInfo(directoryPath).Name;

    public static IEnumerable<string>
    GetAllDirectories(this string directory) {
        if (!directory.DirectoryExists())
            yield break;
        foreach (var folder in Directory.GetDirectories(directory, searchPattern: "*", SearchOption.TopDirectoryOnly))
            yield return folder;
    }

    public static IEnumerable<string>
    GetAllDirectoryFiles(this string directory, string searchPattern = "*", SearchOption option = SearchOption.AllDirectories) =>
        Directory.EnumerateFiles(directory, searchPattern, option);
}