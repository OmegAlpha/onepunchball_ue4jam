using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPB_KillerBallReboundSensor : MonoBehaviour
{
    private OPB_KillerBall ball;
    
    void Start()
    {
        ball = GetComponentInParent<OPB_KillerBall>();
    }

    private void Update()
    {
        if(! BoltNetwork.IsServer)
            return;
        
        float sphereRadius = GetComponent<SphereCollider>().radius;
        
        int lowerAngle = 9999;
        Vector3 resultantReflection = Vector3.zero;

        Vector3 castOrigin = transform.position - ball.MovementDirection;
        
        for (int i = -11; i < 11; i++)
        {
            Quaternion spreadAngle = Quaternion.AngleAxis(i * 1, Vector3.up);
            Vector3 rayDirection = spreadAngle * ball.MovementDirection;
            Vector3 rayEndPos = castOrigin + rayDirection * sphereRadius;
            
            Debug.DrawLine(castOrigin, rayEndPos, Color.blue);
                
            if(i != 0)
                continue;
            
            if (Math.Abs(i) < lowerAngle)
            {

                RaycastHit[] hitInfos = Physics.RaycastAll(castOrigin, rayDirection, sphereRadius * 3f);
                
                for(int j = 0; j < hitInfos.Length; j++)
                {
                    RaycastHit hitInfo = hitInfos[j];
                    
                    
                    // TODO: use layer mask in the raycast intead of this shit
                    if(hitInfo.collider.gameObject == gameObject || hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("BallSensor") 
                                                                 || hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                        continue;

                    resultantReflection = Vector3.Reflect(rayDirection, hitInfo.normal).normalized;

                    lowerAngle = i;
                }
            }
        }

        if (lowerAngle != 9999)
        {
            ball.ReflectMovement(resultantReflection);
        }
        
        
    }
}
