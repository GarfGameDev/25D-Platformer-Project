using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private bool _goingDown = false;
    [SerializeField]
    private Transform _origin, _midPoint, _target;
    private float _speed = 2.5f;
    private bool _elevatorCalled = false;
    [SerializeField]
    private float _middleDist;
    private UIManager _uiManager;

    private void Start()
    {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        
        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is null");
        }
    }

    public void ChangeDirection()
    {
        _goingDown = !_goingDown;
    }

    private void FixedUpdate()
    {
        _middleDist = Vector3.Distance(_midPoint.position, transform.position);
        if (_goingDown == true && _elevatorCalled == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);

            

            if (_middleDist < 0.1f && _middleDist > -0.1f)
            {
                StartCoroutine(PauseTheElevator());
            }

            if (transform.position == _target.position)
            {
                StartCoroutine(PauseTheElevator());
                ChangeDirection();
            }


        }
        else if (_goingDown == false && _elevatorCalled == true) 
        {
            transform.position = Vector3.MoveTowards(transform.position, _origin.position, _speed * Time.deltaTime);

            if (_middleDist < 0.1f && _middleDist > -0.1f)
            {
                StartCoroutine(PauseTheElevator());
            }

            if (transform.position == _origin.position)
            {
                StartCoroutine(PauseTheElevator());
                ChangeDirection();
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (_elevatorCalled == false) 
            {
                _uiManager.ActivateElevatorText();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _uiManager.DeactivateElevatorText();
                    _elevatorCalled = true;
                    ChangeDirection();
                }
            }
            

        }
    }

    IEnumerator PauseTheElevator() 
    {
        _speed = 0f;
        yield return new WaitForSeconds(5.0f);
        _speed = 2.5f;
    }
    
}
