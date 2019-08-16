using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyRendererer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Destroy(renderer);
    }

}
