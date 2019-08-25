using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToCamera : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    // Update is called once per frame
    void Update()
    {
        Camera lookAt = Camera.main;
        transform.rotation = Quaternion.LookRotation(transform.position - lookAt.transform.position );
        
        if(!OPB_GameRules.Instance.IsSetFinished)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z); 
    }
}
