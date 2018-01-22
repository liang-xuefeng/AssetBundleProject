using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// asset bundle 编辑
/// </summary>
public class AssetBundleEditor
{

    #region 自动做标记

    [MenuItem("Asset Bundle/Set Lable")]
    public static void SetAssetBundleLable()
    {
        //移除所有没有使用的AB名
        AssetDatabase.RemoveUnusedAssetBundleNames();

        //资源总路径
        string assetDriectory = "E:/Work/Jobs/Empty/EmptyProject/Assets/Res";

        //遍历所有文件夹
        DirectoryInfo directoryInfo = new DirectoryInfo(assetDriectory);
        DirectoryInfo[]  directoryInfos = directoryInfo.GetDirectories();

        foreach (DirectoryInfo info in directoryInfos)
        {
            string sceneDirectory = assetDriectory + "/" + info.Name;
            DirectoryInfo sceneDirectoryInfo = new DirectoryInfo(sceneDirectory);

            if (sceneDirectoryInfo == null)
            {
                Debug.LogError(sceneDirectory + "不存在");
                return;
            }
            else
            {

                Dictionary<string,string> namePath = new Dictionary<string, string>();

                int index = sceneDirectory.LastIndexOf("/");
                string sceneName = sceneDirectory.Substring(index + 1);
                OnSceneFileSystemInfo(sceneDirectoryInfo, sceneName, namePath);

                WirteConfig(sceneName, namePath);
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("名字设置完成");
    }

    /// <summary>
    /// 写入路径的配置文件
    /// </summary>
    /// <param name="sceneDic"></param>
    /// <param name="namePathDic"></param>
    private static void WirteConfig(string sceneName, Dictionary<string, string> namePathDic)
    {
        string path = PathUtil.GetAssetBundleOutPath() +  "/" + sceneName + "Record.txt";

        using (FileStream fs = new FileStream(path,FileMode.OpenOrCreate,FileAccess.Write))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(namePathDic.Count);

                foreach (KeyValuePair<string, string> pair in namePathDic)
                {
                    sw.WriteLine(pair.Key + " " + pair.Value);
                }
            }
        }
    }

    /// <summary>
    /// 遍历资源文件夹里的场景文件
    /// </summary>
    /// <param name="info">场景文件夹</param>
    /// <param name="sceneName">场景文件夹名</param>
    private static void OnSceneFileSystemInfo(FileSystemInfo info, string sceneName, Dictionary<string, string> namePath)
    {
        if (!info.Exists)
        {
            Debug.LogError(info.FullName + "不存在");
            return;
        }

        DirectoryInfo directoryInfo = info as DirectoryInfo;

        FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
        foreach (FileSystemInfo systemInfo in fileSystemInfos)
        {
            FileInfo fileInfo = systemInfo as FileInfo;
            if (fileInfo == null)
            {
                //如果强转失败，则表示这是个文件夹，不是文件，继续遍历
                OnSceneFileSystemInfo(systemInfo, sceneName, namePath);
            }
            else
            {
                //设置文件的label
                SetLabels(fileInfo, sceneName, namePath);
            }
        }
    }

    /// <summary>
    /// 修改文件的AB名
    /// </summary>
    /// <param name="fileInfo">修改的文件</param>
    /// <param name="sceneName">场景名</param>
    private static void SetLabels(FileInfo fileInfo, string sceneName, Dictionary<string, string> namePath)
    {
        if (fileInfo.Extension == ".meta") return;

        string bundleName = GetBundleName(fileInfo, sceneName);

        int index = fileInfo.FullName.IndexOf("Assets");
        string assetPath = fileInfo.FullName.Substring(index);

        //修改Label类
        AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
        assetImporter.assetBundleName = bundleName.ToLower();
        assetImporter.assetBundleVariant = fileInfo.Extension == ".unity" ? "u3d" : "assetbundle";

        string folderName = bundleName.Contains("/") ? bundleName.Split('/')[1] : bundleName.Split('/')[0];
        string bundlePath = assetImporter.assetBundleName + "." + assetImporter.assetBundleVariant;

        //增加字典
        if (!namePath.ContainsKey(bundleName)) 
            namePath.Add(folderName, bundlePath);
    }

    /// <summary>
    /// 获取要打包的资源名
    /// </summary>
    private static string GetBundleName(FileInfo fileInfo, string sceneName)
    {
        string winPath = fileInfo.FullName;
        string unityPath = winPath.Replace(@"\", "/");  //字符转换

        int index = unityPath.IndexOf(sceneName) + sceneName.Length;
        string bundlePath = unityPath.Substring(index + 1);

        if (bundlePath.Contains("/"))
        {
            string[] tmpStrings = bundlePath.Split('/');
            return sceneName + "/" + tmpStrings[0];
        }
        else
        {
            //这个是场景
            return sceneName;
        }
    }

    #endregion

    #region 打包

    [MenuItem("Asset Bundle/Asset Bundle")]
    public static void BuildAssetBundle()
    {
        string outPath = PathUtil.GetAssetBundleOutPath();
        BuildPipeline.BuildAssetBundles(outPath, 0, BuildTarget.StandaloneWindows64);
    }

    #endregion

    #region 删除打包好的AB资源

    [MenuItem("Asset Bundle/Delet Asset Bundle")]
    public static void DeletAssetBundle()
    {
        string outPath = PathUtil.GetAssetBundleOutPath();
        Directory.Delete(outPath,true);
        File.Delete(outPath + ".meta");
        AssetDatabase.Refresh();
    }

    #endregion

}
