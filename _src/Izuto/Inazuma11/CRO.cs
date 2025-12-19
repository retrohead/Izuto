using Izuto.Extensions.BinaryTools;
using System.IO;
using System.Text;
using static Izuto.Inazuma11.FontTypes;

namespace Izuto.Inazuma11
{
    public class CRO
    {
        public class CROData
        {
            public byte[][] SHA256 = { new byte[32], new byte[32], new byte[32], new byte[32] };
            public byte[] Magic = new byte[4];
            public UInt32 NameOffset;
            public UInt32 NextModulePtr;
            public UInt32 PreviousModulePtr;
            public UInt32 FileSize;
            public UInt32 _zero_0;
            public UInt32 nnroControlObjectPtr;
            public UInt32 OnLoadPtr;
            public UInt32 OnExitPtr;
            public UInt32 OnUnresolvedPtr;
            public UInt32 CodeOffset;
            public UInt32 CodeSize;
            public UInt32 DataOffset;
            public UInt32 DataSize;
            public UInt32 ModuleNameOffset;
            public UInt32 ModuleNameSize;
            public UInt32 SegmentTableOffset;
            public UInt32 SegmentCount;
            public UInt32 ExportedNamedSymbolOffset;
            public UInt32 ExportedNamedSymbolCount;
            public UInt32 ExportedIndexedSymbolOffset;
            public UInt32 ExportedIndexedSymbolCount;
            public UInt32 ExportedStringsOffset;
            public UInt32 ExportedStringsSize;
            public UInt32 ExportedNameTreeOffset;
            public UInt32 ExportedNameTreeNodeCount;
            public UInt32 ImportedModuleTableOffset;
            public UInt32 ImportedModuleCount;
            public UInt32 ExternalPatchTableOffset;
            public UInt32 ExternalPatchCount;
            public UInt32 ImportedNamedSymbolTableOffset;
            public UInt32 ImportedNamedSymbolCount;
            public UInt32 ImportedIndexedSymbolTableOffset;
            public UInt32 ImportedIndexedSymbolCount;
            public UInt32 ImportedAnonymousSymbolTableOffset;
            public UInt32 ImportedAnonymousSymbolCount;
            public UInt32 ImportedStringsTableOffset;
            public UInt32 ImportedStringsSize;
            public UInt32 StaticAnonymousSymbolTableOffset;
            public UInt32 StaticAnonymousSymbolCount;
            public UInt32 InternalPatchTableOffset;
            public UInt32 InternalPatchCount;
            public UInt32 StaticAnonymousSymbolTableOffset_2;
            public UInt32 StaticAnonymousSymbolCount_2;
        }

        public class HeaderData
        {
            public string Magic = "";
            public string Name = "";
            public string FilePath = "";
            public UInt32 FileSize;
        }

        public enum MenuStringsType
        {
            Header,
            Friends,
            Inventory,
            Formation,
            Information,
            System,
            Save,
            NotKnownYet1,
            NotKnownYet2,
            NotKnownYet3,
            NotKnownYet4,
            NotKnownYet5
        }

        public class MenuStringType
        {
            public string String = "";
            public long Offset = -1;
            public int Capacity = -1;
            public FontType FontType = FontType.FONT12;
            public bool IsAlligned = false;
            public bool IsCapactiyRestricted = false;

            public MenuStringType(string str, long offset, int capacity, FontType fontType, bool isAlligned, bool isCapactiyRestricted)
            {
                String = str;
                Offset = offset;
                Capacity = capacity;
                FontType = fontType;
                IsAlligned = isAlligned;
                IsCapactiyRestricted = isCapactiyRestricted;
            }
        }

        private byte[] MenuStringUnknown = new byte[4];


        public Dictionary<MenuStringsType, MenuStringType> MenuStrings = new Dictionary<MenuStringsType, MenuStringType>();


        public CROData Data = new CROData();
        public HeaderData Header = new HeaderData();

