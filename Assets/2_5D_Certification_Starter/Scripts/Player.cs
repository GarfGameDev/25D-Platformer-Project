using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private float speed = 10.0f;
    private float gravity = 10.0f;
    private float _jumpHeight = 15.0f;
    private Vector3 _direction, velocity;
    private Animator _anim;
    private bool _jumping = false;
    private bool _onLedge = false;
    private Ledge _activeLedge;
    private LadderTop _activeLadderTop;
    private UIManager _uiManager;
    private int _coins = 0;
    private float _prevSpeed;
    [SerializeField]
    private bool _onLadder;
    private bool _ladderClimb;
    private bool _isRolling = false;
    [SerializeField]
    private Transform _startPosition;
    [SerializeField]
    private int _lives = 3;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
        if (_anim != null)
        {
            _prevSpeed = _anim.speed;
        }
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_onLedge == true) 
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _anim.SetTrigger("ClimbUp");
            }
        }

        if (_onLadder == true && _isRolling == false) 
        {
            float verticalInput = Input.GetAxis("Vertical");
            _direction = new Vector3(0, verticalInput, 0);
            velocity = _direction * speed;
            _anim.SetBool("LadderClimb", true);

            if (verticalInput == 0 && _ladderClimb == false)
            {
                _anim.speed = 0;
            }
            else 
            {
                _anim.speed = _prevSpeed;
            }
        }
        if (_controller.isGrounded == true && _onLadder == false) 
        {
            _anim.SetBool("Falling", false);
            if (_isRolling == false) 
            {
                if (_jumping == true) 
                {
                    _jumping = false;
                    _anim.SetBool("Jumping", _jumping);
                    _anim.SetBool("StandingJump", _jumping);
                    _anim.SetBool("Falling", true);
                }

                float horizontalInput = Input.GetAxis("Horizontal");
                _direction = new Vector3(0, 0, horizontalInput);
                velocity = _direction * speed;

                _anim.SetFloat("Speed", Mathf.Abs(horizontalInput));

                if (horizontalInput != 0)
                {
                    Vector3 facing = transform.localEulerAngles;
                    facing.y = _direction.z > 0 ? 0 : 180;
                    transform.localEulerAngles = facing;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (horizontalInput == 0)
                    {
                        _jumping = true;
                        _anim.SetBool("StandingJump", _jumping);
                        velocity.y += _jumpHeight;
                    }
                    else 
                    {
                        _jumping = true;
                        _anim.SetBool("Jumping", _jumping);
                        velocity.y += _jumpHeight;
                    }

                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    velocity.z = 0;
                    _anim.SetTrigger("Rolling");
                    _isRolling = true;
                }
            }

        }

        else 
        {
            _anim.SetBool("Falling", true);
        }

        velocity.y -= gravity * Time.fixedDeltaTime;

        _controller.Move(velocity * Time.deltaTime);
    }

    public void GrabLedge(Vector3 handPos, Ledge currentLedge)
    {
        _controller.enabled = false;

        _anim.SetBool("LedgeGrab", true);
        _anim.SetFloat("Speed", 0.0f);
        _anim.SetBool("Jumping", false);
        _onLedge = true;
        _activeLedge = currentLedge;

        transform.localPosition = handPos;
    }

    public void ClimbUpComplete()
    {
        transform.localPosition = _activeLedge.GetStandPos();
        _anim.SetBool("LedgeGrab", false);
        _onLedge = false;
        _controller.enabled = true;
        this.transform.parent = null;
    }

    public void AddCoins()
    {
        _coins++;
        _uiManager.UpdateCoinText(_coins);
        if (_coins == 10)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void ClimbLadder()
    {
        _onLadder = true;
    }

    public void OffLadder()
    {
        _onLadder = false;
    }

    public void GrabLadderTop(Vector3 handPos, LadderTop currentLadderTop)
    {
        _controller.enabled = false;
        _ladderClimb = true;
        _onLadder = false;
        _anim.SetTrigger("LadderClimbUp");
        _anim.SetFloat("Speed", 0.0f);
        _activeLadderTop = currentLadderTop;

        transform.localPosition = handPos;
    }

    public void LadderClimbComplete()
    {
        _ladderClimb = false;
        transform.localPosition = _activeLadderTop.GetStandPos();
        _anim.SetBool("LadderClimb", false);
        _controller.enabled = true;
        this.transform.parent = null;
    }

    public void RollingComplete()
    {
        if (transform.rotation.y == 0)
        {
            _controller.Move(new Vector3(0, 0, 12.5f));
            _isRolling = false;
        }

        else
        {
            _controller.Move(new Vector3(0, 0, -12.5f));
            _isRolling = false;
        }

    }

    public void Respawn()
    {
        if (_lives > 1)
        {
            _lives--;
            _uiManager.UpdateLivesText(_lives);
            this.transform.position = _startPosition.position;
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else 
        {
            SceneManager.LoadScene(0);
        }

    }
}
