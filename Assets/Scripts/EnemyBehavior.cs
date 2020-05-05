using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public Animator Animator;
    public Transform patrolRoute;
    public List<Transform> locations;
    public AudioClip EnemyDeathSound;
    public GameObject TextEnemyAggro;

    public int MaxLives = 3;
    public int Lives;

    private int _locationIndex;
    private NavMeshAgent _agent;
    private AudioHelper _audioHelper;

    public Transform Target;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _audioHelper = GameObject.Find("Audio Manager").GetComponent<AudioHelper>();

        Lives = MaxLives;

        InitializePatrolRoute();
        MoveToNextPatrolLocation();
    }

    void Update()
    {
        if (Target == null)
        {
            if (_agent.remainingDistance <= 0.2f)
            {
                MoveToNextPatrolLocation();
            }
            if (TextEnemyAggro.activeSelf)
            {
                TextEnemyAggro.SetActive(false);
            }
        }
        else
        {
            _agent.destination = Target.position;
            // Debug.DrawLine(transform.position, Target.position, Color.cyan);
            if (!TextEnemyAggro.activeSelf)
            {
                TextEnemyAggro.SetActive(true);
            }
        }

        Animator.SetBool("moving", _agent.velocity != Vector3.zero);
    }

    void InitializePatrolRoute()
    {
        foreach (Transform child in patrolRoute)
        {
            locations.Add(child);
        }
    }

    public void MoveToNextPatrolLocation()
    {
        if (locations.Count == 0)
            return;

        _agent.destination = locations[_locationIndex].position;

        _locationIndex = (_locationIndex + 1) % locations.Count;
    }

    public void OnShot()
    {
        Lives--;
        if (Lives <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        _audioHelper.PlaySound(EnemyDeathSound, 1.0f, 1.0f);
        Destroy(gameObject);
    }
}