        public int LoadedStringBatchSize = 0;

        public async Task<ActionResult> Load(string filename)
        {
            if (!File.Exists(filename)) return new ActionResult(false, "File does not exist");
            ActionResult result = new ActionResult(false, "Unknown error");
            await Task.Run(() =>
            {
                using (BinaryReaderX br = new BinaryReaderX(File.OpenRead(filename)))
                {
                    br.BaseStream.Position = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        Data.SHA256[i] = br.ReadBytes(32);
                    }
                    Data.Magic = br.ReadBytes(4);

                    if (!Encoding.ASCII.GetString(Data.Magic).StartsWith("CRO"))
                        result = new ActionResult(false, $"File is not a valid CRO file:\n\n{filename}");

                    Data.NameOffset = br.ReadUInt32();
                    Data.NextModulePtr = br.ReadUInt32();
                    Data.PreviousModulePtr = br.ReadUInt32();
                    Data.FileSize = br.ReadUInt32();
                    Data._zero_0 = br.ReadUInt32();
                    Data.nnroControlObjectPtr = br.ReadUInt32();
                    Data.OnLoadPtr = br.ReadUInt32();
                    Data.OnExitPtr = br.ReadUInt32();
                    Data.OnUnresolvedPtr = br.ReadUInt32();
                    Data.CodeOffset = br.ReadUInt32();
                    Data.CodeSize = br.ReadUInt32();
                    Data.DataOffset = br.ReadUInt32();
                    Data.DataSize = br.ReadUInt32();
                    Data.ModuleNameOffset = br.ReadUInt32();
                    Data.ModuleNameSize = br.ReadUInt32();
                    Data.SegmentTableOffset = br.ReadUInt32();
                    Data.SegmentCount = br.ReadUInt32();
                    Data.ExportedNamedSymbolOffset = br.ReadUInt32();
                    Data.ExportedNamedSymbolCount = br.ReadUInt32();
                    Data.ExportedIndexedSymbolOffset = br.ReadUInt32();
                    Data.ExportedIndexedSymbolCount = br.ReadUInt32();
                    Data.ExportedStringsOffset = br.ReadUInt32();
                    Data.ExportedStringsSize = br.ReadUInt32();
                    Data.ExportedNameTreeOffset = br.ReadUInt32();
                    Data.ExportedNameTreeNodeCount = br.ReadUInt32();
                    Data.ImportedModuleTableOffset = br.ReadUInt32();
                    Data.ImportedModuleCount = br.ReadUInt32();
                    Data.ExternalPatchTableOffset = br.ReadUInt32();
                    Data.ExternalPatchCount = br.ReadUInt32();
                    Data.ImportedNamedSymbolTableOffset = br.ReadUInt32();
                    Data.ImportedNamedSymbolCount = br.ReadUInt32();
                    Data.ImportedIndexedSymbolTableOffset = br.ReadUInt32();
                    Data.ImportedIndexedSymbolCount = br.ReadUInt32();
                    Data.ImportedAnonymousSymbolTableOffset = br.ReadUInt32();
                    Data.ImportedAnonymousSymbolCount = br.ReadUInt32();
                    Data.ImportedStringsTableOffset = br.ReadUInt32();
                    Data.ImportedStringsSize = br.ReadUInt32();
                    Data.StaticAnonymousSymbolTableOffset = br.ReadUInt32();
                    Data.StaticAnonymousSymbolCount = br.ReadUInt32();
                    Data.InternalPatchTableOffset = br.ReadUInt32();
                    Data.InternalPatchCount = br.ReadUInt32();
                    Data.StaticAnonymousSymbolTableOffset_2 = br.ReadUInt32();
                    Data.StaticAnonymousSymbolCount_2 = br.ReadUInt32();

                    // Header
                    Header = new HeaderData()
                    {
                        FilePath = filename,
                        Magic = Encoding.ASCII.GetString(Data.Magic),
                        FileSize = Data.FileSize
                    };
                    br.BaseStream.Position = Data.NameOffset;
                    Header.Name = br.ReadAlignedString(4, Encoding.ASCII);

                    ReadMenuStrings(br);

                    result = new ActionResult(true, "OK");
                }
            });
            return result;
        }

