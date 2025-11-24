using Ekona.Images;
using Ekona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izuto.Extensions
{
    public class PluginHost : IPluginHost
    {
        ImageBase image;
        PaletteBase palette;
        MapBase map;
        SpriteBase sprite;
        Object objects;

        sFolder extraidos;
        string tempFolder;
        string _tempFolder; // Original

        public PluginHost()
        {
            // Se crea una carpeta temporal donde almacenar los archivos de salida como los descomprimidos.
            string[] subFolders = System.IO.Directory.GetDirectories(Application.StartupPath);
            for (int n = 0; ; n++)
            {
                if (!subFolders.Contains<string>(Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "Temp" + n))
                {
                    tempFolder = Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "Temp" + n;
                    System.IO.Directory.CreateDirectory(tempFolder);
                    _tempFolder = (string)tempFolder.Clone();
                    break;
                }
            }
        }
        public void Dispose()
        {
            //    try { System.IO.Directory.Delete(tempFolder, true); }
            //   catch { MessageBox.Show(Tools.Helper.GetTranslation("Messages", "S22")); }
            throw new NotImplementedException();
        }

        public Object Get_Object() { return objects; }

        public ImageBase Get_Image() { return image; }
        public PaletteBase Get_Palette() { return palette; }
        public MapBase Get_Map() { return map; }
        public SpriteBase Get_Sprite() { return sprite; }

        public void Set_Image(ImageBase image) { this.image = image; }
        public void Set_Palette(PaletteBase palette) { this.palette = palette; }
        public void Set_Map(MapBase map) { this.map = map; }
        public void Set_Sprite(SpriteBase sprite) { this.sprite = sprite; }

        public void Set_Object(Object objects) { this.objects = objects; }

        public string[] PluginList() { return event_PluginList(); }
        public event Func<string[]> event_PluginList;
        public Object Call_Plugin(string[] param, int id, int action) { return event_CallPlugin(param, id, action); }
        public event Func<string[], int, int, object> event_CallPlugin;

        public void Set_Files(sFolder archivos) { extraidos = archivos; }
        public sFolder Get_Files()
        {
            sFolder devuelta = extraidos;
            extraidos = new sFolder();
            return devuelta;
        }
        public event Func<int, sFolder> event_GetDecompressedFiles;
        public sFolder Get_DecompressedFiles(int id) { return event_GetDecompressedFiles(id); }

        public event Func<int, String> event_SearchFile;
        public String Search_File(int id) { return event_SearchFile(id); }
        public event Func<int, sFile> event_SearchFile2;
        public sFile Search_File(short id) { return event_SearchFile2(id); }
        public event Func<string, sFolder> event_SearchFileN;
        public sFolder Search_File(string name) { return event_SearchFileN(name); }
        public Byte[] Get_Bytes(string path, int offset, int length)
        {
            //    return Tools.Helper.Get_Bytes(offset, length, path);
            throw new NotImplementedException();
        }
        public event Func<int, sFolder> event_SearchFolder;
        public sFolder Search_Folder(int id) { return event_SearchFolder(id); }

        public string Get_LanguageFolder()
        {
            return Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "langs" + System.IO.Path.DirectorySeparatorChar;
        }

        public string Get_TempFile()
        {
            return tempFolder + System.IO.Path.DirectorySeparatorChar + System.IO.Path.GetRandomFileName();
        }
        public string Get_TempFolder() { return tempFolder; }
        public void Set_TempFolder(string newPath)
        {
            tempFolder = newPath;
        }
        public void Restore_TempFolder()
        {
            tempFolder = (string)_tempFolder.Clone();
        }

        public string Get_LangXML()
        { //return Tools.Helper.Get_LangXML(); 
            throw new NotImplementedException();
        }
        public string Get_Language()
        {
            System.Xml.Linq.XElement xml = System.Xml.Linq.XElement.Load(Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "Tinke.xml");
            return xml.Element("Options").Element("Language").Value;
        }

        public event Action<string> DescompressEvent;
        public void Decompress(string archivo)
        {
            DescompressEvent(archivo);
        }
        public void Decompress(byte[] data)
        {
            string temp = Get_TempFile();
            System.IO.File.WriteAllBytes(temp, data);
            DescompressEvent(temp);
            try { System.IO.File.Delete(temp); }
            catch { }
        }
        public void Compress(string filein, string fileout, FormatCompress format) { DSDecmp.Main.Compress(filein, fileout, format); }

        public event Action<int, string> ChangeFile_Event;
        public void ChangeFile(int id, string newFile) 
        { 
            if(ChangeFile_Event != null) 
                ChangeFile_Event(id, newFile); 
        }
    }
}
