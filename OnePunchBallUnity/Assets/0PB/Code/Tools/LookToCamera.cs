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
        
        Vector3 direction = (lookAt.transform.position - transform.position).normalized; 
        
        transform.LookAt( transform.position - direction );
    }
}
