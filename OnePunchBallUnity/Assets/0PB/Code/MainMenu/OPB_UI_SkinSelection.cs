using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OPB_UI_SkinSelection : MonoBehaviour
{
    [SerializeField]
    private OPB_PlayerSkins_Data skinsData;
    
    [SerializeField]
    private OPB_UI_PlayerSkinModel skinModel;
    
    [SerializeField]
    private OPB_ColorSelection_Button prefab_ColorSelectBtn;
    
    [SerializeField]
    private Transform tContainer_ColorsHead;
    
    [SerializeField]
    private Transform tContainer_ColorsHips;
    
    [SerializeField]
    private Transform tContainer_ColorsBodySkin;

    
    
    private void Start()
    {
        OPB_LocalUserInfo.index_Mesh_ArmorHead = PlayerPrefs.GetInt("Mesh_ArmorHead", 0);
        OPB_LocalUserInfo.index_Mesh_ArmorHead_Color = PlayerPrefs.GetInt("Mesh_ArmorHead_Color", 0); 
        
        OPB_LocalUserInfo.index_Mesh_ArmorHips = PlayerPrefs.GetInt("Mesh_ArmorHips", 0);
        OPB_LocalUserInfo.index_Mesh_ArmorHips_Color = PlayerPrefs.GetInt("Mesh_ArmorHips_Color", 0);
        
        OPB_LocalUserInfo.index_BodySkin_Color = PlayerPrefs.GetInt("BodySkin_Color", 0);
        
        skinModel.SelectIndex_ArmorHead(OPB_LocalUserInfo.index_Mesh_ArmorHead);
        skinModel.SelectIndex_ArmorHead_Color(OPB_LocalUserInfo.index_Mesh_ArmorHead_Color);
        
        skinModel.SelectIndex_ArmorHips(OPB_LocalUserInfo.index_Mesh_ArmorHips);
        skinModel.SelectIndex_ArmorHips_Color(OPB_LocalUserInfo.index_Mesh_ArmorHips_Color);
        
        skinModel.SelectIndex_BodySkin_Color(OPB_LocalUserInfo.index_BodySkin_Color);
        
        int i = 0;
        foreach (Color skinColor in skinsData.ColorsPalette)
        {
            OPB_ColorSelection_Button headColorBtn = Instantiate(prefab_ColorSelectBtn, tContainer_ColorsHead);
            headColorBtn.Initialize(skinColor, i, OnClick_Mesh_ArmorHead_ColorIndex);
            i++;
        }
        
        
        i = 0;
        foreach (Color skinColor in skinsData.ColorsPalette)
        {
            OPB_ColorSelection_Button hipsColorBtn = Instantiate(prefab_ColorSelectBtn, tContainer_ColorsHips);
            hipsColorBtn.Initialize(skinColor, i, OnClick_Mesh_ArmorHips_ColorIndex);
            i++;
        }
        
        
        i = 0;
        foreach (Color skinColor in skinsData.ColorsPalette)
        {
            OPB_ColorSelection_Button bodySkinColor_Btn = Instantiate(prefab_ColorSelectBtn, tContainer_ColorsBodySkin);
            bodySkinColor_Btn.Initialize(skinColor, i, OnClick_BodyColor_Index);
            i++;
        }
    }
    
    public void OnClick_Mesh_ArmorHead_Prev()
    {
        OPB_LocalUserInfo.index_Mesh_ArmorHead--;
        if (OPB_LocalUserInfo.index_Mesh_ArmorHead < 0)
            OPB_LocalUserInfo.index_Mesh_ArmorHead = skinsData.Meshes_ArmorHead.Count - 1;
        
        skinModel.SelectIndex_ArmorHead(OPB_LocalUserInfo.index_Mesh_ArmorHead);
        PlayerPrefs.SetInt("Mesh_ArmorHead", OPB_LocalUserInfo.index_Mesh_ArmorHead);
    }
    
    public void OnClick_Mesh_ArmorHead_Next()
    {
        OPB_LocalUserInfo.index_Mesh_ArmorHead++;
        if (OPB_LocalUserInfo.index_Mesh_ArmorHead > skinsData.Meshes_ArmorHead.Count - 1)
            OPB_LocalUserInfo.index_Mesh_ArmorHead = 0;
        
        skinModel.SelectIndex_ArmorHead(OPB_LocalUserInfo.index_Mesh_ArmorHead);
        PlayerPrefs.SetInt("Mesh_ArmorHead", OPB_LocalUserInfo.index_Mesh_ArmorHead);
    }
    
    public void OnClick_Mesh_ArmorHead_ColorIndex(int colorIndex)
    {
        skinModel.SelectIndex_ArmorHead_Color(colorIndex);
        PlayerPrefs.SetInt("Mesh_ArmorHead_Color", colorIndex);
    }
    
    public void OnClick_Mesh_ArmorHips_Next()
    {
        OPB_LocalUserInfo.index_Mesh_ArmorHips++;
        if (OPB_LocalUserInfo.index_Mesh_ArmorHips > skinsData.Meshes_ArmorHips.Count - 1)
            OPB_LocalUserInfo.index_Mesh_ArmorHips = 0;
        
        skinModel.SelectIndex_ArmorHips(OPB_LocalUserInfo.index_Mesh_ArmorHips);
        PlayerPrefs.SetInt("Mesh_ArmorHips", OPB_LocalUserInfo.index_Mesh_ArmorHips);
    }
    
    public void OnClick_Mesh_ArmorHips_Prev()
    {
        OPB_LocalUserInfo.index_Mesh_ArmorHips--;
        if (OPB_LocalUserInfo.index_Mesh_ArmorHips < 0)
            OPB_LocalUserInfo.index_Mesh_ArmorHips = skinsData.Meshes_ArmorHips.Count - 1;

        skinModel.SelectIndex_ArmorHips(OPB_LocalUserInfo.index_Mesh_ArmorHips);
        PlayerPrefs.SetInt("Mesh_ArmorHips", OPB_LocalUserInfo.index_Mesh_ArmorHips);
    }
    
    public void OnClick_Mesh_ArmorHips_ColorIndex(int colorIndex)
    {
        skinModel.SelectIndex_ArmorHips_Color(colorIndex);
        PlayerPrefs.SetInt("Mesh_ArmorHips_Color", colorIndex);
    }
    
    public void OnClick_BodyColor_Index(int colorIndex)
    {
        skinModel.SelectIndex_BodySkin_Color(colorIndex);
        PlayerPrefs.SetInt("BodySkin_Color", colorIndex);
    }
}
