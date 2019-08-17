using System.Collections;
using System.Collections.Generic;
using Org.BouncyCastle.Utilities;
using UnityEngine;

public class OPB_UI_PlayerSkinModel : MonoBehaviour
{
    [SerializeField]
    private OPB_PlayerSkins_Data skinsData;
    
    [SerializeField]
    private SkinnedMeshRenderer playerMeshRenderer;
    
    [SerializeField]
    private MeshRenderer meshRenderer_ArmorHead;
    
    [SerializeField]
    private MeshRenderer meshRenderer_ArmorHips;
    
    [SerializeField]
    private Material mat_Alive;
    
    [SerializeField]
    private Material mat_Dead;

    private string skinString = "";

    public void SelectIndex_ArmorHead(int index)
    {
        meshRenderer_ArmorHead.GetComponent<MeshFilter>().mesh = skinsData.Meshes_ArmorHead[index];
        OPB_LocalUserInfo.index_Mesh_ArmorHead = index;
    }
    
    public void SelectIndex_ArmorHead_Color(int index)
    {
        OPB_LocalUserInfo.index_Mesh_ArmorHead_Color = index;
        meshRenderer_ArmorHead.GetComponent<MeshRenderer>().material = skinsData.SkinMaterialColors[index];
    }
    
    public void SelectIndex_ArmorHips(int index)
    {
        OPB_LocalUserInfo.index_Mesh_ArmorHips = index;
        meshRenderer_ArmorHips.GetComponent<MeshFilter>().mesh = skinsData.Meshes_ArmorHips[index];
    }
    
    public void SelectIndex_ArmorHips_Color(int index)
    {
        OPB_LocalUserInfo.index_Mesh_ArmorHips_Color = index;
        meshRenderer_ArmorHips.GetComponent<MeshRenderer>().material = skinsData.SkinMaterialColors[index];
    }
    
    public void SelectIndex_BodySkin_Color(int index)
    {
        OPB_LocalUserInfo.index_BodySkin_Color = index;
        playerMeshRenderer.GetComponent<SkinnedMeshRenderer>().material = skinsData.SkinMaterialColors[index];
    }

    public void ApplyFromSkinString(string st)
    {
        skinString = st;
        RefreshAliveMaterial();
    }

    public void SetDead()
    {
        playerMeshRenderer.material = mat_Dead;
        meshRenderer_ArmorHead.material = mat_Dead;
        meshRenderer_ArmorHips.material = mat_Dead;
    }

    public void RefreshAliveMaterial()
    {
        playerMeshRenderer.material = mat_Alive;
        meshRenderer_ArmorHead.material = mat_Alive;
        meshRenderer_ArmorHips.material = mat_Alive;
        
        string[] explodedSt = skinString.Split('|');

        SelectIndex_ArmorHead(int.Parse(explodedSt[0]));
        SelectIndex_ArmorHead_Color(int.Parse(explodedSt[1]));
        SelectIndex_ArmorHips(int.Parse(explodedSt[2]));
        SelectIndex_ArmorHips_Color(int.Parse(explodedSt[3]));
        SelectIndex_BodySkin_Color(int.Parse(explodedSt[4])); 
    }
}
