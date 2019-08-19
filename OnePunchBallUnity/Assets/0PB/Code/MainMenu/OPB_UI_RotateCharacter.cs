using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPB_UI_RotateCharacter : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    
    [SerializeField]
    private float speed;
    
    private Vector3 contact_point;
    
    private bool can_rotate = false;

    private int rotateDirection = 0;
    
    void Update()
    {
        rotateDirection = 0;
        
        DoMouseRotationCheck();
        DoJoystickRotationCheck();
        
        if (rotateDirection != 0)
        {
            transform.Rotate(0, rotateDirection * speed, 0, Space.Self);
        }
    }

    private void DoMouseRotationCheck()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.name == "PlayerMesh")
                {
                    contact_point = Input.mousePosition;
                    can_rotate = true;
                }
            }
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            can_rotate = false;
        }
        
        if (can_rotate)
        {
            if(Vector3.Distance(contact_point, Input.mousePosition) > 50)
            {
                rotateDirection = (contact_point.x - Input.mousePosition.x) > 0 ? 1 : -1;
            }
        }
    }

    private void DoJoystickRotationCheck()
    {
    }

}
