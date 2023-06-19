/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{


    public static Player Instance { get; private set; }

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed = 500;
    [SerializeField] private LayerMask layerMask;

    public Transform transportedPapers;
    public int carryablePaper;
    public List<Transform> carriedPaperList = new List<Transform>();
    public Transform paperLocation;



    private Touch _touch;
    private Vector3 _touchFirst;
    private Vector3 _touchLast;
    private bool _dragStarted;
    private bool _isMoving;
    private Vector3 _lastInteractDir;
    




    private float speed => movementSpeed * Time.deltaTime;
    private float rotSpeed => rotationSpeed * Time.deltaTime;


    private void Awake()
    {
        if (Instance == null)
        Instance = this;
    }


    private void Update()
    {
        HandleMovement();
        HandleInteractions();
        MovePapers();
    }

    private void HandleInteractions()
    {
        Vector3 moveDir = CalculateNormalizedDirection();

        if (moveDir != Vector3.zero)
        {
            _lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit raycastHit, interactDistance, layerMask))
        {
            if (raycastHit.transform.TryGetComponent(out PhotoCopier photoCopier))
            {
                photoCopier.PaperCollectTimer();
            }
            if (raycastHit.transform.TryGetComponent(out Desk desk))
            {
                desk.PaperDropTimer();
            }
        }
    }

    private void HandleMovement()
    {
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            
            if (_touch.phase == TouchPhase.Began)
            {
                _dragStarted = true;
                _isMoving = true;
                _touchFirst = _touch.position;
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
                _isMoving = false;
                _dragStarted = false;
            }

            Vector3 moveDir = CalculateNormalizedDirection();

            float moveDistance = speed;
            float playerRadius = .7f;
            float playerHeight = 2f;
            
            bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
            
            if (!canMove)
            {
                Vector3 moveDirX = new Vector3(moveDir.x, 0,0).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirX;
                }
                else
                {
                    Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                    canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                    if(canMove)
                    {
                        moveDir = moveDirZ;
                    }
                }
            }
            
            if(canMove)
            {
                transform.position += transform.forward * speed;
            }
            
            transform.forward = moveDir * rotSpeed;

        }
    }

    public bool IsMoving()
    {
        return _isMoving;
    } 

    private Vector3 CalculateNormalizedDirection() 
    {
        Vector3 temp = (_touchLast  - _touchFirst).normalized;
        temp.z = temp.y;
        temp.y = 0;
        
        return temp;
    }
    private void MovePapers()
    {

        if (carriedPaperList.Count > 1)
        {
            var downPaper = carriedPaperList.ElementAt(0);
            downPaper.position = paperLocation.position;


            for (int i = 1; i < carriedPaperList.Count; i++)
            {
                var firstPaper = carriedPaperList.ElementAt(i - 1);
                var secondPaper = carriedPaperList.ElementAt(i);

                secondPaper.position = new Vector3(Mathf.Lerp(secondPaper.position.x, firstPaper.position.x, 45f * Time.deltaTime),
                    Mathf.Lerp(secondPaper.position.y, firstPaper.position.y + 0.1f, 45f * Time.deltaTime), firstPaper.position.z);
            }
        }
    }

}*/
