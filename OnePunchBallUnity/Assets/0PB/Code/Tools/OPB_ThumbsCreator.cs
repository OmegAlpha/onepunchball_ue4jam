using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class OPB_ThumbsCreator : MonoBehaviour
{
    [SerializeField]
    private OPB_PlayerSkins_Data skinsData;
    
    [SerializeField]
    private Camera camera;
    
    [SerializeField]
    private RenderTexture rTexture;
    
    [SerializeField]
    private MeshFilter meshFilter;
    
    [Button("Create")]
    public void CreateTextures()
    {
        var prevRT = RenderTexture.active;
        
        skinsData.Meshes_ArmorHead_Thumbs.Clear();
        skinsData.Meshes_ArmorHips_Thumbs.Clear();

        int i = 0;
        foreach (var headMesh in skinsData.Meshes_ArmorHead)
        {
            Texture2D txt = CreateThumb(headMesh, i, "head");
            skinsData.Meshes_ArmorHead_Thumbs.Add(txt);
            
            i++;
        }

        i = 0;
        foreach (var hipsMesh in skinsData.Meshes_ArmorHips)
        {
            Texture2D txt = CreateThumb(hipsMesh, i, "hips");
            skinsData.Meshes_ArmorHips_Thumbs.Add(txt);
            i++;
        }
        
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        
        RenderTexture.active = prevRT;
    }

    private Texture2D CreateThumb(Mesh mesh, int index, string subfix)
    {
        meshFilter.mesh = mesh;

        camera.Render();
            
        FitCameraToObject(meshFilter.gameObject);
            
        rTexture.Release();
            
        camera.Render();
        
        Texture2D text2d = new Texture2D(rTexture.width, rTexture.height);
        RenderTexture.active = rTexture;
        text2d.ReadPixels(new Rect(0, 0, rTexture.width, rTexture.height), 0, 0);
        text2d.Apply();

        string textureLocation = "Assets/0PB/0Data/GeneratedThumbs/";
        string textureName = "thumb_" + subfix + index.ToString("000") + ".png";
            
        File.WriteAllBytes( textureLocation + textureName, text2d.EncodeToPNG());
            
        return AssetDatabase.LoadAssetAtPath<Texture2D>(textureLocation + textureName );
    }


    private void FitCameraToObject(GameObject go)
    {
        MeshRenderer meshRendererer = go.GetComponent<MeshRenderer>();
        
        camera.transform.position = new Vector3(meshRendererer.bounds.center.x, meshRendererer.bounds.center.y, meshRendererer.transform.position.z - 3f);
        //meshRendererer.transform.position += meshRendererer.bounds.center;

        float biggerSide = Mathf.Max(meshRendererer.bounds.size.x, meshRendererer.bounds.size.y);
        camera.orthographicSize = biggerSide/ 2;
    }
}
