using Ekona;
using Izuto;
using Izuto.Extensions;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public class PAC
{
    public class BinaryEntry
    {
        public Int16 FileId;    // 0x0000 – entry index
        public Int16 FileSize;  // 0x0002 – entry size field
        public byte[]? Data;    // rest of the entry
    }

    public class ScriptEntry
    {
        public Int32 ID;
        public Int16 LineNumber;
        public Int16 Type; // not always used
        public int OriginalSize;
        public string Text = "";
        public byte[]? TextBytes = null;
        public bool IsLinked = false;
        public byte[]? Data = null; // rest of entry, added in later versions for linking text scripts
    }
    public class LinkedScriptEntry
    {
        public int Offset;
        public int Size;

        public LinkedScriptEntry(string LinkText)
        {
            Offset = int.Parse(LinkText.Replace("@", "").Split(',')[0]);
            Size = int.Parse(LinkText.Replace("@", "").Split(',')[1]);
        }
    }
    public class HeaderInfo
    {
        // Header is 32 bytes
        // Entries start at 0x0020
        public string Magic = "";       // 0x0000 – "SSD\0"
        public Int16 Version;           // 0x0004 – usually 1
        public Int16 Type;              // 0x0006 – usually 3
        public Int32 FileSize;          // 0x0008 – total file size
        public Int16 AssetCount;        // 0x000C – max index for entries
        public Int16 StringCount;       // 0x000E – strings are stored after binary content
        public Int32 DataSize;          // 0x0010 – size of data section
        public byte[]? Unknown = new byte[12]; // 0x0014 – unknown
    }

    public HeaderInfo Header = new HeaderInfo();
    public List<BinaryEntry> BinaryEntries = new List<BinaryEntry>(); //0x0020
    public List<ScriptEntry> StringEntries = new List<ScriptEntry>(); //0x0020 + DataSize

    public bool Load(string fn)
    {
        FileInfo fileInfo = new FileInfo(fn);
        if(fileInfo.Length == 0)
        {
            return false;
        }


        using (var fs = File.OpenRead(fn))
        using (var br = new BinaryReader(fs))
        {
            // --- Read header ---
            byte[] magicbytes = br.ReadBytes(4);
            Header.Magic = System.Text.Encoding.GetEncoding("shift_jis").GetString(magicbytes); // "SSD\0" 4 bytes
            if (Header.Magic != "SSD\0" && Header.Magic != "\0\0\0\0")
            {
                if (Header.Magic.Substring(0,2) == "\0\0")
                {
                    // uncompressed file containing strings only
                    LoadPACV2(br);
                    return true;
                }
                try
                {
                    // uncompressed file containing strings only
                    LoadPACV0(br);
                    return true;
                } catch
                {

                }
                MessageBox.Show("PAC file not currently supported", "PAC File Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            // compressed file
            LoadPACV1(br);
        }
        return true;
    }

    // string pac to go with V1 DS pacs
    private void LoadPACV0(BinaryReader br)
    {
        Header.Version = 0;
        Header.Type = -2; // using type -2 as v0 indicator            
        Header.FileSize = 0;
        Header.AssetCount = 0;
        Header.StringCount = 0;
        Header.DataSize = 0;
        Header.Unknown = null;

        // --- Read entries ---
        BinaryEntries = new List<BinaryEntry>();
        StringEntries = new List<ScriptEntry>();

        // reverse back to 4th byte of the stream
        br.BaseStream.Position = 4;

        while (true)
        {
            if (br.BaseStream.Position == br.BaseStream.Length)
                break;
            ScriptEntry sentry = new ScriptEntry();
            sentry.ID = br.ReadInt16();
            sentry.LineNumber = br.ReadInt16();
            sentry.OriginalSize = br.ReadInt32();   // 4 bytes
            int headerSize = 8;
            if (sentry.OriginalSize > 1)
            {
                sentry.TextBytes = br.ReadBytes(sentry.OriginalSize - headerSize);
                sentry.Text = System.Text.Encoding.GetEncoding("shift_jis").GetString(sentry.TextBytes);
            }
            sentry.Data = null;
            sentry.IsLinked = false;
            if (sentry.Text.StartsWith("@"))
            {
                // reverse back and scan the string again, storing the remaining part for analysis
                br.BaseStream.Position -= sentry.OriginalSize - headerSize;

                sentry.Text = sentry.Text.Split('\0')[0];
                byte[] text = Encoding.GetEncoding("shift_jis").GetBytes(sentry.Text);
                int len = text.Count();
                sentry.IsLinked = true;

                byte[] bytes = br.ReadBytes(len); // including null terminator
                sentry.Text = System.Text.Encoding.GetEncoding("shift_jis").GetString(bytes);

                // scan the remaining bytes into data
                if (sentry.OriginalSize - (len) - headerSize > 0)
                    sentry.Data = br.ReadBytes(sentry.OriginalSize - (len) - headerSize);
            }
            StringEntries.Add(sentry);
            Header.StringCount++;
            if (br.BaseStream.Position >= br.BaseStream.Length - 1)
                break; // finished reading, hopefully didn't go past the end of the stream :)
        }
    }

    // standard pac, sometimes may contain a string count but strings are in a separate pac like evet.pkb (IE1 DS)
    private void LoadPACV1(BinaryReader br)
    {
        Header.Version = br.ReadInt16();   // 2 bytes
        Header.Type = br.ReadInt16();      // 2 bytes
        Header.FileSize = br.ReadInt32();  // 4 bytes
        Header.AssetCount = br.ReadInt16();  // 2 bytes
        Header.StringCount = br.ReadInt16(); // 2 bytes
        Header.DataSize = br.ReadInt32();  // 4 bytes
        Header.Unknown = br.ReadBytes(12);  // 12 bytes unknown

        // --- Read entries ---
        BinaryEntries = new List<BinaryEntry>();
        for (int i = 0; i < Header.AssetCount; i++)
        {
            BinaryEntry bentry = new BinaryEntry();
            bentry.FileId = br.ReadInt16();   // 2 bytes
            bentry.FileSize = br.ReadInt16(); // 2 bytes

            // Each entry is larger than 4 bytes, but for now we only know FileId + FileSize.
            // Read the rest of the entry into Unknown so we preserve the data.
            bentry.Data = br.ReadBytes(bentry.FileSize - 4); // remaining bytes of the entry
            BinaryEntries.Add(bentry);
        }

        StringEntries = new List<ScriptEntry>();
        if (Header.StringCount > 0)
        {
            // reverse back to parse strings
            br.BaseStream.Position = 20 + Header.DataSize;
            br.ReadBytes(12);  // 12 bytes unknown

            // read strings
            if (br.BaseStream.Position == br.BaseStream.Length)
            {
                // assuming v0 pack with strings in a different file?
                return;
            }
            for (int i = 0; i < Header.StringCount; i++)
            {
                ScriptEntry sentry = new ScriptEntry();
                int headersize = 0;
                if (Header.Version == 2)
                {
                    sentry.ID = br.ReadInt16();   // 2 bytes
                    sentry.LineNumber = br.ReadInt16();   // 2 bytes
                    sentry.OriginalSize = br.ReadInt16();   // 2 bytes
                    sentry.Type = br.ReadInt16();  // type 2 bytes
                    headersize = 8;
                }
                else
                {
                    sentry.ID = br.ReadInt16();   // 2 bytes
                    sentry.LineNumber = br.ReadByte();   // 1 byte
                    sentry.OriginalSize = br.ReadByte();   // 1 byte
                    sentry.Type = -1;
                    headersize = 4;
                }
                sentry.TextBytes = br.ReadBytes(sentry.OriginalSize - headersize);
                sentry.Text = System.Text.Encoding.GetEncoding("shift_jis").GetString(sentry.TextBytes);
                sentry.Data = null;
                sentry.IsLinked = false;
                if (sentry.Text.StartsWith("@"))
                {
                    // reverse back and scan the string again, storing the remaining part for analysis
                    br.BaseStream.Position -= sentry.OriginalSize - headersize;

                    sentry.Text = sentry.Text.Split('\0')[0];
                    byte[] text = Encoding.GetEncoding("shift_jis").GetBytes(sentry.Text);
                    int len = text.Count();
                    sentry.IsLinked = true;

                    byte[] bytes = br.ReadBytes(len); // including null terminator
                    sentry.Text = System.Text.Encoding.GetEncoding("shift_jis").GetString(bytes);

                    // scan the remaining bytes into data
                    if (sentry.OriginalSize - (len) - headersize > 0)
                        sentry.Data = br.ReadBytes(sentry.OriginalSize - (len) - headersize);
                }
                StringEntries.Add(sentry);
            }
        }
        if (br.BaseStream.Position != br.BaseStream.Length)
        {
            MessageBox.Show("remaining bytes unread");
        }
    }

    // linked pacs like eve.pkb and evet.pkb where @ strings in eve.pkb link to the pac file (IE 123 3DS)
    private void LoadPACV2(BinaryReader br)
    {
        Header.Version = 0;      
        Header.Type = -1; // using type -1 as v2 indicator            
        Header.FileSize = 0;        
        Header.AssetCount = 0;      
        Header.StringCount = 0;     
        Header.DataSize = 0;        
        Header.Unknown = null;

        // --- Read entries ---
        BinaryEntries = new List<BinaryEntry>();

        StringEntries = new List<ScriptEntry>();

        // reverse back to start of the stream
        br.BaseStream.Position = 0;

        // keep reading until we reach the end of the file
        while (true)
        {
            br.ReadBytes(3);  // zero bytes
            if (br.BaseStream.Position == br.BaseStream.Length)
                break;
            ScriptEntry sentry = new ScriptEntry();
            sentry.ID = Header.StringCount;
            sentry.LineNumber = 0;
            sentry.OriginalSize = br.ReadByte();   // 1 byte
            if (sentry.OriginalSize > 1)
            {
                sentry.TextBytes = br.ReadBytes(sentry.OriginalSize - 4);
                sentry.Text = System.Text.Encoding.GetEncoding("shift_jis").GetString(sentry.TextBytes);
            }
            sentry.Data = null;
            sentry.IsLinked = false;
            if (sentry.Text.StartsWith("@"))
            {
                // reverse back and scan the string again, storing the remaining part for analysis
                br.BaseStream.Position -= sentry.OriginalSize - 4;

                sentry.Text = sentry.Text.Split('\0')[0];
                byte[] text = Encoding.GetEncoding("shift_jis").GetBytes(sentry.Text);
                int len = text.Count();
                sentry.IsLinked = true;

                byte[] bytes = br.ReadBytes(len); // including null terminator
                sentry.Text = System.Text.Encoding.GetEncoding("shift_jis").GetString(bytes);

                // scan the remaining bytes into data
                if (sentry.OriginalSize - (len) - 4 > 0)
                    sentry.Data = br.ReadBytes(sentry.OriginalSize - (len) - 4);
            }
            StringEntries.Add(sentry);
            Header.StringCount++;
            if (br.BaseStream.Position >= br.BaseStream.Length - 1)
                break; // finished reading, hopefully didn't go past the end of the stream :)
        }
    }

    public void SaveAs(string fn)
    {
        using (var fs = new FileStream(fn, FileMode.Create, FileAccess.Write))
        using (var bw = new BinaryWriter(fs))
        {
            if (Header.Type == -2)
            {
                SavePACV0(bw);
            }
            else if(Header.Type == -1)
            {
                SavePACV2(bw);
            } else
            {
                SavePACV1(bw);
            }
        }
    }
    private void SavePACV0(BinaryWriter bw)
    {
        // seems to only contain strings
        int sizeChange = 0;
        foreach (var sentry in StringEntries)
        {
            // update the size of the string
            byte[] text = Encoding.GetEncoding("shift_jis").GetBytes(sentry.Text);
            ushort StringSize = (ushort)(text.Count() + 8);
            sizeChange += StringSize - sentry.OriginalSize;
            sentry.OriginalSize = StringSize;
            bw.Write((Int16)sentry.ID);
            bw.Write((Int16)sentry.LineNumber);
            bw.Write((Int32)sentry.OriginalSize);
            bw.Write(text);
        }
    }

    private void SavePACV1(BinaryWriter bw)
    {
        // header
        bw.Write(Encoding.GetEncoding("shift_jis").GetBytes(Header.Magic));
        bw.Write(Header.Version);
        bw.Write(Header.Type);
        bw.Write(Header.FileSize);
        bw.Write(Header.AssetCount);
        bw.Write((Int16)StringEntries.Count);
        bw.Write(Header.DataSize);
        bw.Write(Header.Unknown);

        // write all binary data except the last entry as that will contain the strings which may have changed
        foreach (var bentry in BinaryEntries)
        {
            bw.Write(bentry.FileId);
            bw.Write(bentry.FileSize);
            bw.Write(bentry.Data);
        }

        // reverse back to overwrite the strings, 12 unknown bytes at start 
        bw.BaseStream.Position = 20 + Header.DataSize + 12;

        int sizeChange = 0;
        foreach (var sentry in StringEntries)
        {
            int headersize = 0;
            if (Header.Version == 2)
            {
                bw.Write((Int16)sentry.ID);
                bw.Write((Int16)sentry.LineNumber);
                bw.Write((Int16)sentry.Type);
                headersize = 8;
            }
            else
            {
                bw.Write((Int16)sentry.ID);
                bw.Write((byte)sentry.LineNumber);
                headersize = 4;
            }
            // update the size of the string
            byte[] text = Encoding.GetEncoding("shift_jis").GetBytes(sentry.Text);
            ushort StringSize = (ushort)(text.Count() + headersize);
            while(StringSize % 4 > 0)
            {
                StringSize++;
                sentry.Text += "\0";
            }
            text = Encoding.GetEncoding("shift_jis").GetBytes(sentry.Text);

            sizeChange += StringSize - sentry.OriginalSize;
            sentry.OriginalSize = StringSize;
            if (Header.Version == 2)
            {
                bw.Write((Int16)sentry.OriginalSize);
            }
            else
            {
                bw.Write((byte)sentry.OriginalSize);
            }
            bw.Write(text);
        }

        // reverse back and overwrite the total file size
        bw.BaseStream.Position = 8;
        Header.FileSize += sizeChange;
        bw.Write(Header.FileSize);
    }
    private void SavePACV2(BinaryWriter bw)
    {
        // seems to only contain strings
        int sizeChange = 0;
        foreach (var sentry in StringEntries)
        {
            bw.Write(new byte[3] { 0, 0, 0 });
            // update the size of the string
            byte[] text = Encoding.GetEncoding("shift_jis").GetBytes(sentry.Text);
            ushort StringSize = (ushort)(text.Count() + 4);
            sizeChange += StringSize - sentry.OriginalSize;
            sentry.OriginalSize = StringSize;
            bw.Write((byte)sentry.OriginalSize);
            bw.Write(text);

        }
    }

    public static string UpdateString(string InputString)
    {
        string newString = InputString;
        newString = newString.Replace("\r\n", "\n");
        newString = newString.Replace("\n", "\\n");
        byte[] text = Encoding.GetEncoding("shift_jis").GetBytes(newString);
        int remain = newString.Length % 4;
        while (remain > 0)
        {
            newString = newString + "\0";
            remain--;
        }
        return newString;
    }

    private static void CopyStringBetweenPACS(ref PAC DestPAC, ScriptEntry DestEntry, PAC SourcePAC, ScriptEntry SourceEntry, OptionsFileData? SourceTranslationOptions)
    {
        int destPacIndex = DestPAC.StringEntries.IndexOf(DestEntry);
        string sourceText = SourceEntry.Text;
        if(SourceTranslationOptions != null)
        {
            sourceText = TextTranslation.ConvertBackTextString(SourceTranslationOptions.Config.TranslationTable, sourceText);
        }
        DestPAC.StringEntries[destPacIndex].Text = UpdateString(TextTranslation.ConvertTextString(MainForm.OptionsFile.Config.TranslationTable, sourceText));


        DestPAC.StringEntries[destPacIndex].Type = SourceEntry.Type;
        if (SourceEntry.Data != null && SourceEntry.Data.Count() > 0)
        {
            DestPAC.StringEntries[destPacIndex].Data = new byte[SourceEntry.Data.Count()];
            SourceEntry.Data.CopyTo(DestPAC.StringEntries[destPacIndex].Data, 0);
        }

    }

    public static bool ImportStringsFromPACSourcePriority(ref PAC DestPAC, PAC SourcePAC, string SourceTranslationFile = "")
    {
        int copiedstrings = 0;
        List<ScriptEntry> FailedCopies = new List<ScriptEntry>();
        OptionsFileData? SourceTranslationOptions = null;
        if(!string.IsNullOrEmpty(SourceTranslationFile))
        {
            SourceTranslationOptions = new OptionsFileData();
            SourceTranslationOptions.Load(SourceTranslationFile);
        }
        // scan the strings
        foreach(var sourceentry in SourcePAC.StringEntries.FindAll(s => !s.IsLinked))
        {
            var destentry = DestPAC.StringEntries.Find(s => s.ID.Equals(sourceentry.ID) && s.LineNumber.Equals(sourceentry.LineNumber));
            if(destentry != null)
            {
                CopyStringBetweenPACS(ref DestPAC, destentry, SourcePAC, sourceentry, SourceTranslationOptions);
                copiedstrings++;
            } else
            {
                FailedCopies.Add(sourceentry);
            }
        }
        return true;
    }
    public static bool ImportStringsFromPACDestinationPriority(ref PAC DestPAC, PAC SourcePAC, string SourceTranslationFile = "")
    {
        int copiedstrings = 0;
        List<ScriptEntry> FailedCopies = new List<ScriptEntry>();
        OptionsFileData? SourceTranslationOptions = null;
        if (!string.IsNullOrEmpty(SourceTranslationFile))
        {
            SourceTranslationOptions = new OptionsFileData();
            SourceTranslationOptions.Load(SourceTranslationFile);
        }
        // scan the strings
        foreach (var destentry in DestPAC.StringEntries.FindAll(s => !s.IsLinked))
        {
            var sourceentry = SourcePAC.StringEntries.Find(s => s.ID.Equals(destentry.ID) && s.LineNumber.Equals(destentry.LineNumber));
            if (sourceentry != null)
            {
                CopyStringBetweenPACS(ref DestPAC, destentry, SourcePAC, sourceentry, SourceTranslationOptions);
                copiedstrings++;
            } else
            {
                FailedCopies.Add(destentry);
            }
        }
        int failedCopiesWithLineNumber1 = FailedCopies.FindAll(f => f.LineNumber == 1).Count();
        return true;
    }
}