        private MenuStringType ReadMenuString(BinaryReaderX br, Encoding encoding, FontType fontType)
        {
            long offset = br.BaseStream.Position;
            string str = br.ReadNullTerminatedString(encoding);
            int capacity = (int)(br.BaseStream.Position - offset);
            return new MenuStringType(str, offset, capacity, fontType, false, false);
        }

        private MenuStringType ReadAlignedMenuString(BinaryReaderX br, int alignTo, Encoding encoding, FontType fontType)
        {
            long offset = br.BaseStream.Position;
            string str = br.ReadAlignedString(alignTo, encoding);
            int capacity = (int)(br.BaseStream.Position - offset);
            return new MenuStringType(str, offset, capacity, fontType, true, false);
        }
        private MenuStringType ReadSizedMenuString(BinaryReaderX br, int size, Encoding encoding, FontType fontType)
        {
            long offset = br.BaseStream.Position;
            string str = br.ReadNullTerminatedString(encoding);

            int capacity = size;
            while (br.BaseStream.Position != offset + size)
                br.ReadByte(); // skipping zero bytes if any
            return new MenuStringType(str, offset, capacity, fontType, true, true);
        }

        private long GetMenuStringPostion()
        {
            long pos = -1;
            if (Header.Name == "ina_main1")
            {
                pos = 0x000C7D34;
            }
            else if (Header.Name == "ina_main2")
            {
                pos = 0x000FBF60;
            }
            else if (Header.Name == "ina_main3ogre")
            {
                pos = 0x00165E80;
            }
            return pos;
        }

        private void ReadMenuStrings(BinaryReaderX br)
        {
            MenuStrings = new Dictionary<MenuStringsType, MenuStringType>();
            var sjis = Encoding.GetEncoding("shift_jis");
            long pos = GetMenuStringPostion();
            if (pos == -1)
                return;
            br.BaseStream.Position = pos;
            MenuStrings.Add(MenuStringsType.Header, ReadSizedMenuString(br, 12, sjis, FontType.FONT8)); // 12 bytes fixed
            MenuStrings.Add(MenuStringsType.Friends, ReadMenuString(br, sjis, FontType.FONT12));
            MenuStrings.Add(MenuStringsType.Inventory, ReadMenuString(br, sjis, FontType.FONT12));
            MenuStrings.Add(MenuStringsType.Formation, ReadMenuString(br, sjis, FontType.FONT12));
            MenuStrings.Add(MenuStringsType.Information, ReadMenuString(br, sjis, FontType.FONT12));
            MenuStrings.Add(MenuStringsType.System, ReadMenuString(br, sjis, FontType.FONT12));
            MenuStrings.Add(MenuStringsType.Save, ReadAlignedMenuString(br, 4, sjis, FontType.FONT12));
            MenuStringUnknown = br.ReadBytes(4); // unknown integer
            MenuStrings.Add(MenuStringsType.NotKnownYet1, ReadAlignedMenuString(br, 4, sjis, FontType.FONT12));
            MenuStrings.Add(MenuStringsType.NotKnownYet2, ReadMenuString(br, sjis, FontType.FONT12));
            MenuStrings.Add(MenuStringsType.NotKnownYet3, ReadMenuString(br, sjis, FontType.FONT12));

            if (Header.Name != "ina_main1" && Header.Name != "ina_menu")
            {
                MenuStrings.Add(MenuStringsType.NotKnownYet4, ReadMenuString(br, sjis, FontType.FONT12));
                MenuStrings.Add(MenuStringsType.NotKnownYet5, ReadAlignedMenuString(br, 4, sjis, FontType.FONT12));
            }
            else
            {
                MenuStrings.Add(MenuStringsType.NotKnownYet4, ReadAlignedMenuString(br, 4, sjis, FontType.FONT12));
            }

            LoadedStringBatchSize = (int)(br.BaseStream.Position - pos);
        }

