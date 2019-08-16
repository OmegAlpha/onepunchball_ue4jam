using System;
using System.Collections.Generic;
using Bolt;
using Bolt.Matchmaking;
using UdpKit;
using UnityEngine;
using UnityEngine.Events;

public class EventBoltSessionsReceived : UnityEvent<List<UdpSession>>
{
}

[BoltGlobalBehaviour]
public class OPB_Bolt_GlobalEventsListener : Bolt.GlobalEventListener
{
    public static readonly EventBoltSessionsReceived OnSessionsReceived = new EventBoltSessionsReceived();
    public static readonly UnityEvent OnRoundFinished = new UnityEvent();
    public static readonly UnityEvent OnRoundStarted = new UnityEvent();
    
    public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer)
        {
            string sessionID = "TEST_MATCH_NAME";

            BoltMatchmaking.CreateSession(sessionID);
            BoltNetwork.LoadScene("Gameplay");
            
            
        }
    }

    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
    {
        List<UdpSession> sessions = new List<UdpSession>();
        
        foreach (var session in sessionList)
        {
            UdpSession photonSession = session.Value as UdpSession;

            sessions.Add(photonSession);
        }
        
        OnSessionsReceived.Invoke(sessions);
        
    }
    
    
    // Event ROUND FINISHED
    public override void OnEvent(OPB_RoundFinished evt)
    {
        OnRoundFinished.Invoke();   
    }
    
    // Event ROUND STARTED
    public override void OnEvent(OPB_RoundStarted evt)
    {
        OnRoundStarted.Invoke();
    }
    
    
    public override void EntityAttached(BoltEntity entity)
    {
        Bolt.EntityBehaviour entityBeh = entity.gameObject.GetComponent<Bolt.EntityBehaviour>();
        
        if (BoltNetwork.IsServer)
        {
            if (entityBeh != null)
            {
                OPB_GlobalAccessors.AddEntity(entityBeh.GetType(), entityBeh);
            }
        }
    }

    public override void EntityDetached(BoltEntity entity)
    {
        Bolt.EntityBehaviour entityBeh = entity.gameObject.GetComponent<Bolt.EntityBehaviour>();
        
        if (BoltNetwork.IsServer)
        {
            if (entityBeh != null)
            {
                OPB_GlobalAccessors.RemoveEntity(entityBeh.GetType(), entityBeh);
            }
        } 
    }
}
