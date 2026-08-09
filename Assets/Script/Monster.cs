using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Rigidbody2D _rigidbody = null;
    Animator _animator = null;
    Player player = null;
    [SerializeField] LayerMask tileLayer;
    [SerializeField] List<RuntimeAnimatorController> animators = new List<RuntimeAnimatorController> ();

    [SerializeField] float moveSpeed = 1f;
    Vector2 moveDir = Vector2.right;
    float awakeDelay = 0.5f;
    float delay = 0f;

    public Vector2 MoveDir { get { return moveDir; } set { moveDir = value; if (moveDir.x != 0) transform.localScale = new Vector3(moveDir.x, 1, 1); } }

    public Player Player { get { return player; } set { player = value; } }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();        
    }

    private void OnEnable()
    {
        delay += awakeDelay;
        int type = Random.Range(0, 2);
        _animator.runtimeAnimatorController = animators[type];
    }

    private void Update()
    {
        if (delay > 0f)
        {
            delay -= Time.deltaTime;
            if (delay <= 0f)
            {
                delay = 0f;
                _animator.SetTrigger("Awake");
            }
        }

        if(transform.position.y > 12 ||  transform.position.y < -10) { gameObject.SetActive(false); }
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            Move();
        }
        else
        {
            Tracking();
        }
    }

    void Move()
    {
        _rigidbody.position += moveDir * moveSpeed * Time.fixedDeltaTime;
        Vector2 raypos = new Vector2(transform.position.x + moveDir.x * 0.5f, transform.position.y);
        if (Physics2D.Raycast(raypos, Vector2.down, 0.2f, tileLayer) == false || Physics2D.Raycast(transform.position, moveDir, 0.4f, tileLayer) == true)
        {
            MoveDir *= -1;
        }
    }

    void Tracking()
    {
        float xPos = player.transform.position.x - transform.position.x;
        MoveDir = new Vector2(xPos, 0).normalized;
        _rigidbody.position += moveDir * moveSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.GetComponent<Player>().Damaged();
    }
}
