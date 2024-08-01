using StarterAssets;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Component;

public class PlayerController : MonoSingleton<PlayerController>
{
    #region Components
    private Rigidbody2D _rb;
    private BoxCollider2D _playerCollider;
    private InputTesting _input;
    
    
    #endregion

    private float jumpForce;
    private float gravityStrength;
    private float gravityScale;

    public bool CoyoteTimeOpen { get; private set; }
    public bool DoubleJumpOpen { get; private set; }

    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _buildingLayerMask;

    public float coyoteTime;
    private bool isJumping;
    private bool canJump;

    private Vector2 _moveInput;

    [Header("Gravity Settings")]
    public float jumpHeight;
    public float jumpTimeToApex;
    public float fastFallGravityMult;
    public float jumpCutGravityMult;
    public float fallGravityMult;
    public float maxFastFallSpeed;
    public float maxFallSpeed;
    public float jumpInputBufferTime;
    public float LastPressedJumpTime;
    private float LastOnGroundTime;

    //Jump
    private bool _isJumpCut;
    private bool _isJumpFalling;

    private bool bulshitTesting;

    [Header("Custom Jump Testing Stuffs")]
    //public GameObject heightObject;
    private bool heightObjectTest;
    private float previousY;
    private float jumpTimer;
    //public TextMeshProUGUI text;
    private bool oneTimeTest;


    private bool canShot;
    public bool CanShot { get { return canShot; } set { canShot = value; } }

    private Animator _animator;

    private bool jumpTesting = false;

    private float _speed;
    public float Speed { private get { return _speed; } set {  _speed = value; } }

