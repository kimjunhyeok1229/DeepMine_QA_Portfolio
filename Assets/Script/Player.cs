using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FocusDir { Left, Down, Right };

public class Player : MonoBehaviour
{
    // component
    Rigidbody2D _rigidbody = null;
    CapsuleCollider2D _collider = null;
    Animator _animator = null;
    [SerializeField] SpawnTile tileManager = null;
    [SerializeField] Tile focusedTile = null;
    [SerializeField] LayerMask tileLayer;

    // player var
    bool onGround = true;
    bool isMining = false;
    bool isInvincibility = false;
    bool isPushing = false;
    bool hasShield = false;
    float delay = 0f;
    float invincibilityTime = 0f;
    FocusDir focusDir;
    Vector2 center;
    Vector2 pushedDir;
    Vector2 pushedPos;

    // value
    Vector2 moveDir = Vector2.zero;
    [Header("Player Value")]
    [SerializeField] int hp = 3;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float jumpPower = 3f;
    [SerializeField] float gravityScale = 1f;
    [SerializeField] float miningDelay = 1f;
    [SerializeField] float invincibilityDelay = 1.5f;
    [SerializeField] float pushPower = 10f;
    [SerializeField] int miningPower = 1;

    // get set
    public Vector2 MoveDir { get { return moveDir; } set {  moveDir = value; if (moveDir.x != 0) transform.localScale = new Vector3(moveDir.x, 1, 1); } }
    public int Hp { get { return hp; } set { if (hasShield == true) { HasShield = false; return; } hp = Mathf.Clamp(value, 0, 3); UIManager.instance.HPUpdate(hp); } }
    public float GravityScale { get { return gravityScale; } }
    public Tile FocusTile { get { return focusedTile; } }
    public bool HasShield { get { return hasShield; } set { hasShield = value; UIManager.instance.ShieldUI.gameObject.SetActive(hasShield); } }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
        _rigidbody.gravityScale = gravityScale;
        center = new Vector2(0, 0.5f);
    }

    private void Update()
    {
        if(isMining == true && delay > 0f)
        {
            delay -= Time.deltaTime;
            if (delay <= 0f)
            {
                delay = 0f;
                isMining = false;
            }
        }

        if (isInvincibility == true && invincibilityTime > 0f)
        {
            invincibilityTime -= Time.deltaTime;
            if(invincibilityTime <= 0f)
            {
                invincibilityTime = 0f;
                isInvincibility = false;
            }
        }

        if (isPushing == true)
        {
            RaycastHit2D hit = Physics2D.Raycast(_rigidbody.position + center, pushedDir, 0.7f, tileLayer);
            if (hit.collider != null)
            {
                tileManager.EffectTile(hit.collider.GetComponent<Tile>(), true);
            }

            if (Mathf.Abs(Vector2.Distance(_rigidbody.position, pushedPos)) >= 3f || _rigidbody.velocity == Vector2.zero)
            {
                isPushing = false;
                //_rigidbody.velocity = Vector2.zero;
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            PushPlayer(new Vector2(10, 6));
        }

        GroundRay();
    }

    private void FixedUpdate()
    {
        if (isMining == true) return;

        Move(moveDir);
        //_rigidbody.position += moveDir * moveSpeed * Time.fixedDeltaTime;
    }

    #region Action
    void Move(Vector2 dir)
    {
        _rigidbody.position += dir * moveSpeed * Time.deltaTime;
        if (onGround == true)
        {
            if(dir == Vector2.zero)
                _animator.SetInteger("PlayerState", 0);
            else
                _animator.SetInteger("PlayerState", 1);
        }
    }
    public void Jump()
    {
        if (onGround == false || isMining == true) return;

        onGround = false;
        _rigidbody.velocity = jumpPower * Vector2.up;
        _animator.SetInteger("PlayerState", 2);
    }

    public void Mining()
    {
        if (onGround == false || isMining == true) return;

        if (focusDir == FocusDir.Down && focusedTile != null)
        {
            isMining = true;
            StartCoroutine(MoveToCenter(focusedTile.transform.position.x));
        }
        else
        {
            if(focusedTile != null)
                tileManager.Mining(focusedTile);
            _animator.SetTrigger("Mining");
            _animator.SetInteger("PlayerState", 0);
            delay += miningDelay;
            isMining = true;
            AudioManager.instance?.Play_Sfx(SFXList.Effect_Mining);
        }
    }

    IEnumerator MoveToCenter(float xPos)
    {
        Vector2 dir = (new Vector2(xPos - transform.position.x, 0)).normalized;
        float moveTime = Mathf.Abs(xPos - transform.position.x) / moveSpeed;
        while (true)
        {
            Move(dir);
            moveTime -= Time.deltaTime;
            if(moveTime <= 0)
            {                
                tileManager.Mining(focusedTile);
                _animator.SetTrigger("Mining");
                _animator.SetInteger("PlayerState", 0);
                delay += miningDelay;
                AudioManager.instance?.Play_Sfx(SFXList.Effect_Mining);
                yield break;
            }
            yield return null;
        }
    }

    public void Marking()
    {
        if (onGround == false || isMining == true || focusedTile == null) return;

        //focusedTile.Making();
    }

    public void Focusing(FocusDir dir)
    {
        if (isMining == true || onGround == false) return;

        if (focusedTile != null)
            UIManager.instance.FocusOff();
        focusedTile = null;
        focusDir = dir;

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        RaycastHit2D hit;

        switch (dir)
        {
            case FocusDir.Left:
                hit = Physics2D.Raycast(pos, Vector3.left, 0.7f, tileLayer);
                break;

            case FocusDir.Right:
                hit = Physics2D.Raycast(pos, Vector3.right, 0.7f, tileLayer);
                break;

            case FocusDir.Down:
                hit = Physics2D.Raycast(pos, Vector3.down, 0.7f, tileLayer);
                break;

            default:
                return;
        }

        if (hit.collider != null)
        {
            focusedTile = hit.transform.GetComponent<Tile>();
            UIManager.instance.moveFocusImage(Vector3Int.FloorToInt(hit.transform.position));
        }
    }

    public void FocusClear()
    {
        if (isMining == true || focusedTile == null) return;

        UIManager.instance.FocusOff();
        focusedTile = null;
    }
    #endregion

    public void GroundRay()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        bool check = Physics2D.Raycast(transform.position, Vector3.down, 0.1f, tileLayer);

        if (check == true && onGround == false)
        {
            onGround = true;
            _animator.SetInteger("PlayerState", 0);
        }
        else if (check == false && onGround == true)
        {
            onGround = false;
            isMining = false;
            FocusClear();
            _animator.SetInteger("PlayerState", 2);
        }
    }

    public void Damaged(int power = 1)
    {
        if (isInvincibility == true) return;

        Hp -= power;
        isInvincibility = true;
        invincibilityTime += invincibilityDelay;
        AudioManager.instance?.Play_Sfx(SFXList.Effect_Hit_Player);
    }

    public void PushPlayer(Vector2 pos)
    {
        FocusClear();
        isPushing = true;
        pushedPos = _rigidbody.position;
        pushedDir = (_rigidbody.position + center - pos).normalized;
        _rigidbody.velocity = pushedDir * pushPower * new Vector2(1, 1.5f);
    }
}
