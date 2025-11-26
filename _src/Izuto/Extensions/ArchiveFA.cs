using Ekona;
using plugin_level5.N3DS.Archive;
using Konnect.Extensions;
using Pack;

namespace Izuto.Extensions
{

    internal class ArchiveFA
    {
        private class FileReplacement
        {
            public B123ArchiveFile ArchiveToReplace;
            public string ReplacementFilePath;

            public FileReplacement(B123ArchiveFile ArchiveToReplace, string ReplacementFilePath)
            {
                this.ArchiveToReplace = ArchiveToReplace;
                this.ReplacementFilePath = ReplacementFilePath;
            }
        }

        private static List<FileReplacement> FileReplacements = new List<FileReplacement>();

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



        public static async Task<bool> QueueReplaceFile(string archiveFileName, B123ArchiveFile archiveFileToReplace, string newFilePath)
        {
            // load original archive for reading
            B123 archive_fa = new B123();
            using (Stream s = File.OpenRead(archiveFileName))
            {
                List<B123ArchiveFile> files = archive_fa.Load(s);
                // find original archive
                B123ArchiveFile? file = files.FirstOrDefault(f => f.FilePath.FullName == archiveFileToReplace.FilePath.FullName);

                int fileIndex = files.IndexOf(file);
                if (file == null) return false;
            }
            FileReplacements.Add(new FileReplacement(archiveFileToReplace, newFilePath));
            return true;
        }


        public static async Task ReplaceQueuedFiles(string archiveFileName)
        {
            await Task.Run(() =>
            {
                // load original archive for reading
                B123 archive_fa = new B123();
                string tempFilePath = archiveFileName + "_tmp";
                using (Stream sourceStream = File.OpenRead(archiveFileName))
                {
                    List<B123ArchiveFile> files = archive_fa.Load(sourceStream);

                    // open the destination archive for reading
                    // open a temp file for writing
                    using (Stream destStream = File.OpenWrite(tempFilePath))
                    {
                        List<Stream> openstreams = new List<Stream>();
                        int importedFileCount = 0;
                        foreach (var fileToReplace in FileReplacements)
                        {
                            MainForm.Self.UpdateProgress("Importing Files", importedFileCount, FileReplacements.Count);
                            importedFileCount++;
                            openstreams.Add(File.OpenRead(fileToReplace.ReplacementFilePath));
                            // find original archive
                            B123ArchiveFile? file = files.FirstOrDefault(f => f.FilePath.FullName == fileToReplace.ArchiveToReplace.FilePath.FullName);
                            int fileIndex = files.IndexOf(file);
                            if (file == null)
                                throw new Exception("Trying to import a file that does not exist in the original archive");

                            files[fileIndex].SetFileData(openstreams[openstreams.Count - 1]);
                        }
                        // save the new archive
                        MainForm.Self.UpdateProgress("Saving New Archive",0,1);
                        archive_fa.Save(destStream, files);
                        sourceStream.Close();
                        destStream.Close();
                        foreach (var s in openstreams)
                        {
                            s.Close();
                        }
                    }
                }
                File.Delete(archiveFileName);
                File.Move(tempFilePath, archiveFileName);
            });
        }
    }
}
