using UnityEngine;

public class OPB_KillerBallSensor : MonoBehaviour
{
    private OPB_KillerBall ball;
    
    void Start()
    {
        ball = GetComponentInParent<OPB_KillerBall>();
    }

    private void OnTriggerEnter(Collider other)
    {
        OPB_PlayerController player = other.GetComponent<OPB_PlayerController>();

        if (player != null && ball != null)
        {
            ball.HitToPlayer(player);
        }
    }
}
