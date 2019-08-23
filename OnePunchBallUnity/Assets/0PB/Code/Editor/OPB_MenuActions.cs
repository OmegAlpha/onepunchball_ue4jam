using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OPB_MenuActions 
{
    [InitializeOnLoadMethod]
    [MenuItem("OPB/Generate Data")]
    public static void GenerateData()
    {
		// Generate Player Skins Data
        // -> search for all the related skins meshes and populate the PlayerSkinsData ScriptableObject
        string[] guids_PlayerSkinsData = AssetDatabase.FindAssets("t:OPB_PlayerSkins_Data", new[] {"Assets/0PB/0Data"});
        
        OPB_PlayerSkins_Data playerSkinsData = AssetDatabase.LoadAssetAtPath<OPB_PlayerSkins_Data>(AssetDatabase.GUIDToAssetPath(guids_PlayerSkinsData[0]));
        playerSkinsData.Meshes_ArmorHead.Clear();
        playerSkinsData.Meshes_ArmorHips.Clear();
        
        // Find all Texture2Ds that have 'co' in their filename, that are labelled with 'architecture' and are placed in 'MyAwesomeProps' folder
        string[] guids_SkinsHead = AssetDatabase.FindAssets("t:Mesh", new[] {"Assets/0PB/Art/Characters/Accessories/Head"});

        foreach (string guid in guids_SkinsHead)
        {
            var loadedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(AssetDatabase.GUIDToAssetPath(guid));
            
            playerSkinsData.Meshes_ArmorHead.Add(loadedMesh);
        }
        
        string[] guids_SkinsHips = AssetDatabase.FindAssets("t:Mesh", new[] {"Assets/0PB/Art/Characters/Accessories/Hips"});

        foreach (string guid in guids_SkinsHips)
        {
            var loadedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(AssetDatabase.GUIDToAssetPath(guid));
            
            playerSkinsData.Meshes_ArmorHips.Add(loadedMesh);
        }
        
        string[] guids_SkinColorMaterials = AssetDatabase.FindAssets("t:Material", new[] {"Assets/0PB/0Data/SkinMaterialColors"});

        playerSkinsData.SkinMaterialColors.Clear();

        int i = 0;
        foreach (string guid in guids_SkinColorMaterials)
        {
            var loadedMaterial = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(guid));
            loadedMaterial.SetColor("_MainColor", playerSkinsData.ColorsPalette[i]);
            
            playerSkinsData.SkinMaterialColors.Add(loadedMaterial);
            
            i++;
        }
        
        
        // ------------- create Thumbs
        OPB_ThumbsCreator thumbsCreatorAsset = AssetDatabase.LoadAssetAtPath<OPB_ThumbsCreator>("Assets/0PB/Prefabs/Tools/ThumbsCreator.prefab");

        OPB_ThumbsCreator thumbsCreator = GameObject.Instantiate(thumbsCreatorAsset, Vector3.one * 1000f, Quaternion.identity);
        thumbsCreator.CreateTextures(true);
        GameObject.DestroyImmediate(thumbsCreator.gameObject);
    }
}
