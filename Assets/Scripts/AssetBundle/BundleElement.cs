namespace Assets.Scripts.AssetBundle
{
    using UnityEngine;

    /// <summary>
    /// 游戏资源(bundle封装类)
    /// </summary>
    public class BundleElement
    {
        /// <summary> 路径 </summary>
        public string Path;
        /// <summary> 被引用次数 </summary>
        public int RefCount;
        /// <summary> bundle包 </summary>
        public AssetBundle Bundle;
        /// <summary> 依赖文件 </summary>
        public string[] Dependencies;
        /// <summary> bundle名 </summary>
        public string AssetName;
        /// <summary> 资源 </summary>
        private Object asset;

        public BundleElement(string assetName, string path, AssetBundle bundle, Object asset = null)
        {
            AssetName = assetName;
            Path = path;
            Bundle = bundle;
            RefCount = 1;
        }

        /// <summary> 返回这个包中的文件 </summary>
        public Object Asset{ get { return asset ?? (asset = Bundle.LoadAsset(AssetName)); } }

        /// <summary> 释放资源 </summary>
        public void UnLoad(bool param = true)
        {
            //if (CResourceManager.showDebug)
                Debug.LogWarning("释放资源:" + Path);

            Bundle.Unload(param);
        }
    }
}

