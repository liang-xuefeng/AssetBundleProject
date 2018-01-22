using System.Collections.Generic;

namespace Assets.Scripts.AssetBundle
{
    using UnityEngine;

    /// <summary>
    /// 包的管理，获取依赖
    /// </summary>
    public class BundleManager : MonoBehaviour
    {
        public Dictionary<string, BundleElement> BundleElements = new Dictionary<string, BundleElement>();
        public static BundleManager Instance { get { return _instance ?? new BundleManager(); } }

        /// <summary> 是否初始化化 </summary>
        public static bool isInit = false;

        private static BundleManager _instance;

        /// <summary> 总依赖 </summary>
        private AssetBundleManifest _manifest;
        /// <summary> 依赖的AB </summary>
        private AssetBundle _manifestBundle = null;

        /// <summary>
        /// 总依赖
        /// </summary>
        public AssetBundleManifest Manifest
        {
            get
            {
                if (_manifest == null)
                {
                    _manifestBundle = LoadAssetBundleSync(PathUtil.GetWWWPath() + "/" + PathUtil.GetPlatfromName());
                    if (_manifestBundle != null)
                    {
                        _manifest = (AssetBundleManifest)_manifestBundle.LoadAsset("AssetBundleManifest");
                    }
                }
                return _manifest;
            }
        }

        public BundleElement LoadReource(string assetName)
        {
            string path = PathUtil.GetWWWPath() + "/";

            //1.获取依赖文件列表  
            string[] depends = Manifest.GetAllDependencies(assetName+".u3d");

            //2.获取要加截的文件
            BundleElement res = LoadAsset(assetName, path);
            if (res == null)
                Debug.LogError(assetName);
            res.Dependencies = depends;
            return res;
        }

        /// <summary>
        /// 同步加载bundle
        /// </summary>
        private AssetBundle LoadAssetBundleSync(string path)
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(path);
            return bundle;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        private BundleElement LoadAsset(string assetName, string path)
        {
            BundleElement resource = null;
            if (BundleElements.TryGetValue(assetName, out resource))
            {
                resource.RefCount++;
            }
            else
            {
                //注:安卓平台无法使用AssetBundle.LoadFromFile 获取StreamingAssets目录下的资源

                AssetBundle bundle = LoadAssetBundleSync(path + assetName + ".u3d");

                if (bundle != null)
                {
                    resource = new BundleElement(assetName, path, bundle);
                    BundleElements.Add(assetName, resource);
                    Debug.Log("同步加载了:" + path);
                }
            }
            return resource;
        }
    }
}
