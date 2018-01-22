using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.AssetBundle;
using UnityEngine;

public class LoadAsset : MonoBehaviour
{

	void Start ()
	{
	    Instantiate(BundleHelper.Instance.Load<GameObject>("prefabs", "Cancel"));
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
