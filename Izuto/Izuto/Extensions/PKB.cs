using Ekona;
using INAZUMA11;
using Izuto.Extensions;
using Konnect.Extensions;
using plugin_level5.N3DS.Archive;
using System.Text;
using System.Threading;

public class PKB
{
    public class FileEntry
    {
        public sFile FileData = new sFile();
        public sFolder FileContents = new sFolder();
    }

    public static async Task<PKB.FileEntry> UnpackPKBFromArchiveFA_Async(string inputArchiveFAFilePath, B123ArchiveFile ArchiveFAFileToExtract, string workingDirectory)
    {
        sFile pkbFile = await ArchiveFA.ExtractFile(inputArchiveFAFilePath, ArchiveFAFileToExtract.FilePath.FullName, workingDirectory);
        sFile pkhFile = await ArchiveFA.ExtractFile(inputArchiveFAFilePath, ArchiveFAFileToExtract.FilePath.FullName.Replace(".pkb", ".pkh"), workingDirectory);
        sFolder extractedPKBItems = INAZUMA11.PKB.Unpack(pkbFile, pkhFile);
        return new PKB.FileEntry() { FileData = pkbFile, FileContents = extractedPKBItems };
    }


    public static async Task<FileEntry> ExtractPACFileFromPKB_Async(FileEntry PKBFileInfo, sFile fileToExtract, string outputDirectory)
    {
        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);
        var pkbitem = PKBFileInfo.FileContents.files.FirstOrDefault(f => f.name.Equals(fileToExtract.name));

        string compressedFileName = Path.Combine(outputDirectory, pkbitem.name);

        // open the pkb file and extract the selected file
        using (var br = new BinaryReader(File.OpenRead(PKBFileInfo.FileData.path)))
        {
            br.BaseStream.Position = pkbitem.offset;
            File.WriteAllBytes(compressedFileName, br.ReadBytes((int)pkbitem.size));
        }
        FileEntry PACFileInfo = new FileEntry();
        PACFileInfo.FileData.path = compressedFileName + "_decompressed";
        PACFileInfo.FileData.name = pkbitem.name + "_decompressed";

