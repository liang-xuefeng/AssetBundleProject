using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 获取路径工具类
/// </summary>
public class PathUtil
{

    /// <summary>
    /// 获取assetbundl输出路径
    /// </summary>
    /// <returns></returns>
    public static string GetAssetBundleOutPath()
    {
        string outPath = GetPlatfromPath() + "/" + GetPlatfromName();
        if (!Directory.Exists(outPath))
            Directory.CreateDirectory(outPath);
        return outPath;
    }

    /// <summary>
    /// 获取WWW协议路径
    /// </summary>
    /// <returns></returns>
    public static string GetWWWPath()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return "jar:file://" + GetAssetBundleOutPath();
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                return /*"file:///" +*/ GetAssetBundleOutPath();
            default:
                return null;
        }
    }

    /// <summary>
    /// 自动获取对应平台的路径
    /// </summary>
    /// <returns></returns>
    private static string GetPlatfromPath()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return Application.persistentDataPath;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                return Application.streamingAssetsPath;
            default:
                return null;
        }
    }

    /// <summary>
    /// 自动获取对应平台的名字
    /// </summary>
    /// <returns></returns>
    public static string GetPlatfromName()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                return "Windows";
            default:
                return null;
        }
    }
	
}
