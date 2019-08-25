
using System.Collections;
using System.Linq;
using Bolt;
using DG.Tweening;
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
    private OPB_UI_PlayerSkinModel skinModel;
    
    [SerializeField]
    private GameObject playerCameraObject;

    private Camera playerCamera;
    
    [SerializeField]
    private Transform tCameraWinnerPointer;
    public Transform TCameraWinnerPointer => tCameraWinnerPointer;
    
    [SerializeField]
    private TextMeshPro txtPlayerName;

    [SerializeField]
    private CharacterController charController;

    [SerializeField]
    private GameObject prefabKillerBall;
    
    [SerializeField]
    private Transform tHandPosition;
    
    [SerializeField]
    private LayerMask maskMouseInputRay;

    public Transform THandPosition => tHandPosition;

    private OPB_KillerBall ballInHand;

    public bool Debug_OverrideCameraPosition;

    private bool inputIsEnabled = true;
    
    
    
    public override void Attached()
    {
        OPB_Bolt_GlobalEventsListener.OnSetFinished.AddListener(OnSetFinished);
        
        playerCamera = playerCameraObject.GetComponent<Camera>();
        
        tCameraWinnerPointer.SetParent(charMesh.transform);
        
        state.SetTransforms(state.PlayerTransform, transform);
        
        OPB_GlobalAccessors.ConnectedPlayers.Add(this);

        if (!entity.IsOwner)
        {
            gameObject.name = "player lejano";
            Destroy(playerCameraObject);
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
            state.UserName = OPB_LocalUserInfo.UserName;
            state.SkinString = OPB_LocalUserInfo.GetSkinString();
            
            OPB_UI_ChatPanel.Get().Initialize();
        }

        if (!entity.IsOwner && !BoltNetwork.IsServer)
        {
            Destroy(charController);
        }

        state.AddCallback("IsAlive", OnStateChanged_IsAlive);
        state.AddCallback("SkinString", OnStateChange_SkinString);
    }



    private void OnStateChange_SkinString()
    {
        skinModel.ApplyFromSkinString(state.SkinString);
    }

    private void OnStateChanged_IsAlive()
    {
        
    }

    public void ToggleInputEnabled(bool isEnabled)
    {
        inputIsEnabled = isEnabled;
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
        
        OPB_GlobalAccessors.ConnectedPlayers.Remove(this);
    }

    public override void SimulateOwner()
    {
        // TODO: put in some method or class for movement
        if (state.IsAlive && state.CanMove)
        {
            float speed = 12f;
            Vector3 movement = Vector3.zero;


            // TODO: MOVE this to input control

            if (inputIsEnabled)
            {
                movement.z = Input.GetAxis("Vertical");
                movement.x = Input.GetAxis("Horizontal");
                movement = (movement.normalized * speed * BoltNetwork.FrameDeltaTime);    
            }

            if (movement.magnitude > 0)
            {
                transform.position = new Vector3(transform.position.x, 0.6f, transform.position.z);

                charMesh.transform.LookAt(transform.position + movement);
                charMesh.transform.eulerAngles = new Vector3(0, charMesh.transform.eulerAngles.y, 0);

                float yRotation = charMesh.transform.eulerAngles.y;
                if (yRotation < 0)
                    yRotation += 360f;

                yRotation /= 360f;
                
                state.yRotation =  yRotation;
                charController.Move(movement);
            }

            if (BoltNetwork.IsClient)
            {
                state.ServerPing = BoltNetwork.Server.PingAliased;
            }
        }
        
        
    }


    private void Update()
    {
//
//        if (Input.GetKeyDown(KeyCode.O))
//        {
//            StartCoroutine(ShowWinnerCamera(entity.NetworkId));
//        }

        //TODO: check how simulate proxy works instead of doing this
        if (!entity.IsOwner)
        {
            charMesh.transform.eulerAngles = new Vector3(0, state.yRotation * 360f, 0); 
        }
        else
        {
            // TODO: move this to input control
            if (Input.GetMouseButtonDown(0) && inputIsEnabled)
            {
                RaycastHit hit;
                Ray ray = playerCameraObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        
                if (Physics.Raycast(ray, out hit, maskMouseInputRay ))
                {
                    OPB_OnPlayerShoot evt = OPB_OnPlayerShoot.Create(entity, EntityTargets.Everyone);

                    Vector3 targetPos = hit.point;
                    targetPos.y = 2.6f;

                    Debug.Log("[Simulater Owner] RayCast Worked and Hit Detected. Send Event");
                
                    evt.ShootDirection = targetPos;
                    evt.Send();
                }
            }
        }
        
        if(state.IsAlive)
            skinModel.RefreshAliveMaterial();
        else
        {
            skinModel.SetDead();
        }

        if (BoltNetwork.IsServer || entity.IsOwner)
        {
            charController.detectCollisions = state.IsAlive;
        }
        
        txtPlayerName.text = state.Score.ToString() + "-" + state.UserName;

        /*  --- JUST FOR TESTING THE CAMERA, BUT IT BREAKS THE WINNER CAMERA --------
        if (BoltNetwork.IsServer)
        {
            if (OPB_CameraSettings.Instance != null && entity.IsOwner)
            {
                OPB_CameraSettings.Instance.state.FOV = playerCamera.fieldOfView;
                OPB_CameraSettings.Instance.state.LocalPos = playerCamera.transform.localPosition;
                OPB_CameraSettings.Instance.state.LocalEuler = playerCamera.transform.localEulerAngles;
            }
        }
        else
        {
            if (OPB_CameraSettings.Instance != null && entity.IsOwner)
            {
                playerCamera.fieldOfView = OPB_CameraSettings.Instance.state.FOV;
                playerCamera.transform.localPosition = OPB_CameraSettings.Instance.state.LocalPos;
                playerCamera.transform.localEulerAngles = OPB_CameraSettings.Instance.state.LocalEuler;
            }
        }*/
    }

    
    
    private IEnumerator ShowWinnerCamera(NetworkId winnerID)
    {
        if (entity.IsOwner)
        {
            state.CanMove = false;
        }

        Transform targetTransform = OPB_GlobalAccessors.ConnectedPlayers.Find(p => p.entity.NetworkId == winnerID).TCameraWinnerPointer;
        
        Vector3 originalLocalPosition = playerCamera.transform.localPosition.GetClone();
        Vector3 originalLocalEulers = playerCamera.transform.localEulerAngles.GetClone();
        
        
        playerCamera.transform.DOMove(targetTransform.position, 1f);
        playerCamera.transform.DORotate(targetTransform.eulerAngles, 1f);
        
        yield return new WaitForSeconds(0.5f);
        
        OPB_SoundsManager.Get().PlaySFX_Winner();
        
        yield return new WaitForSeconds(2.5f);
        
        playerCamera.transform.DOLocalMove(originalLocalPosition, 0.5f);
        playerCamera.transform.DOLocalRotate(originalLocalEulers, 0.5f);
        
        yield return new WaitForSeconds(1.5f);
        
        if (entity.IsOwner)
        {
            state.CanMove = true;
        }
    }
    
    
    private void OnSetFinished(NetworkId winnerID)
    {
        if (entity.IsOwner)
        {
            StartCoroutine(ShowWinnerCamera(winnerID));
            
            ToggleInputEnabled(false);
        }

        state.Score = 0;

        if (entity.NetworkId == winnerID)
        {
            state.SetsWon++;
        }
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
    
    public void EndRound()
    {
        if (entity.IsOwner)
        {
            state.IsAlive = true;
            state.CanMove = true;
        }
    }

    private void StartRound_Server()
    {
        GiveNewBall();
    }

    private void StartRound_ClientOwner()
    {
        state.IsAlive = true;
        state.CanMove = true;
        ToggleInputEnabled(true);
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
        
        OPB_SoundsManager.Get().PlaySFX_Death();
    }

    public override void OnEvent(OPB_OnPlayerScoreChanged evt)
    {
        state.Score += evt.IsIncrement ? +1 : -1;
        
        if(evt.IsIncrement)
            OPB_SoundsManager.Get().PlaySFX_Point();
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

            ballInHand = null;
            
            OPB_SoundsManager.Get().PlaySFX_Woosh();
        }
    }

}
