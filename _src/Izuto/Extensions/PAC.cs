using Ekona;
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
        public int Size;
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
            sentry.Size = br.ReadInt32();   // 4 bytes
            int headerSize = 8;
            if (sentry.Size > 1)
            {
                sentry.TextBytes = br.ReadBytes(sentry.Size - headerSize);
                sentry.Text = System.Text.Encoding.GetEncoding("shift_jis").GetString(sentry.TextBytes);
            }
            sentry.Data = null;
            sentry.IsLinked = false;
            if (sentry.Text.StartsWith("@"))
            {
                // reverse back and scan the string again, storing the remaining part for analysis
                br.BaseStream.Position -= sentry.Size - headerSize;

                sentry.Text = sentry.Text.Split('\0')[0];
                byte[] text = Encoding.GetEncoding("shift_jis").GetBytes(sentry.Text);
                int len = text.Count();
                sentry.IsLinked = true;

                byte[] bytes = br.ReadBytes(len); // including null terminator
                sentry.Text = System.Text.Encoding.GetEncoding("shift_jis").GetString(bytes);

                // scan the remaining bytes into data
                if (sentry.Size - (len) - headerSize > 0)
                    sentry.Data = br.ReadBytes(sentry.Size - (len) - headerSize);
            }
            StringEntries.Add(sentry);
            Header.StringCount++;
            if (br.BaseStream.Position >= br.BaseStream.Length - 1)
                break; // finished reading, hopefully didn't go past the end of the stream :)
        }
    }

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
            sentry.Size = br.ReadByte();   // 1 byte
            if (sentry.Size > 1)
            {
                sentry.TextBytes = br.ReadBytes(sentry.Size - 4);
                sentry.Text = System.Text.Encoding.GetEncoding("shift_jis").GetString(sentry.TextBytes);
            }
            sentry.Data = null;
            sentry.IsLinked = false;
            if (sentry.Text.StartsWith("@"))
            {
                // reverse back and scan the string again, storing the remaining part for analysis
                br.BaseStream.Position -= sentry.Size - 4;

                sentry.Text = sentry.Text.Split('\0')[0];
                byte[] text = Encoding.GetEncoding("shift_jis").GetBytes(sentry.Text);
                int len = text.Count();
                sentry.IsLinked = true;

                byte[] bytes = br.ReadBytes(len); // including null terminator
                sentry.Text = System.Text.Encoding.GetEncoding("shift_jis").GetString(bytes);

                // scan the remaining bytes into data
                if (sentry.Size - (len) - 4 > 0)
                    sentry.Data = br.ReadBytes(sentry.Size - (len) - 4);
            }
            StringEntries.Add(sentry);
            Header.StringCount++;
            if (br.BaseStream.Position >= br.BaseStream.Length - 1)
                break; // finished reading, hopefully didn't go past the end of the stream :)
        }
    }

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
                sentry.ID = br.ReadInt16();   // 2 bytes
                sentry.LineNumber = br.ReadByte();   // 1 byte
                sentry.Size = br.ReadByte();   // 1 byte
                sentry.TextBytes = br.ReadBytes(sentry.Size - 4);
                sentry.Text = System.Text.Encoding.GetEncoding("shift_jis").GetString(sentry.TextBytes);
                sentry.Data = null;
                sentry.IsLinked = false;
                if (sentry.Text.StartsWith("@"))
                {
                    // reverse back and scan the string again, storing the remaining part for analysis
                    br.BaseStream.Position -= sentry.Size - 4;

                    sentry.Text = sentry.Text.Split('\0')[0];
                    byte[] text = Encoding.GetEncoding("shift_jis").GetBytes(sentry.Text);
                    int len = text.Count();
                    sentry.IsLinked = true;

                    byte[] bytes = br.ReadBytes(len); // including null terminator
                    sentry.Text = System.Text.Encoding.GetEncoding("shift_jis").GetString(bytes);

                    // scan the remaining bytes into data
                    if(sentry.Size - (len) - 4 > 0)
                        sentry.Data = br.ReadBytes(sentry.Size - (len) - 4);
                }
                StringEntries.Add(sentry);
            }
        }
        if (br.BaseStream.Position != br.BaseStream.Length)
        {
            MessageBox.Show("remaining bytes unread");
        }
    }

    public void SaveAs(string fn)
    {
        using (var fs = new FileStream(fn, FileMode.Create, FileAccess.Write))
        using (var bw = new BinaryWriter(fs))
        {
            if (Header.Type == -2)
            {
                //SavePACV0(bw);
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
            bw.Write((Int16)sentry.ID);
            bw.Write((byte)sentry.LineNumber);
            // update the size of the string
            byte[] text = Encoding.GetEncoding("shift_jis").GetBytes(sentry.Text);
            ushort StringSize = (ushort)(text.Count() + 4);
            while(StringSize % 4 > 0)
            {
                StringSize++;
                sentry.Text += "\0";
            }
            text = Encoding.GetEncoding("shift_jis").GetBytes(sentry.Text);

            sizeChange += StringSize - sentry.Size;
            sentry.Size = StringSize;
            bw.Write((byte)sentry.Size);
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
            sizeChange += StringSize - sentry.Size;
            sentry.Size = StringSize;
            bw.Write((byte)sentry.Size);
            bw.Write(text);

        }
    }
}