        await Task.Run(() =>
        {


            // decompress the file
            FormatCompress compressFormat = DSDecmp.Main.Get_Format(compressedFileName);

            if (compressFormat != FormatCompress.Invalid)
            {
                DSDecmp.Main.Decompress(compressedFileName, PACFileInfo.FileData.path, compressFormat);
                if (!File.Exists(PACFileInfo.FileData.path))
                    throw new Exception($"Failed to decompress file {compressedFileName}");
            } else
            {
                throw new Exception($"Failed to decompress file {compressedFileName}");
            }
        }
        );
        return PACFileInfo;
    }

    private static long AlignFileTo4Bytes(string fn, byte byteToPad)
    {
        FileInfo info = new FileInfo(fn);
        if (info.Length % 0x04 > 0)
        {
            long remain = 0x04 - (info.Length % 0x04);
            if (remain != 0)
            {
                long remained = remain;
                using (var fs = new FileStream(fn, FileMode.Append, FileAccess.Write))
                {
                    while (remain > 0)
                    {
                        fs.Write(new byte[] { byteToPad }, 0, 1);
                        remain--;
                    }
                }
                return remained;
            }
        }
        return 0;
    }
    private static long AlignFileTo16Bytes(string fn, byte byteToPad)
    {
        FileInfo info = new FileInfo(fn);
        if (info.Length % 0x10 > 0)
        {
            long remain = 0x10 - (info.Length % 0x10);
            if (remain != 0)
            {
                long remained = remain;
                using (var fs = new FileStream(fn, FileMode.Append, FileAccess.Write))
                {
                    while (remain > 0)
                    {
                        fs.Write(new byte[] { byteToPad }, 0, 1);
                        remain--;
                    }

                }
                return remained;
            }
        }
        return 0;
    }

    private static void addLZ77Header(string fn)
    {
        // Write the header LZ77
        BinaryReader br = new BinaryReader(File.OpenRead(fn));
        BinaryWriter bw = new BinaryWriter(File.OpenWrite(fn + ".lz"));
        bw.Write(new char[] { 'L', 'Z', '7', '7' });
        bw.Write(br.ReadBytes((int)br.BaseStream.Length));
        bw.Flush();
        bw.Close();
        br.Close();
        File.Delete(fn);
        File.Move(fn + ".lz", fn);
    }

    public static async Task<bool> ImportDecompressedPACFile_Async(FileEntry PKBFileInfo, PKB.FileEntry fileToImport)
    {
        string tempDir = Path.Combine(Path.GetDirectoryName(fileToImport.FileData.path), "import");
        if (Directory.Exists(tempDir))
            Directory.Delete(tempDir, true);
        Directory.CreateDirectory(tempDir);
        string path = fileToImport.FileData.path.ToString();
        path = path.Replace("_decompressed", "");
        string compressedFileName = Path.Combine(Path.GetDirectoryName(path)?.ToString() ?? "", "import", fileToImport.FileData.name.Replace("_decompressed", ""));
        string orignalCompressedFileName = path;
        // compress
        long adjustedSize = 0;
        long padding = 0;
        long newSize = 0;
        await Task.Run(() =>
        {
            // compress the file
            FormatCompress compressFormat = DSDecmp.Main.Get_Format(orignalCompressedFileName); // get the format from the orignal compressed file

            if (compressFormat != FormatCompress.Invalid)
            {
                DSDecmp.Main.Compress(fileToImport.FileData.path + "_modified", compressedFileName, compressFormat);
                // align compressed file to 16 bytes
                adjustedSize = AlignFileTo4Bytes(compressedFileName, 0x00);
                FileInfo newFileInfo = new FileInfo(compressedFileName);
                newSize = newFileInfo.Length;
                padding = AlignFileTo16Bytes(compressedFileName, 0xFF);
                if (!File.Exists(fileToImport.FileData.path + "_modified"))
                    throw new Exception($"Failed to compress file {Path.GetFileName(compressedFileName)}");
            } else
            {
                throw new Exception($"Unexpected compression format");
            }
        }
        );

        // update the file size
        var pkbEntry = PKBFileInfo.FileContents.files.FirstOrDefault(f => f.name.Equals(fileToImport.FileData.name.Replace("_decompressed", "")));

        // get new next offset, must be aligned to 4 bytes
        long sizediff = 0;
        long oldEnd = pkbEntry.offset + pkbEntry.size;
        long oldPadding = oldEnd % 0x10;
        long newEnd = pkbEntry.offset + (uint)newSize + padding;
        sizediff = 0;
        if (oldEnd + oldPadding != newEnd)
            sizediff += (newEnd) - (oldEnd + oldPadding);
        var pkbEntryIndex = PKBFileInfo.FileContents.files.IndexOf(pkbEntry);

        sFile newFile = new sFile();
        newFile.name = pkbEntry.name;
        newFile.offset = pkbEntry.offset;
        newFile.path = pkbEntry.path;
        newFile.size = (uint)newSize;

        PKBFileInfo.FileContents.files.RemoveAt(pkbEntryIndex);
        PKBFileInfo.FileContents.files.Insert(pkbEntryIndex, newFile);


        // open the pkb file so we can read from it writing to a new file
        string modifiedPKBPath = PKBFileInfo.FileData.path + "_modified";
        if (File.Exists(modifiedPKBPath))
            File.Delete(modifiedPKBPath);

        using (var br = new BinaryReader(File.OpenRead(PKBFileInfo.FileData.path)))
        {
            using (var bw = new BinaryWriter(File.OpenWrite(modifiedPKBPath)))
            {
                // write everything before the file entry
                if(pkbEntry.offset > 0)
                    bw.Write(br.ReadBytes((int)pkbEntry.offset - 1));
                bw.BaseStream.Position = pkbEntry.offset;
                // write the new file entry
                using (var br2 = new BinaryReader(File.OpenRead(compressedFileName)))
                {
                    byte[] buffer = new byte[81920]; // 80 KB buffer (common default)
                    int bytesRead;

                    while ((bytesRead = br2.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        bw.Write(buffer, 0, bytesRead);
                    }
                }
                // write everything after the file entry
                if (pkbEntryIndex + 1 < PKBFileInfo.FileContents.files.Count())
                {
                    br.BaseStream.Position = PKBFileInfo.FileContents.files[pkbEntryIndex + 1].offset;

                    const int bufferSize = 81920; // 80 KB buffer
                    byte[] buffer2 = new byte[bufferSize];
                    int bytesRead2;

                    while ((bytesRead2 = br.BaseStream.Read(buffer2, 0, buffer2.Length)) > 0)
                    {
                        bw.Write(buffer2, 0, bytesRead2);
                    }
                }
            }
        }
        // update the offsets for all files past this one
        if (sizediff != 0)
        {
            var files = PKBFileInfo.FileContents.files.FindAll(f => f.offset > pkbEntry.offset);
            for (int i = 0; i < files.Count; i++)
            {
                var file = PKBFileInfo.FileContents.files.FirstOrDefault(f => f.path.Equals(files[i].path));
                var index = PKBFileInfo.FileContents.files.IndexOf(file);

                sFile newFile2 = new sFile();
                newFile2.name = file.name;
                newFile2.offset = file.offset + (uint)sizediff;
                newFile2.path = pkbEntry.path;
                newFile2.size = file.size;

                PKBFileInfo.FileContents.files.RemoveAt(index);
                PKBFileInfo.FileContents.files.Insert(index, newFile2);
            }
        }

        // create new pkh
        PluginHost host = new PluginHost();
        sFile pkh = new sFile();
        pkh.path = PKBFileInfo.FileData.path.Replace(".pkb", ".pkh");
        createPKH(
            pkh.path,
            pkh.path + "_modified",
            PKBFileInfo.FileData.path + "_modified",
            PKBFileInfo.FileContents
        );

        Directory.Delete(tempDir, true);
        return true;
    }

    public static void createPKH(string oldPKH, string newPKH, string newPKB, sFolder unpacked)
    {
        BinaryReader br = new BinaryReader(File.OpenRead(oldPKH));
        BinaryReader bwPkb = new BinaryReader(File.OpenRead(newPKB));
        BinaryWriter bwPkh = new BinaryWriter(File.OpenWrite(newPKH));

        // Write header
        bwPkh.Write(Encoding.ASCII.GetChars(br.ReadBytes(16))); // Header
        bwPkh.Write(br.ReadUInt16());                           //File_Size
        ushort type = br.ReadUInt16();
        bwPkh.Write(type);                                      //Type
        bwPkh.Write(br.ReadUInt16());                           //Unknown
        ushort num_files = br.ReadUInt16();
        bwPkh.Write(num_files);                                 //Num_files
        bwPkh.Write(br.ReadUInt32());                           //Unknown

        if (type == 0)
            bwPkh.Write(br.ReadUInt32());                       //Block_Length
        else if (type == 2 || type == 3)
        {
            bwPkh.Write(unpacked.files[0].size);
            br.BaseStream.Position += 4;
        }

        bwPkh.Write(br.ReadBytes(0x10));                        // Unknown, usually 0x00 but not always

        uint offset = 0;
        for (int i = 0; i < num_files; i++)
        {
            if (type == 0)
            {
                bwPkh.Write(br.ReadUInt32());           // Unknown, ID¿?
                bwPkh.Write(offset);                    // Offset
                bwPkh.Write(unpacked.files[i].size);    //Size File
                br.BaseStream.Position += 8;
                offset += unpacked.files[i].size;
                if (offset % 0x10 != 0)
                    offset += 0x10 - (offset % 0x10);
            }
            else if (type == 2)
                bwPkh.Write(br.ReadUInt64());
            else if (type == 3)
                bwPkh.Write(br.ReadUInt32());
        }

        while (bwPkh.BaseStream.Position % 0x10 != 0)
            bwPkh.Write((byte)0xFF);

        bwPkh.Flush();

        bwPkb.Close();
        bwPkh.Close();

        br.Close();
    }
}
