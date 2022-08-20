namespace TasksTracker.Common;

public static class Files {
    public static string
    AppendPath(this string path1, string path2) => Path.Combine(path1, path2);

    public static string
    ToAbsolutePath(this string subPath, string fullPath) => Path.Combine(fullPath, subPath);

    public static string
    AppendFileExtension(this string path, string extension) {
        if (extension.IsEmpty())
            return path;
        if (extension[0] == '.')
            return path + extension;
        return $"{path}.{extension}";
    }

    public static bool
    FileExists(this string path) => File.Exists(path);

    public static void
    DeleteFile(this string file) => File.Delete(file);

    public static string
    DeleteFileIfExists(this string file) {
        if (file.FileExists())
            file.DeleteFile();
        return file;
    }

    public static string
    FileWriteAllText(this string file, string text) {
        File.WriteAllText(file, text);
        return file;
    }

    public static FileStream
    OpenFileForRead(this string file) =>
        File.OpenRead(file);

    public static FileStream
    OpenFileForWrite(this string file) =>
        File.OpenWrite(file);
}