    public Button _pausebButton;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<InputTesting>();
        _playerCollider = GetComponent<BoxCollider2D>();
        
    }

    private void Start()
    {
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
        gravityScale = gravityStrength / Physics2D.gravity.y;
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && Time.timeScale >= 0.9f)
        {
            _pausebButton.onClick.Invoke();
        }

        Debug.Log($"Time scale is {Time.timeScale}");

        Fire();

        if (isJumping)
        {
            oneTimeTest = true;
            if (previousY < transform.position.y && heightObjectTest)
            {
                //heightObject.transform.position = transform.position;
                previousY = transform.position.y;
                jumpTimer += Time.deltaTime;
            }
        }
        else
        {
            if(oneTimeTest)
            {
                previousY -= 26;
                //text.text = $"Jump time: {jumpTimer - (jumpTimer % 0.01f)}s\n" +
                //    $"jump height: {previousY - (previousY % 0.01f)}";
                //Debug.Log(jumpTimer);
                oneTimeTest = false;
            }
            previousY = 0;
            jumpTimer = 0;
        }
        
        

        
        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;
        #endregion

        //if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.J))
        //{
        //    OnJumpInput();
        //    bulshitTesting = true;
        //    heightObjectTest = true;
        //}

        //if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.J))
        //{
        //    OnJumpUpInput();
        //}


        if(_input.jump)
        {
            if(!jumpTesting)
            {
                OnJumpInput();
                bulshitTesting = true;
                heightObjectTest = true;
                jumpTesting = true;
            }
        }
        else
        {
            if (jumpTesting)
            {
                OnJumpUpInput();
                jumpTesting = false;
            }
        }

        #region COLLISION CHECKS
        if (!isJumping)
        {
            //Ground Check
            if (TestOnGround()) //checks if set box overlaps with ground
            {
                LastOnGroundTime = coyoteTime; //if so sets the lastGrounded to coyoteTime
            }
        }
        #endregion
        #region JUMP CHECKS
        if(CanJump())
        {
            _animator.SetTrigger("Run Again");
        }
        if (isJumping && _rb.velocity.y < 0)
        {
            isJumping = false;

            _isJumpFalling = true;
            if (!TestCeiling())
                _animator.SetTrigger("Fall");
        }

        if (LastOnGroundTime > 0 && !isJumping)
        {
            _isJumpCut = false;

            _isJumpFalling = false;
                
        }

       
        //Jump
        if (CanJump() && LastPressedJumpTime > 0)
        {
            isJumping = true;
            _isJumpCut = false;
            _isJumpFalling = false;
            if (!TestCeiling())
                _animator.SetTrigger("Jump");


            Jump();
        }
        #endregion

        //if (TestOnGround() /*&& _input.jump*/)
        //{
        //    canJump = true;
        //}

        //if (canJump && _input.jump)
        //{
        //    Jump();
        //}

        _moveInput.y = Input.GetAxisRaw("Vertical");

        #region GRAVITY
        if (_rb.velocity.y < 0 && Input.GetKey(KeyCode.F))
        {
            //Much higher gravity if holding down
            SetGravityScale(gravityScale * fastFallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Max(_rb.velocity.y, -maxFastFallSpeed));
        }
        else if (_isJumpCut)
        {
            //Higher gravity if jump button released
            SetGravityScale(gravityScale * jumpCutGravityMult);
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Max(_rb.velocity.y, -maxFallSpeed));
        }
        else if (_rb.velocity.y < 0)
        {
            //Higher gravity if falling
            SetGravityScale(gravityScale * fallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Max(_rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            
            //Default gravity if standing on a platform or moving upwards
            SetGravityScale(gravityScale);
            //heightObjectTest = false;
        }
        #endregion
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_speed, _rb.velocity.y);
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        //RaycastHit2D raycastHit = Physics2D.BoxCast(_playerCollider.bounds.center, _playerCollider.bounds.size, 0f, Vector2.down, _playerCollider.bounds.extents.y, _buildingLayerMask);
        //if (collision.gameObject.CompareTag("Building") && transform.position.y < 30f)
        //{
        //    _isJumpCut = true;
        //}
    }
    private void Jump()
    {
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        canJump = false;
        #region Perform Jump
        float force = jumpForce;
        if (_rb.velocity.y < 0)
            force -= _rb.velocity.y;

        _rb.AddForce(transform.up * force, ForceMode2D.Impulse);
        SoundManager.Instance.MakeJumpSound();
        #endregion
        Debug.Log("Jump");
    }

    private bool TestOnGround()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_playerCollider.bounds.center, _playerCollider.bounds.size, 0f, Vector2.down, _playerCollider.bounds.extents.y, _groundLayerMask);
        Color testColor;
        if (raycastHit.collider != null)
        {
            testColor = Color.green;
            if(bulshitTesting)
            {
                //raycastHit.collider.gameObject.SetActive(false);
            }
            
        }
        else
        {
            testColor = Color.red;
        }

        Debug.DrawRay(_playerCollider.bounds.center, Vector2.down * (_playerCollider.bounds.extents.y), testColor);
        return raycastHit.collider != null;
    }

    private bool TestCeiling()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_playerCollider.bounds.center, _playerCollider.bounds.size, 0f, Vector2.up, _playerCollider.bounds.extents.y + 1f, _buildingLayerMask);
        Color testColor;
        if (raycastHit.collider != null)
        {
            testColor = Color.blue;
            if (bulshitTesting)
            {
                //raycastHit.collider.gameObject.SetActive(false);
            }

        }
        else
        {
            testColor = Color.yellow;
        }

        Debug.DrawRay(_playerCollider.bounds.center, Vector2.up * (_playerCollider.bounds.extents.y + 1f), testColor);
        return raycastHit.collider != null;
    }


    #region INPUT CALLBACKS
    //Methods which whandle input detected in Update()
    public void OnJumpInput()
    {
        LastPressedJumpTime = jumpInputBufferTime;
    }

    public void OnJumpUpInput()
    {
        if (CanJumpCut())
            _isJumpCut = true;
    }

    #endregion
    public void SetGravityScale(float scale)
    {
        _rb.gravityScale = scale;
    }

    #region CHECK METHODS

    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !isJumping;
    }
    private bool CanJumpCut()
    {
        return isJumping && _rb.velocity.y > 0;
    }
    #endregion

    //private void OnDrawGizmosSelected()
    //{
    //    float extraHeight = 0.1f;
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireCube(_playerCollider.bounds.center, _playerCollider.bounds.size);
    //}

    public void Fire()
    {
        if (canShot && _input.fire)
        {
            EventHolder.Instance.OnActivateFireBall();
            PlayerEventPerformer.Instance._fireBallThing.SetActive(false);
            canShot = false;
        }
    }
}
