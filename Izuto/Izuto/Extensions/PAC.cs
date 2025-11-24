using System.Text;

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
        public Int16 ID;
        public byte LineNumber;
        public byte Size;
        public string Text = "";
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
        public byte[] Unknown = new byte[12]; // 0x0014 – unknown
    }

    public HeaderInfo Header = new HeaderInfo();
    public List<BinaryEntry> BinaryEntries = new List<BinaryEntry>(); //0x0020
    public List<ScriptEntry> StringEntries = new List<ScriptEntry>(); //0x0020 + DataSize

    public void Load(string fn)
    {

        using (var fs = File.OpenRead(fn))
        using (var br = new BinaryReader(fs))
        {
            // --- Read header ---
            byte[] magicbytes = br.ReadBytes(4);
            Header.Magic = System.Text.Encoding.GetEncoding("shift_jis").GetString(magicbytes); // "SSD\0" 4 bytes
            if (Header.Magic != "SSD\0")
            {

            }
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
                for (int i = 0; i < Header.StringCount; i++)
                {
                    ScriptEntry sentry = new ScriptEntry();
                    sentry.ID = br.ReadInt16();   // 2 bytes
                    sentry.LineNumber = br.ReadByte();   // 1 byte
                    sentry.Size = br.ReadByte();   // 1 byte
                    byte[] bytes = br.ReadBytes(sentry.Size - 4);
                    sentry.Text = System.Text.Encoding.GetEncoding("shift_jis").GetString(bytes);
                    StringEntries.Add(sentry);
                }
            }
            long bytesNotReadYet = Header.FileSize - br.BaseStream.Position;
            if (bytesNotReadYet != 0)
            {
                bytesNotReadYet = bytesNotReadYet;
            }
        }
    }

    public void SaveAs(string fn)
    {
        using (var fs = new FileStream(fn, FileMode.Create, FileAccess.Write))
        using (var bw = new BinaryWriter(fs))
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
                bw.Write(sentry.ID);
                bw.Write(sentry.LineNumber);
                // update the size of the string
                byte[] text = Encoding.GetEncoding("shift_jis").GetBytes(sentry.Text);
                ushort StringSize = (ushort)(text.Length + 4);
                sizeChange += sentry.Size - StringSize;
                sentry.Size = (byte)StringSize;
                bw.Write(sentry.Size);
                bw.Write(text);
            }

            // reverse back and overwrite the total file size
            bw.BaseStream.Position = 8;
            Header.FileSize += sizeChange;
            bw.Write(Header.FileSize);
        }
    }
}