        public string ReadDataAsString()
        {

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(Header.Name);
            stringBuilder.AppendLine("------------------------");
            for (int i = 0; i < 4; i++)
            {
                stringBuilder.AppendLine($"SHA256[{i}]: {BitConverter.ToString(Data.SHA256[i]).Replace("-", "")}");
            }
            stringBuilder.AppendLine($"Magic: {Header.Magic}");
            stringBuilder.AppendLine($"NameOffset: 0x{Data.NameOffset.ToString("X8")}");
            stringBuilder.AppendLine($"NextModulePtr: 0x{Data.NextModulePtr.ToString("X8")}");
            stringBuilder.AppendLine($"PreviousModulePtr: 0x{Data.PreviousModulePtr.ToString("X8")}");
            stringBuilder.AppendLine($"FileSize: {Data.FileSize} bytes");
            stringBuilder.AppendLine($"Zero: {Data._zero_0}");
            stringBuilder.AppendLine($"nnroControlObjectPtr: 0x{Data.nnroControlObjectPtr.ToString("X8")}");
            stringBuilder.AppendLine($"OnLoadPtr: 0x{Data.OnLoadPtr.ToString("X8")}");
            stringBuilder.AppendLine($"OnExitPtr: 0x{Data.OnExitPtr.ToString("X8")}");
            stringBuilder.AppendLine($"OnUnresolvedPtr: 0x{Data.OnUnresolvedPtr.ToString("X8")}");
            stringBuilder.AppendLine($"Code: 0x{Data.CodeOffset.ToString("X8")} - 0x{(Data.CodeOffset + Data.CodeSize).ToString("X8")} | {Data.CodeSize} bytes");
            stringBuilder.AppendLine($"Data: 0x{Data.DataOffset.ToString("X8")} - 0x{(Data.DataOffset + Data.DataSize).ToString("X8")} | {Data.DataSize} bytes");
            stringBuilder.AppendLine($"ModuleName: 0x{Data.ModuleNameOffset.ToString("X8")} | {Data.ModuleNameSize} bytes");
            stringBuilder.AppendLine($"SegmentTable: 0x{Data.SegmentTableOffset.ToString("X8")} | {Data.SegmentCount} entries");
            stringBuilder.AppendLine($"ExportedNamedSymbol: 0x{Data.ExportedNamedSymbolOffset.ToString("X8")} | {Data.ExportedNamedSymbolCount} entries");
            stringBuilder.AppendLine($"ExportedIndexedSymbol: 0x{Data.ExportedIndexedSymbolOffset.ToString("X8")} | {Data.ExportedIndexedSymbolCount} entries");
            stringBuilder.AppendLine($"ExportedStrings: 0x{Data.ExportedStringsOffset.ToString("X8")} | {Data.ExportedStringsSize} bytes");
            stringBuilder.AppendLine($"ExportedNameTree: 0x{Data.ExportedNameTreeOffset.ToString("X8")} | {Data.ExportedNameTreeNodeCount} nodes");
            stringBuilder.AppendLine($"ImportedModuleTable: 0x{Data.ImportedModuleTableOffset.ToString("X8")} | {Data.ImportedModuleCount} entries");
            stringBuilder.AppendLine($"ExternalPatchTable: 0x{Data.ExternalPatchTableOffset.ToString("X8")} | {Data.ExternalPatchCount} entries");
            stringBuilder.AppendLine($"ImportedNamedSymbolTable: 0x{Data.ImportedNamedSymbolTableOffset.ToString("X8")} | {Data.ImportedNamedSymbolCount} entries");
            stringBuilder.AppendLine($"ImportedIndexedSymbolTable: 0x{Data.ImportedIndexedSymbolTableOffset.ToString("X8")} | {Data.ImportedIndexedSymbolCount} entries");
            stringBuilder.AppendLine($"ImportedAnonymousSymbolTable: 0x{Data.ImportedAnonymousSymbolTableOffset.ToString("X8")} | {Data.ImportedAnonymousSymbolCount} entries");
            stringBuilder.AppendLine($"ImportedStringsTable: 0x{Data.ImportedStringsTableOffset.ToString("X8")} | {Data.ImportedStringsSize} bytes");
            stringBuilder.AppendLine($"StaticAnonymousSymbolTable: 0x{Data.StaticAnonymousSymbolTableOffset.ToString("X8")} | {Data.StaticAnonymousSymbolCount} entries");
            stringBuilder.AppendLine($"InternalPatchTable: 0x{Data.InternalPatchTableOffset.ToString("X8")} | {Data.InternalPatchCount} entries");
            stringBuilder.AppendLine($"StaticAnonymousSymbolTable 2: 0x{Data.StaticAnonymousSymbolTableOffset_2.ToString("X8")} | {Data.StaticAnonymousSymbolCount_2} entries");

            return stringBuilder.ToString();
        }


