using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OPB_UI_InMatchHUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txtTimer;
    
    [SerializeField]
    private TextMeshProUGUI txtNewBall;
    
    [SerializeField]
    private TextMeshProUGUI txtMakeItCount;
    
    void Start()
    {
        txtNewBall.gameObject.SetActive(false);
        txtMakeItCount.gameObject.SetActive(false);
        
        txtTimer.text = "0";
        OPB_GameRules.OnTimerTick.AddListener(OnTimerTick);
        
        OPB_Bolt_GlobalEventsListener.OnRoundStarted.AddListener(OnRoundStarted);
        OPB_Bolt_GlobalEventsListener.OnRoundStarted.AddListener(OnRoundFinished);
        
    }

    private void OnRoundStarted()
    {
        
    }

    private void OnRoundFinished()
    {
        StartCoroutine(OnRoundFinished_Routine());
    }

    private IEnumerator OnRoundFinished_Routine()
    {
        yield return new WaitForSeconds(0.2f);
        
        OPB_SoundsManager.Get().PlaySFX_NewBall();
        
        txtNewBall.gameObject.SetActive(true);
        txtTimer.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(1.3f);
        
        OPB_SoundsManager.Get().PlaySFX_MakeItcount();
        
        txtNewBall.gameObject.SetActive(false);
        txtMakeItCount.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(1.2f);
        txtMakeItCount.gameObject.SetActive(false);
    }

    private void OnTimerTick(int time)
    {
        txtTimer.text = time.ToString();
    }


    void Update()
    {
        
    }
}
