using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.IO
{
    public class LevelInfo
    {
        public LevelType type;
        public string levelName;
        public string levelFile;

        public LevelInfo(LevelInfoRaw raw)
        {
            type = (LevelType)Enum.Parse(typeof(LevelType), raw.type);
            levelName = raw.levelName;
            levelFile = raw.levelFile;
        }
    }

    [System.Serializable]
    public class LevelInfoRaw
    {
        public string type;
        public string levelName;
        public string levelFile;

        public LevelInfoRaw() { }

        public LevelInfoRaw(LevelInfo info)
        {
            type = info.type.ToString();
            levelName = info.levelName;
            levelFile = info.levelFile;
        }
    }
}
