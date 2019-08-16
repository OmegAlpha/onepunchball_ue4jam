using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BoltGlobalBehaviour]
public class OPB_NetworkCallbacks : Bolt.GlobalEventListener
{
    public override void SceneLoadLocalDone(string map)
    {
        
        
        Vector3 spawnPos = new Vector3(Random.Range(-8, 8), 0.6f, Random.Range(-8, 8));

        BoltNetwork.Instantiate(BoltPrefabs.APlayer, spawnPos, Quaternion.identity);

        if (BoltNetwork.IsServer)
        {
            Debug.Log("[Server] Instantiating Objects");
            BoltNetwork.Instantiate(BoltPrefabs.Entity_GameRules, Vector3.zero, Quaternion.identity);
        }
    }
}
