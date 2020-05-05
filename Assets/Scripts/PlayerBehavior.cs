using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 75f;
    public float gravity = 8.0f;
    public float jumpVelocity = 5f;

    public GameObject bullet;
    public float bulletSpeed = 100f;

    public GameObject playerModel;
    public Animator playerModelAnimator;

    public GameObject invincibilityIndicator;
    public Animator invincibilityIndicatorAnimator;

    public int lives;
    public int maxLives = 3;
    public int items;
    public int maxItems = 4;

    private Vector3 _moveDir;
    private float _vInput;
    private float _hInput;

    public AudioClip PlayerDeathSound;

    public CharacterController characterController;
    public GameBehavior gameBehavior;
    private AudioHelper _audioHelper;

    public bool inDanger;
    public bool isInvincible;

    void Start()
    {
        lives = maxLives;
        items = 0;

        _audioHelper = GameObject.Find("Audio Manager").GetComponent<AudioHelper>();
    }

    private IEnumerator ResetInvincible()
    {
        yield return new WaitForSeconds(3);
        yield return new WaitUntil(() => !inDanger);
        isInvincible = false;
        invincibilityIndicatorAnimator.SetBool("active", false);
    }

    void Update()
    {
        _vInput = Input.GetAxis("Vertical") * moveSpeed;
        _hInput = Input.GetAxis("Horizontal") * rotateSpeed;

        playerModelAnimator.SetBool("is_walking", _vInput != 0f);

        if (characterController.isGrounded)
        {
            playerModelAnimator.SetBool("is_jumping", false);
            // Debug.Log("grounded");
            _moveDir = new Vector3(0, 0, _vInput);
            _moveDir = transform.TransformDirection(_moveDir);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _moveDir.y = jumpVelocity;
                playerModelAnimator.SetBool("is_jumping", true);
            }
            else
            {
                _moveDir.y = -0.5f;
            }
        }
        else
        {
            // Debug.LogWarning("not grounded");
            _moveDir = new Vector3(0, _moveDir.y, _vInput);
            _moveDir = transform.TransformDirection(_moveDir);
            _moveDir.y -= gravity * Time.deltaTime;
        }
        characterController.Move(_moveDir * Time.deltaTime);

        if (_hInput != 0)
        {
            Vector3 rotation = Vector3.up * _hInput;
            transform.Rotate(rotation * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0))
        {
            var newBullet = Instantiate(bullet, transform.position + (transform.forward * 1.5f), transform.rotation);
            var bulletRb = newBullet.GetComponent<Rigidbody>();
            bulletRb.velocity = transform.forward * bulletSpeed;


            var phq = Physics.queriesHitTriggers;
            ;
            var overlapResults = Physics.OverlapSphere(transform.position, 100.0f);
            foreach (var hit in overlapResults)
            {
                var enemy = hit.gameObject.FindParentWithTag("Enemy");
                if (enemy != null)
                {
                    var eb = enemy.GetComponent<EnemyBehavior>();
                    eb.Target = transform;
                    Debug.Log("Bullet sound triggered enemy!");
                }
            }
        }
    }

    public void OnLifeLost()
    {
        lives--;
        isInvincible = true;
        invincibilityIndicatorAnimator.SetBool("active", true);
        StartCoroutine(nameof(ResetInvincible));
        gameBehavior.OnLifeLost();
        if (lives <= 0)
        {
            _audioHelper.PlaySound(PlayerDeathSound, 1.0f, 1.0f);
            gameBehavior.OnGameLose();
        }
    }

    public void OnItemCollected()
    {
        items++;
        gameBehavior.OnItemCollected();
        if (items >= maxItems)
        {
            gameBehavior.OnLevelComplete();
        }
    }
}