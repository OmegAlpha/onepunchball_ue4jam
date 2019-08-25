using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bolt;
using UnityEngine;




public class OPB_GameRules : Bolt.EntityBehaviour<IOPB_GameRuleState>
{
    public static OPB_GameRules Instance;
    
    public static readonly UnityEventInt OnTimerTick = new UnityEventInt();

    private float timerSeconds = 0;

    private int localTimer = 0;

    private bool isMatchRunning = false;
    public bool IsMatchRunning => isMatchRunning;

    private bool isSetFinished = false;
    public bool IsSetFinished => isSetFinished;

    private float timerUpdateUserList = 0;
    
    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public override void Attached()
    {
        timerSeconds = 0;
        
        state.RoundTimer = 0;

        localTimer = 0;

        if (entity.IsOwner)
        {
            state.RoundDuration = 10;
            isMatchRunning = true;
        }

        OPB_Bolt_GlobalEventsListener.OnRoundStarted.AddListener(OnRoundStarted_Clients);
        OPB_Bolt_GlobalEventsListener.OnRoundFinished.AddListener(OnRoundFinished_Clients);
    }
   

    public override void SimulateOwner()
    {
        if (entity.IsOwner && BoltNetwork.IsServer && isMatchRunning)
        {
            timerSeconds += BoltNetwork.FrameDeltaTime;

            if (timerSeconds > 1f)
            {
                timerSeconds -= 1f;
                state.RoundTimer++;

                if (state.RoundTimer == state.RoundDuration + 1)
                {
                    state.RoundTimer = 0;

                    StartCoroutine(FinishRound_Routine());
                }
            }
        }
    }

    private IEnumerator FinishRound_Routine()
    {
        isMatchRunning = false;
        
        DestroyAllBalls();
        
        OPB_RoundFinished evt = OPB_RoundFinished.Create();
        evt.Send();

        yield return new WaitForSeconds(1.5f);
        
        StartRound();
    }

    private void DestroyAllBalls()
    {
        List<EntityBehaviour> allBalls = OPB_GlobalAccessors.GetAllEntitiesOfType(typeof(OPB_KillerBall)) ;

        while (allBalls.Count > 0)
        {
            EntityBehaviour ballToDestroy = allBalls[0];
            allBalls.RemoveAt(0);
            BoltNetwork.Destroy(ballToDestroy.gameObject);
        }
    }

    private void OnRoundStarted_Clients()
    {
        List<OPB_PlayerController> allPlayers = FindObjectsOfType<OPB_PlayerController>().ToList();
        
        allPlayers.ForEach(p => { p.StartRound(); });
    }
    
    private void OnRoundFinished_Clients()
    {
        List<OPB_PlayerController> allPlayers = FindObjectsOfType<OPB_PlayerController>().ToList();
        
        allPlayers.ForEach(p => { p.EndRound(); }); 
    }
    
    private void StartRound()
    {
        OPB_RoundStarted evt = OPB_RoundStarted.Create();
        evt.Send();

        isMatchRunning = true;
        isSetFinished = false;
    }

    private void Update()
    {
        if (localTimer != state.RoundTimer)
        {
            localTimer = state.RoundTimer;
            OnTimerTick.Invoke(state.RoundTimer);
        }


        if (!isSetFinished)
        {
            // Check Winners
            List<OPB_PlayerController> allPlayers = FindObjectsOfType<OPB_PlayerController>().ToList();
        
            for(int i = 0; i < allPlayers.Count; i++)
            {
                OPB_PlayerController p = allPlayers[i];
            
                if (p.state.Score >= OPB_MatchHostSettings.PointsToWinSet)
                {
                   StartCoroutine(FinishSet_Coroutine(p)); 
                   break;
                }
            }
        }
    }


    private IEnumerator FinishSet_Coroutine(OPB_PlayerController winner)
    {
        isSetFinished = true;
        
        DestroyAllBalls();
        
        OPB_SetWon evt = OPB_SetWon.Create();
        evt.Winner = winner.entity.NetworkId;
                
        evt.Send();
        
        yield return new WaitForSeconds(1.5f);
        
        isSetFinished = false;
        
        yield return new WaitForSeconds(1f);
        
        StartRound();
    }
    
    


}
