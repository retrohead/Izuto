using Ekona;
using plugin_level5.N3DS.Archive;
using Konnect.Extensions;

namespace Izuto.Extensions
{
    internal class ArchiveFA
    {
        public static async Task<List<B123ArchiveFile>> ListFiles(string archiveFileName)
        {
            List<B123ArchiveFile> archiveFiles = new List<B123ArchiveFile>();
            if (archiveFileName == string.Empty)
                return archiveFiles;

            await Task.Run(() =>
            {
                B123 archive_fa = new B123();
                using Stream s = File.OpenRead(archiveFileName);
                archiveFiles = archive_fa.Load(s);
                s.Close();
            });
            return archiveFiles;
        }

        public static async Task<sFile> ExtractFile(string archiveFileName, string filePath, string outputDirectory)
        {
            B123 archive_fa = new B123();
            using Stream s = File.OpenRead(archiveFileName);
            List<B123ArchiveFile> files = archive_fa.Load(s);
            B123ArchiveFile? file = files.FirstOrDefault(f => f.FilePath.FullName.Equals(filePath));
            string selectedDir = outputDirectory;
            string subDir = file.FilePath.GetDirectory().FullName.Replace("/", "\\");
            string outputDir = Path.Combine(selectedDir, subDir.TrimStart('\\', '/'));
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);
            string outputFileName = Path.Combine(outputDir, file.FilePath.GetName());
            using Stream newFileStream = File.OpenWrite(outputFileName);
            var currentFileStream = file.GetFileData().Result;
            await currentFileStream.CopyToAsync(newFileStream);
            s.Close();
            sFile outputFile = new sFile() { name = file.FilePath.GetName(), path = outputFileName };
            return outputFile;
        }

        public static async Task<List<B123ArchiveFile>> UnpackArchive(string archiveFileName, string outputDirectory)
        {
            List<B123ArchiveFile> archiveFiles = await ListFiles(archiveFileName);
            foreach(var file in archiveFiles)
            {
                await ExtractFile(archiveFileName, file.FilePath.FullName, outputDirectory);
            }
            return archiveFiles;
        }

        public static async Task ReplaceFile(string archiveFileName, B123ArchiveFile archiveFileToReplace, string newFilePath)
        {
            // load original archive for reading
            B123 archive_fa = new B123();
            using Stream s = File.OpenRead(archiveFileName);
            List<B123ArchiveFile> files = archive_fa.Load(s);
            // find original archive
            B123ArchiveFile? file = files.FirstOrDefault(f => f.FilePath.FullName == archiveFileToReplace.FilePath.FullName);
            int fileIndex = files.IndexOf(file);
            if (file == null) return;
            string tempFilePath = archiveFileName + "_tmp";
            using (Stream newFileDataStream = File.OpenRead(newFilePath))
            {
                files[fileIndex].SetFileData(newFileDataStream);
                B123 arch = new B123();
                Stream sourceStream = File.OpenRead(archiveFileName);
                arch.Load(sourceStream);
                Stream destStream = File.OpenWrite(tempFilePath);
                arch.Save(destStream, files);
                sourceStream.Close();
                destStream.Close();
            }
            s.Close();
            File.Delete(archiveFileName);
            File.Move(tempFilePath, archiveFileName);
        }
    }
}
