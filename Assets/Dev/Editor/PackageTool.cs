using UnityEngine;
using UnityEditor;

public class PackageTool
{
	[MenuItem("Package/Update Package")]
	static void UpdatePackage()
	{
		AssetDatabase.ExportPackage( "Assets/AlignQuestCameraRigWithKnownGuardian", "AlignQuestCameraRigWithKnownGuardian.unitypackage", ExportPackageOptions.Recurse );
		Debug.Log( "Updated package\n" );
	}
}