using System;
using UnityEditor;
using UnityEngine;

/**
 * Enemy Sphere of Influence
 */
public class EnemySOI : MonoBehaviour
{
    public EnemyBehavior EnemyBehavior;

    void OnTriggerEnter(Collider other)
    {
        var hitTag = other.tag;
        if (hitTag == "Player")
        {
            Debug.Log("Player entered sphere of influence");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var hitTag = other.tag;
        if (hitTag == "Player")
        {
            var dir = other.transform.position - transform.position;
            if (Physics.Raycast(transform.position, dir, out var hitInfo))
            {
                if (hitInfo.transform.CompareTag("Player"))
                {
                    // trigger player in danger
                    var pb = other.GetComponent<PlayerBehavior>();
                    if (pb.isInvincible) return;
                    pb.inDanger = true;
                    // set agent after player
                    EnemyBehavior.Target = other.transform;

                    Debug.DrawLine(transform.position, other.transform.position, Color.green);
                }
                else
                {
                    // trigger player safe
                    var pb = other.GetComponent<PlayerBehavior>();
                    pb.inDanger = false;
                    // set agent back to route
                    EnemyBehavior.Target = null;

                    Debug.DrawLine(transform.position, other.transform.position, Color.red);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        var hitTag = other.tag;
        if (hitTag == "Player")
        {
            Debug.Log("Player exited sphere of influence");

            // trigger player out of danger
            var pb = other.GetComponent<PlayerBehavior>();
            pb.inDanger = false;
            // resume patrol
            EnemyBehavior.Target = null;
        }
    }
}