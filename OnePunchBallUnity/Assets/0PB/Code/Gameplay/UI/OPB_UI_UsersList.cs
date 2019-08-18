using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPB_UI_UsersList : MonoBehaviour
{
    private const float TIME_UPDATE_USER_LIST = 1f;
    
    [SerializeField]
    private OPB_UI_PlayerRow prefabPlayerRow;
    
    [SerializeField]
    private Transform tContainerRows;

    private float timerUpdate = 0;

    private List<OPB_UI_PlayerRow> rows;
    
    // Start is called before the first frame update
    void Start()
    {
        rows = new List<OPB_UI_PlayerRow>();

        for (int i = 0; i < 16; i++)
        {
            OPB_UI_PlayerRow newRow = Instantiate(prefabPlayerRow, tContainerRows);
            rows.Add(newRow);
            
            newRow.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timerUpdate += Time.deltaTime;

        if (timerUpdate >= TIME_UPDATE_USER_LIST)
        {
            timerUpdate = 0;
            
            UpdateList();
            

        }

    }

    private void UpdateList()
    {
        List<OPB_PlayerController> playersSortedByScore = new List<OPB_PlayerController>(OPB_GlobalAccessors.ConnectedPlayers);
        
        playersSortedByScore.Sort((a, b) => { return b.state.Score.CompareTo(a.state.Score); });
        
        int i = 0;
        foreach (var player in playersSortedByScore)
        {
            rows[i].Initialize(player, i);
            rows[i].gameObject.SetActive(true);
            
            i++;
        }

        for (int j = i; j < rows.Count; j++)
        {
            rows[j].gameObject.SetActive(false);
        }
    }
}
