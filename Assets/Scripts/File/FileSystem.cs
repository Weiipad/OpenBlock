using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSystem : MonoBehaviour
{
    private string saveDir;

    public string GetSaveDirectory() => saveDir;
    public void SetSaveDirectory(string saveDir)
    {
        this.saveDir = saveDir;
    }
}
