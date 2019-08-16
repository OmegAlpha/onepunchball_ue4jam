using System;
using System.Collections;
using System.Collections.Generic;
using Bolt;
using UnityEngine;

public class OPB_KillerBall : Bolt.EntityEventListener<IOPB_KillerBallStated>
{
    public NetworkId idOwner = new NetworkId();

    public OPB_PlayerController OwnerPlayer = null;

    public Vector3 MovementDirection;

    public int QtyRebounds = 0;

    public bool IsInHand;

    public MeshRenderer renderer;

    public Material matMine;
    public Material matOthers;
    
    public void Initialize(OPB_PlayerController ownerP)
    {
        state.OwnerPlayer  = ownerP.entity.NetworkId;
        OwnerPlayer = ownerP;
        OwnerPlayer.AssignBallToHand(this);

        if (entity.IsOwner)
        {
            state.InHand = true;
        }

        renderer.material = ownerP.entity.IsOwner ? matMine : matOthers;
    }


    public override void Attached()
    {
        state.AddCallback("OwnerPlayer", OnOwnerAssigned);
        state.AddCallback("InHand", OnInHandChanged);

        if (!entity.IsOwner)
            gameObject.name = "Ball_Lejana";
    }

    private void OnInHandChanged()
    {
        IsInHand = state.InHand;
        
        if(!state.InHand)
        {
            transform.SetParent(null);
        }
    }

    private void OnOwnerAssigned()
    {
        BoltEntity myOwnerEntity = BoltNetwork.FindEntity(state.OwnerPlayer);
        OPB_PlayerController owner = myOwnerEntity.GetComponent<OPB_PlayerController>();
        Initialize(owner);
    }

    public override void SimulateOwner()
    {
        if (BoltNetwork.IsServer)
        {
            if (!state.InHand)
            {
                state.BallTransform = transform.position + MovementDirection * 25f * Time.deltaTime;
            }
        }
    }

    public override void SimulateController()
    {

    }

    
    // ONLY SERVER
    public void HitToPlayer(OPB_PlayerController player)
    {
        if(state.InHand)
            return;
        
        if (player == OwnerPlayer)
        {
            if (QtyRebounds > 0)
            {
                OnPlayerBallHit_Event evt = OnPlayerBallHit_Event.Create(player.entity, EntityTargets.Everyone);
                evt.SelfBall = true;
                evt.Send();

                OPB_OnPlayerScoreChanged evt2 = OPB_OnPlayerScoreChanged.Create(OwnerPlayer.entity, EntityTargets.OnlyOwner);
                evt2.IsIncrement = false;
                evt2.Send();
                
                
                BoltNetwork.Destroy(gameObject);
            }
        }
        else
        {
            OnPlayerBallHit_Event evt = OnPlayerBallHit_Event.Create(player.entity, EntityTargets.Everyone);
            evt.SelfBall = false;
            evt.Send();
            
            OPB_OnPlayerScoreChanged evt2 = OPB_OnPlayerScoreChanged.Create(OwnerPlayer.entity, EntityTargets.OnlyOwner);
            evt2.IsIncrement = true;
            evt2.Send();
            
            BoltNetwork.Destroy(gameObject);
        }
        
        
    }

    private void Update()
    {
        if (!state.InHand)
        {
            transform.position = state.BallTransform;
        }
    }

    public void Shoot(Vector3 ShootDirection)
    {
        if (BoltNetwork.IsServer)
        {
            entity.TakeControl();
            state.InHand = false;
            MovementDirection = ShootDirection;
            state.BallTransform = new Vector3(transform.position.x, 2.6f, transform.position.z);
        }
    }

    public void ReflectMovement(Vector3 reflectedVector)
    {
        MovementDirection = reflectedVector;
        QtyRebounds++;
    }
}
