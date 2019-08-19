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
            meshFilter.mesh = headMesh;
            
            rTexture.Release();
            
            camera.Render();
        
            Texture2D text2d = new Texture2D(rTexture.width, rTexture.height);
            RenderTexture.active = rTexture;
            text2d.ReadPixels(new Rect(0, 0, rTexture.width, rTexture.height), 0, 0);
            text2d.Apply();

            string textureLocation = "Assets/0PB/0Data/GeneratedThumbs/";
            string textureName = "thumb_head_" + i.ToString("000") + ".png";
            
            File.WriteAllBytes( textureLocation + textureName, text2d.EncodeToPNG());
            
            Texture2D textureJustCreated = AssetDatabase.LoadAssetAtPath<Texture2D>(textureLocation + textureName );
            skinsData.Meshes_ArmorHead_Thumbs.Add(textureJustCreated);
            
            i++;
        }

        i = 0;
        foreach (var hipsMesh in skinsData.Meshes_ArmorHips)
        {
            meshFilter.mesh = hipsMesh;

            camera.Render();
            
            FitCameraToObject(meshFilter.gameObject);
            
            rTexture.Release();
            
            camera.Render();
        
            Texture2D text2d = new Texture2D(rTexture.width, rTexture.height);
            RenderTexture.active = rTexture;
            text2d.ReadPixels(new Rect(0, 0, rTexture.width, rTexture.height), 0, 0);
            text2d.Apply();

            string textureLocation = "Assets/0PB/0Data/GeneratedThumbs/";
            string textureName = "thumb_hips_" + i.ToString("000") + ".png";
            
            File.WriteAllBytes( textureLocation + textureName, text2d.EncodeToPNG());
            
            Texture2D textureJustCreated = AssetDatabase.LoadAssetAtPath<Texture2D>(textureLocation + textureName );
            skinsData.Meshes_ArmorHips_Thumbs.Add(textureJustCreated);
            
            i++;
        }
        
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        
        RenderTexture.active = prevRT;
    }

    private void FitCameraToObject(GameObject go)
    {
        MeshRenderer meshRendererer = go.GetComponent<MeshRenderer>();
        
        float screenRatio = 1f;
        
        meshRendererer.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, meshRendererer.bounds.center.z);
        //meshRendererer.transform.position += meshRendererer.bounds.center;
        
        float targetRatio = meshRendererer.bounds.size.x / meshRendererer.bounds.size.y;
 
        if (screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = meshRendererer.bounds.size.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = meshRendererer.bounds.size.y / 2 * differenceInSize;
        }
 
        transform.position = new Vector3(meshRendererer.bounds.center.x, meshRendererer.bounds.center.y, -1f);
    }
}
