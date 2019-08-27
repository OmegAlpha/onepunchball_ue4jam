using System;
using System.Collections;
using System.Collections.Generic;
using Bolt;
using UnityEngine;

public class OPB_KillerBall : Bolt.EntityEventListener<IOPB_KillerBallStated>
{

    [SerializeField]
    private GameObject serverLogicGameobject;
    
    [SerializeField]
    private GameObject meshGameObject;

    public OPB_PlayerController OwnerPlayer = null;

    public Vector3 MovementDirection;

    public int QtyRebounds = 0;

    public bool IsInHand;

    public MeshRenderer renderer;

    public Material matMine;
    public Material matOthers;

    private float linearSpeed = 30f;
    
    private Vector3 positionVectors = new Vector3();
    
    public void Initialize(OPB_PlayerController ownerP)
    {
        state.IsAlive = true;
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
        serverLogicGameobject.SetActive(false);
        meshGameObject.SetActive(true);
        
        state.AddCallback("OwnerPlayer", OnOwnerAssigned);
        state.AddCallback("InHand", OnInHandChanged);
        state.AddCallback("IsAlive", OnIsAliveChanged);
        
        state.SetTransforms(state.BallTransform, transform);

        if (!entity.IsOwner)
            gameObject.name = "Ball_Lejana";
    }

    private void OnIsAliveChanged()
    {
        meshGameObject.SetActive(state.IsAlive);
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
        if (!state.InHand)
        {
            positionVectors.x = transform.position.x;
            positionVectors.z = transform.position.z;
            positionVectors.y = 2.75f;
                
            transform.position = positionVectors + MovementDirection * linearSpeed * Time.deltaTime;
        }
    }
    
    // ONLY SERVER
    public void HitToPlayer(OPB_PlayerController player)
    {
        if(state.InHand || ! player.state.IsAlive || !state.IsAlive)
            return;
        
        if (player == OwnerPlayer)
        {
            // don't hit on yourself if Rebounds were 0 (for some self-safety)
            if(QtyRebounds <= 0)
                return;
            
            OnPlayerBallHit_Event evt = OnPlayerBallHit_Event.Create(player.entity, EntityTargets.Everyone);
            evt.SelfBall = true;
            evt.Send();

            OPB_OnPlayerScoreChanged evt2 = OPB_OnPlayerScoreChanged.Create(OwnerPlayer.entity, EntityTargets.OnlyOwner);
            evt2.IsIncrement = false;
            evt2.Send();
        }
        else
        {
            OnPlayerBallHit_Event evt = OnPlayerBallHit_Event.Create(player.entity, EntityTargets.Everyone);
            evt.SelfBall = false;
            evt.Send();
            
            OPB_OnPlayerScoreChanged evt2 = OPB_OnPlayerScoreChanged.Create(OwnerPlayer.entity, EntityTargets.OnlyOwner);
            evt2.IsIncrement = true;
            evt2.Send();
        }
        
        state.IsAlive = false;
        serverLogicGameobject.SetActive(false);
    }

    public void DestroyBall()
    {
        BoltNetwork.Destroy(gameObject);
    }

    private void Update()
    {
    }

    public void Shoot(Vector3 ShootDirection)
    {
        if (BoltNetwork.IsServer)
        {
            state.InHand = false;
            MovementDirection = ShootDirection;
            transform.position = new Vector3(OwnerPlayer.transform.position.x, 2.75f, OwnerPlayer.transform.position.z);
            serverLogicGameobject.SetActive(true);
        }
    }

    public void ReflectMovement(Vector3 reflectedVector)
    {
        MovementDirection = reflectedVector;
        QtyRebounds++;
    }
}
