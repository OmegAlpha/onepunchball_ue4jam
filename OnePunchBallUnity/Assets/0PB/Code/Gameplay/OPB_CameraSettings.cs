using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPB_CameraSettings : Bolt.EntityBehaviour<IOPB_PlayerCamera_State>
{
    public static OPB_CameraSettings Instance;

    public override void Attached()
    {
        Instance = this;
    }
}
