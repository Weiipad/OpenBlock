using OpenBlock.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.File
{
    public class FileManager : CommonSingleton<FileManager>
    {
        public string levelsDirName = "levels";
        public string modsDirName = "mods";
        public string savePath { get; private set; }
        public FileManager()
        {
            savePath = Application.persistentDataPath;
        }
    }

}