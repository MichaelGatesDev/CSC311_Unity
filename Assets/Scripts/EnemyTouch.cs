using System;
using UnityEngine;

public class EnemyTouch : MonoBehaviour
{
    public EnemyBehavior EnemyBehavior;

    void OnTriggerEnter(Collider other)
    {
        var hitTag = other.tag;
        // touched player
        if (hitTag == "Player")
        {
            Debug.Log("Player touched me!");
            // trigger player in danger
            var pb = other.GetComponent<PlayerBehavior>();
            if (!pb.isInvincible)
            {
                pb.OnLifeLost();
            }

            pb.inDanger = false;
            // resume patrol
            EnemyBehavior.MoveToNextPatrolLocation();
            EnemyBehavior.Target = null;
        }
        // touched projectile
        else if (hitTag == "Projectile")
        {
            Debug.Log("Projectile touched me!");
            // destroy bullet immediately
            Destroy(other.gameObject);
            EnemyBehavior.OnShot();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var hitTag = other.tag;
        if (hitTag == "Player")
        {
            Debug.Log(other.name + " exited touch");
        }
    }
}