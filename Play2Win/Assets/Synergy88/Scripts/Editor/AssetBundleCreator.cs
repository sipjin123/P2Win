using UnityEditor;

public class AssetBundleCreator {

	[MenuItem("Assets/Build AssetBundles/Android")]
	public static void BuildAssetBundleAndroid() {
		BuildPipeline.BuildAssetBundles("AssetBundles/Android", BuildAssetBundleOptions.None, BuildTarget.Android);
	}

	[MenuItem("Assets/Build AssetBundles/iOS")]
	public static void BuildAssetBundleIOS() {
		BuildPipeline.BuildAssetBundles("AssetBundles/iOS", BuildAssetBundleOptions.None, BuildTarget.iOS);
	}

	[MenuItem("Assets/Build AssetBundles/WebGL")]
	public static void BuildAssetBundleWebGL() {
		BuildPipeline.BuildAssetBundles("AssetBundles/WebGL", BuildAssetBundleOptions.None, BuildTarget.WebGL);
	}

	[MenuItem("Assets/Build AssetBundles/BuildAll")]
	public static void BuildAllAssetBundles()	{
		BuildAssetBundleAndroid();
		BuildAssetBundleIOS();
		BuildAssetBundleWebGL();
	}
}
