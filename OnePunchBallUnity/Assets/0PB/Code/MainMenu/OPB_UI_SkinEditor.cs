using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class OPB_UI_SkinEditor : MonoBehaviour
{
    [SerializeField]
    private OPB_UI_PlayerSkinModel skinModel;
    
    [SerializeField]
    private OPB_PlayerSkins_Data skinsData;
    
    [SerializeField]
    private Transform buttons_transform;
    
    [SerializeField]
    private GameObject prefab_categoryBtn;
    
    [SerializeField]
    private GameObject prefab_itemBtn;
    
    [SerializeField]
    private OPB_ColorSelection_Button prefab_ColorBtn;
    
    [SerializeField]
    private Transform categories_transform;
    
    [SerializeField]
    private string selected_part = "";
    
    [SerializeField]
    private string selected_category = "";
    
    public void DrawSlots(string name)
    {
        int i = 0;
        ClearList(buttons_transform.gameObject);

        if (name == "Colors")
        {
            foreach (Color skinColor in skinsData.ColorsPalette)
            {
                OPB_ColorSelection_Button color_btn = Instantiate(prefab_ColorBtn, buttons_transform);
                color_btn.Initialize(skinColor, i, OnClick_Selected_ColorIndex);
                i++;
            }
        }
        
        if(name == "Items")
        {
            List<Mesh> items = new List<Mesh>();
            if (selected_part == "Head") items = skinsData.Meshes_ArmorHead;
            if (selected_part == "Hips") items = skinsData.Meshes_ArmorHips;
            
            foreach(Mesh item in items)
            {
                GameObject btn = Instantiate(prefab_itemBtn, buttons_transform);
                btn.name = "Btn_Item_" + i;
                int index = i;
                btn.GetComponent<Button>().onClick.AddListener(() => OnClick_Selected_ItemIndex(index));
                i++;
            }
        }   
    }
    
    void CreateCategoryBtn(string name)
    {
        GameObject btn = Instantiate(prefab_categoryBtn, categories_transform);
        btn.name = "Btn_Category" + name;
        btn.GetComponent<TextMeshProUGUI>().text = name;
        btn.GetComponent<Button>().onClick.AddListener(() => OnClick_SetCategory(name));
    }

    public void OnClick_Selected_ItemIndex(int itemIndex)
    {
        if (selected_part == "Head") skinModel.SelectIndex_ArmorHead(itemIndex);
        if (selected_part == "Hips") skinModel.SelectIndex_ArmorHips(itemIndex);

        PlayerPrefs.SetInt("Mesh_Armor" + selected_part, itemIndex);
    }
    public void OnClick_Selected_ColorIndex(int colorIndex)
    {
        if (selected_part == "Head") skinModel.SelectIndex_ArmorHead_Color(colorIndex);
        if (selected_part == "Hips") skinModel.SelectIndex_ArmorHips_Color(colorIndex);
        if (selected_part == "Body") skinModel.SelectIndex_BodySkin_Color(colorIndex);

        PlayerPrefs.SetInt("Mesh_Armor"+ selected_part +"_Color", colorIndex);
    }
    
    public void OnClick_SetCategory(string name)
    {
        selected_category = name;
        if (name == "Colors") DrawSlots("Colors");
        if (name == "Type") DrawSlots("Items");
    }
    
    void ClearList(GameObject list)
    {
        foreach (Transform child in list.transform) { Destroy(child.gameObject); }
    }
    
    public void OnClick_SetPart(string name)
    {
        selected_part = name;
        GameObject.Find("Btn_" + selected_part).GetComponent<Button>().Select();
        ClearList(categories_transform.gameObject);
        
        if(name == "Body")
        {
            CreateCategoryBtn("Colors");
            OnClick_SetCategory("Colors");
        }
        else
        {
            OnClick_SetCategory("Type");
            CreateCategoryBtn("Type");
            CreateCategoryBtn("Colors");
        }
    }

    public void Start()
    {
        OnClick_SetPart("Head");
    }
}
