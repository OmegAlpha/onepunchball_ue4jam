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

    void Update()
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
        if (Input.GetButtonUp("Fire1"))
        {
            can_rotate = false;
        }
        if (Input.GetButton("Fire1"))
        {
            if(Vector3.Distance(contact_point, Input.mousePosition) > 50 && can_rotate)
            {
                float dir = (contact_point.x - Input.mousePosition.x) > 0 ? 1 : -1;
                this.transform.Rotate(0, dir * speed, 0, Space.Self);
            }
        }
    }
}
