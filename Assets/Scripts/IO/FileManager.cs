using OpenBlock.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace OpenBlock.IO
{
    public class FileManager : CommonSingleton<FileManager>
    {
        public string levelsDirName = "levels";
        public string modsDirName = "mods";
        public string levelInfoName = "info.json";

        private string levelsDirPath = null;
        public string levelsDirectoryPath
        {
            get
            {
                if (levelsDirPath == null) levelsDirPath = Path.Combine(savePath, levelsDirName);
                return levelsDirPath;
            }
        }
        public string savePath { get; private set; }
        public FileManager()
        {
            savePath = Application.persistentDataPath;
            
        }

        public string ReadExternalText(string path)
        {
            return System.IO.File.ReadAllText(path);
        }

        public void WriteExternalText(string path, string text)
        {
            System.IO.File.WriteAllText(path, text);
        }

        public bool GetLevelInfo(string levelDirName, out LevelInfo info)
        {
            string infoPath = Path.Combine(levelsDirectoryPath, levelDirName, levelInfoName);
            try
            {
                info = new LevelInfo(JsonUtility.FromJson<LevelInfoRaw>(ReadExternalText(infoPath)));
                return true;
            }
            catch (System.OverflowException)
            {
                info = null;
                return false;
            }
        }
    }

}