        public bool WriteMenuStrings()
        {

            long offset = GetMenuStringPostion();
            if (offset == -1)
                return false;
            // open the cro file for reading and copying to a new file

            string tmpFile = Header.FilePath + ".tmp";
            if (File.Exists(tmpFile))
                File.Delete(tmpFile);

            // open existing file
            using (BinaryReaderX br = new BinaryReaderX(File.OpenRead(Header.FilePath)))
            using (BinaryWriterX bw = new BinaryWriterX(tmpFile, true))
            {

                // write everything before the strings
                bw.WriteBytes(br.ReadBytes((int)offset), 0, (int)offset);

                // write the new strings
                int oldstringsize = 0;
                foreach (var str in MenuStrings)
                {
                    long currentPos = bw.BaseStream.Position;
                    byte[] strbytes = Encoding.GetEncoding("shift_jis").GetBytes(str.Value.String);
                    bw.WriteBytes(strbytes, 0, strbytes.Length);
                    bw.WriteByte((byte)0x00); // null terminator
                    int writtenbytes = 0;
                    while (str.Value.IsCapactiyRestricted && strbytes.Length + 1 + writtenbytes < str.Value.Capacity)
                    {
                        bw.WriteByte((byte)0x00); // null terminators for restricted size strings
                        writtenbytes++;
                    }
                    if (str.Value.IsAlligned)
                    {
                        // align to 4 bytes
                        long padding = (4 - (bw.BaseStream.Position % 4)) % 4;
                        for (long i = 0; i < padding; i++)
                        {
                            bw.WriteByte((byte)0x00);
                        }
                    }
                    oldstringsize += str.Value.Capacity;

                    if (str.Key == MenuStringsType.Save)
                    {
                        oldstringsize += MenuStringUnknown.Length;
                        // write the unknown bytes after this string
                        bw.WriteBytes(MenuStringUnknown, 0, MenuStringUnknown.Length);
                    }
                }
                int newstringsize = (int)(bw.BaseStream.Position - offset);
                int stringSizeDiff = newstringsize - oldstringsize;
                // skip the strings in the reader
                br.BaseStream.Position = offset + oldstringsize;

                // write up to the end of the data section but trim any padding off
                int dataSectionLeftToRead = (int)(Data.DataOffset + Data.DataSize - br.BaseStream.Position);
                if(stringSizeDiff > 0)
                    dataSectionLeftToRead -= stringSizeDiff; // trim padding if we have added size
                bw.WriteBytes(br.ReadBytes(dataSectionLeftToRead), 0, dataSectionLeftToRead);
                if (stringSizeDiff > 0)
                {
                    // skip the bytes we consumed
                    br.ReadBytes(stringSizeDiff);
                }
                else
                {
                    // add the padding we gained from shortening a string
                    while (bw.BaseStream.Position != br.BaseStream.Position)
                        bw.WriteByte(0x00);
                }

                if (bw.BaseStream.Position != Data.DataOffset + Data.DataSize || br.BaseStream.Position != Data.DataOffset + Data.DataSize)
                    throw new Exception("Logic error");

                // pad the data section to 0x1000 00 filled, should not change if size is > 0 so going to throw
                int paddingNeeded = (int)(0x1000 - (bw.BaseStream.Position % 0x1000)) % 0x1000;
                if(paddingNeeded != 0)
                    throw new Exception("Logic error");

                // write the rest of the file
                int leftToRead = (int)(br.BaseStream.Length - br.BaseStream.Position);
                bw.WriteBytes(br.ReadBytes(leftToRead), 0, leftToRead);

                bw.Dispose();
            }
            File.Delete(Header.FilePath);
            File.Copy(tmpFile, Header.FilePath);
            return true;
        }

