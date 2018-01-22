namespace Assets.Scripts.AssetBundle
{
    using UnityEngine;

    /// <summary>
    /// 加载包与卸载
    /// </summary>
    public class BundleHelper
    {
        public static BundleHelper Instance { get { return _instance ?? (_instance = new BundleHelper()); } }

        private static BundleHelper _instance;

        /// <summary>
        /// 当泛型T为Sprite，即你需要Load的类型为资源图时，请使用LoadSprite方法
        /// </summary>
        public T Load<T>(string assetBundleName, string assetName) where T : Object
        {
            BundleElement res = BundleManager.Instance.LoadReource(assetBundleName);
            var prefab = res.Bundle.LoadAsset<T>(assetName);
            //---补丁---当上面的Api无法加载时,使用下面的方式加载
            if (prefab == null)
            {
                GameObject obj = (GameObject)res.Bundle.LoadAsset(assetName);
                if (obj != null)
                    prefab = obj.GetComponent<T>();
            }
            return prefab;
        }
    }


}
