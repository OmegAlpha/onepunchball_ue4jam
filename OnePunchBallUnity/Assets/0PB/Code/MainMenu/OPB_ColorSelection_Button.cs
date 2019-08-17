using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OPB_ColorSelection_Button : MonoBehaviour
{
    public int Index;

    public Button myBtn;

    public Action<int> clickAction;
    
    public void Initialize(Color skinColor, int i, Action<int> onClickAction)
    {
        myBtn = GetComponent<Button>();
        
        GetComponent<Image>().color = skinColor;
        
        Index = i;
        clickAction = onClickAction;
        
        myBtn.onClick.AddListener(() =>
        {
            clickAction(Index);
        });
    }
}
