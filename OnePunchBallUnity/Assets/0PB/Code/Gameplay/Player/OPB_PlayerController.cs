using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Bolt;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


public class OPB_PlayerController : Bolt.EntityEventListener<IOPB_PlayerState>
{
    public static OPB_PlayerController LocalInstance;
    
    [SerializeField]
    private GameObject charMesh;
    
    [SerializeField]
    private SkinnedMeshRenderer playerMeshRenderer;
    
    [SerializeField]
    private GameObject playerCamera;
    
    [SerializeField]
    private TextMeshPro txtPlayerName;

    [SerializeField]
    private CharacterController charController;

    [SerializeField]
    private GameObject prefabKillerBall;
    
    [SerializeField]
    private Transform tHandPosition;
    
    [SerializeField]
    private Material material_Common;
    
    [SerializeField]
    private Material material_Dead;

    public Transform THandPosition => tHandPosition;

    private OPB_KillerBall ballInHand;
    
    public override void Attached()
    {
        state.SetTransforms(state.PlayerTransform, transform);

        if (!entity.IsOwner)
        {
            gameObject.name = "player lejano";
            Destroy(playerCamera);
        }

        if (BoltNetwork.IsServer)
        {
            GiveNewBall();
        }
        
        if (entity.IsOwner)
        {
            LocalInstance = this;
            state.IsAlive = true;
            state.CanMove = true;
            state.UserName = GlobalEvents.USERNAME;
        }
        
        state.AddCallback("IsAlive", OnStateChanged_IsAlive);
    }

    private void OnStateChanged_IsAlive()
    {
        
    }

    // USED: SERVER / CLIENT
    public void AssignBallToHand(OPB_KillerBall ball)
    {
        ball.transform.SetParent(tHandPosition);
        ball.transform.localPosition = Vector3.zero;

        ballInHand = ball;
    }

    private void OnDestroy()
    {
        LocalInstance = null;
    }

    public override void SimulateOwner()
    {
        // TODO: put in some method or class for movement
        if (state.IsAlive && state.CanMove)
        {
            float speed = 12f;
            Vector3 movement = Vector3.zero;


            movement.z = Input.GetAxis("Vertical");
            movement.x = Input.GetAxis("Horizontal");
        
            movement = (movement.normalized * speed * BoltNetwork.FrameDeltaTime);
        
            if (movement != Vector3.zero)
            {
                // transform.position = transform.position + movement;
            }
        
            transform.position = new Vector3(transform.position.x, 0.6f, transform.position.z); 
        
            charMesh.transform.LookAt(transform.position + movement);
            charMesh.transform.eulerAngles = new Vector3(0, charMesh.transform.eulerAngles.y, 0);

            state.yRotation = charMesh.transform.eulerAngles.y;

            charController.Move(movement);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out hit))
            {
                OPB_OnPlayerShoot evt = OPB_OnPlayerShoot.Create(entity, EntityTargets.Everyone);

                Vector3 targetPos = hit.point;
                targetPos.y = 2.6f;

                evt.ShootDirection = targetPos;
                evt.Send();
            }
            
            
        }
        
    }

    private void Update()
    {
        //TODO: check how simulate proxy works instead of doing this
        if (!entity.IsOwner)
        {
            charMesh.transform.eulerAngles = new Vector3(0, state.yRotation, 0); 
        }
        
        playerMeshRenderer.material = state.IsAlive ? material_Common : material_Dead;
        charController.detectCollisions = state.IsAlive;
        
        txtPlayerName.text = state.Score.ToString() + "-" + state.UserName;
    }
    
    
    

    public void StartRound()
    {
        if (BoltNetwork.IsServer)
        {
            StartRound_Server();
        }

        if(entity.IsOwner)
            StartRound_ClientOwner();
        else
            StartRound_ClientNotOwner();
    }

    private void StartRound_Server()
    {
        GiveNewBall();
    }

    private void StartRound_ClientOwner()
    {
        state.IsAlive = true;
        state.CanMove = true;
    }
    
    private void StartRound_ClientNotOwner()
    {
        
    }


    private void GiveNewBall()
    {
        if (BoltNetwork.IsServer)
        {
            GameObject newBall = BoltNetwork.Instantiate(prefabKillerBall, tHandPosition.position,
               Quaternion.identity);
            
            ballInHand = newBall.GetComponent<OPB_KillerBall>();
            ballInHand.Initialize(this);
            
        }
    }

    // ONLY OWNER 
    public override void OnEvent(OnPlayerBallHit_Event evt)
    {
        state.IsAlive = false;
        state.CanMove = false;
    }

    public override void OnEvent(OPB_OnPlayerScoreChanged evt)
    {
        state.Score += evt.IsIncrement ? +1 : -1;
    }

    // ONLY SERVER 
    public override void OnEvent(OPB_OnPlayerShoot evt)
    {
        if (!BoltNetwork.IsServer)
            return;
        
        if (ballInHand != null)
        {
            Vector3 targetPos = evt.ShootDirection;
            targetPos.y = transform.position.y;
            
            Vector3 ShootDirecton = (targetPos - transform.position).normalized;
            ballInHand.Shoot(ShootDirecton);
        }
    }

}