        private void SerializeHeader(BinaryWriterX bw)
        {

            // annoyingly checking all offsets
            //if (Data.NameOffset > offset && Data.NameOffset < origfilesize) Data.NameOffset += (uint)sizeDiff;
            //if (Data.OnLoadPtr > offset && Data.OnLoadPtr < origfilesize) Data.OnLoadPtr += (uint)sizeDiff;
            //if (Data.OnExitPtr > offset && Data.OnExitPtr < origfilesize) Data.OnExitPtr += (uint)sizeDiff;
            //if (Data.OnUnresolvedPtr > offset && Data.OnUnresolvedPtr < origfilesize) Data.OnUnresolvedPtr += (uint)sizeDiff;
            //if (Data.CodeOffset > offset && Data.CodeOffset < origfilesize) Data.CodeOffset += (uint)sizeDiff;
            //if (Data.ModuleNameOffset > offset && Data.ModuleNameOffset < origfilesize) Data.ModuleNameOffset += (uint)sizeDiff;
            //if (Data.SegmentTableOffset > offset && Data.SegmentTableOffset < origfilesize) Data.SegmentTableOffset += (uint)sizeDiff;
            //if (Data.ExportedNamedSymbolOffset > offset && Data.ExportedNamedSymbolOffset < origfilesize) Data.ExportedNamedSymbolOffset += (uint)sizeDiff;
            //if (Data.ExportedIndexedSymbolOffset > offset && Data.ExportedIndexedSymbolOffset < origfilesize) Data.ExportedIndexedSymbolOffset += (uint)sizeDiff;
            //if (Data.ExportedStringsOffset > offset && Data.ExportedStringsOffset < origfilesize) Data.ExportedStringsOffset += (uint)sizeDiff;
            //if (Data.ExportedNameTreeOffset > offset && Data.ExportedNameTreeOffset < origfilesize) Data.ExportedNameTreeOffset += (uint)sizeDiff;
            //if (Data.ImportedModuleTableOffset > offset && Data.ImportedModuleTableOffset < origfilesize) Data.ImportedModuleTableOffset += (uint)sizeDiff;
            //if (Data.ExternalPatchTableOffset > offset && Data.ExternalPatchTableOffset < origfilesize) Data.ExternalPatchTableOffset += (uint)sizeDiff;
            //if (Data.ImportedNamedSymbolTableOffset > offset && Data.ImportedNamedSymbolTableOffset < origfilesize) Data.ImportedNamedSymbolTableOffset += (uint)sizeDiff;
            //if (Data.ImportedIndexedSymbolTableOffset > offset && Data.ImportedIndexedSymbolTableOffset < origfilesize) Data.ImportedIndexedSymbolTableOffset += (uint)sizeDiff;
            //if (Data.ImportedAnonymousSymbolTableOffset > offset && Data.ImportedAnonymousSymbolTableOffset < origfilesize) Data.ImportedAnonymousSymbolTableOffset += (uint)sizeDiff;
            //if (Data.ImportedStringsTableOffset > offset && Data.ImportedStringsTableOffset < origfilesize) Data.ImportedStringsTableOffset += (uint)sizeDiff;
            //if (Data.StaticAnonymousSymbolTableOffset > offset && Data.StaticAnonymousSymbolTableOffset < origfilesize) Data.StaticAnonymousSymbolTableOffset += (uint)sizeDiff;
            //if (Data.InternalPatchTableOffset > offset && Data.InternalPatchTableOffset < origfilesize) Data.InternalPatchTableOffset += (uint)sizeDiff;
            //if (Data.StaticAnonymousSymbolTableOffset_2 > offset && Data.StaticAnonymousSymbolTableOffset_2 < origfilesize) Data.StaticAnonymousSymbolTableOffset_2 += (uint)sizeDiff;

            // rewrite the header
            bw.BaseStream.Position = 132;

            bw.WriteUInt32(Data.NameOffset);
            bw.WriteUInt32(Data.NextModulePtr);
            bw.WriteUInt32(Data.PreviousModulePtr);
            bw.WriteUInt32(Data.FileSize);
            bw.WriteUInt32(Data._zero_0);
            bw.WriteUInt32(Data.nnroControlObjectPtr);
            bw.WriteUInt32(Data.OnLoadPtr);
            bw.WriteUInt32(Data.OnExitPtr);
            bw.WriteUInt32(Data.OnUnresolvedPtr);
            bw.WriteUInt32(Data.CodeOffset);
            bw.WriteUInt32(Data.CodeSize);
            bw.WriteUInt32(Data.DataOffset);
            bw.WriteUInt32(Data.DataSize);
            bw.WriteUInt32(Data.ModuleNameOffset);
            bw.WriteUInt32(Data.ModuleNameSize);
            bw.WriteUInt32(Data.SegmentTableOffset);
            bw.WriteUInt32(Data.SegmentCount);
            bw.WriteUInt32(Data.ExportedNamedSymbolOffset);
            bw.WriteUInt32(Data.ExportedNamedSymbolCount);
            bw.WriteUInt32(Data.ExportedIndexedSymbolOffset);
            bw.WriteUInt32(Data.ExportedIndexedSymbolCount);
            bw.WriteUInt32(Data.ExportedStringsOffset);
            bw.WriteUInt32(Data.ExportedStringsSize);
            bw.WriteUInt32(Data.ExportedNameTreeOffset);
            bw.WriteUInt32(Data.ExportedNameTreeNodeCount);
            bw.WriteUInt32(Data.ImportedModuleTableOffset);
            bw.WriteUInt32(Data.ImportedModuleCount);
            bw.WriteUInt32(Data.ExternalPatchTableOffset);
            bw.WriteUInt32(Data.ExternalPatchCount);
            bw.WriteUInt32(Data.ImportedNamedSymbolTableOffset);
            bw.WriteUInt32(Data.ImportedNamedSymbolCount);
            bw.WriteUInt32(Data.ImportedIndexedSymbolTableOffset);
            bw.WriteUInt32(Data.ImportedIndexedSymbolCount);
            bw.WriteUInt32(Data.ImportedAnonymousSymbolTableOffset);
            bw.WriteUInt32(Data.ImportedAnonymousSymbolCount);
            bw.WriteUInt32(Data.ImportedStringsTableOffset);
            bw.WriteUInt32(Data.ImportedStringsSize);
            bw.WriteUInt32(Data.StaticAnonymousSymbolTableOffset);
            bw.WriteUInt32(Data.StaticAnonymousSymbolCount);
            bw.WriteUInt32(Data.InternalPatchTableOffset);
            bw.WriteUInt32(Data.InternalPatchCount);
            bw.WriteUInt32(Data.StaticAnonymousSymbolTableOffset_2);
            bw.WriteUInt32(Data.StaticAnonymousSymbolCount_2);
        }

    }
}
