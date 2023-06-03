using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed = 500;


    private Touch _touch;
    private Vector3 _touchFirst;
    private Vector3 _touchLast;
    private bool _dragStarted;
    private bool _isMoving;


    private void Update() => MoveAndRotateCharacter();


    private void MoveAndRotateCharacter()
    {
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began)
            {
                _dragStarted = true;
                _isMoving = true;
                _touchFirst = _touch.position;
                _touchLast = _touch.position;
            }
        }
        if (_dragStarted)
        {
            if (_touch.phase == TouchPhase.Moved)
            {
                _touchLast = _touch.position;
            }
            if (_touch.phase == TouchPhase.Ended)
            {
                _touchLast = _touch.position;
                _isMoving = false;
                _dragStarted = false;
            }
            gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, CalculateRotation(), rotationSpeed * Time.deltaTime);
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
        }
    }

    public bool IsMoving()
    {
        return _isMoving;
    }

    private Quaternion CalculateRotation()
    {
        Quaternion temp = Quaternion.LookRotation(CalculateNormalizedDirection(),Vector3.up);

        return temp;
    }

    private Vector3 CalculateNormalizedDirection() 
    {
        Vector3 temp = (_touchLast  - _touchFirst).normalized;
        temp.z = temp.y;
        temp.y = 0;
        
        return temp;
    }

}
