using UnityEditor;
using UnityEngine;

public class TXTFileAccessor {

    private static string path;
    private static TextAsset textAsset;
    private static string[] textAssetSplitLine;

    public static void SetPath(string _path)
    {
        path = _path;
        textAsset = (TextAsset) AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));
        textAssetSplitLine = textAsset.text.Split('\n');
    }

    public static string GetPath()
    {
        return path;
    }

    public static string ReadValue()
    {
        return textAsset.text;
    }

    public static string ReadValue(int line)
    {
        return textAssetSplitLine[line];
    }

}
