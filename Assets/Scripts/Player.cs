using System;
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

    private const float PlayerHeight = 2f;
    private const float PlayerRadius = .7f;
    private const float InteractDistance = 2f;

    private Touch _touch;
    private Vector3 _touchFirst;
    private Vector3 _touchLast;
    private bool _dragStarted;
    private bool _isMoving;
    private Vector3 _lastInteractDir;

    private float speed => movementSpeed * Time.deltaTime;
    private float rotSpeed => rotationSpeed * Time.deltaTime;

    private void Awake() => Instance = Instance ?? this;

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
        MovePapers();
    }

    private Vector3 CalculateNormalizedDirection()
    {
        Vector3 direction = (_touchLast - _touchFirst).normalized;
        return new Vector3(direction.x, 0, direction.y);
    }

    public bool IsMoving() => _isMoving;

    private void MovePapers()
    {
        if (carriedPaperList.Count <= 1) return;

        var downPaper = carriedPaperList[0];
        downPaper.position = paperLocation.position;

        for (int i = 1; i < carriedPaperList.Count; i++)
        {
            var firstPaper = carriedPaperList[i - 1];
            var secondPaper = carriedPaperList[i];
            secondPaper.position = Vector3.Lerp(
                secondPaper.position,
                new Vector3(firstPaper.position.x, firstPaper.position.y + 0.1f, firstPaper.position.z),
                45f * Time.deltaTime);
        }
    }

    private void HandleInteractions()
    {
        Vector3 moveDir = CalculateNormalizedDirection();

        if (moveDir != Vector3.zero) _lastInteractDir = moveDir;

        if (!Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit hit, InteractDistance, layerMask)) return;

        if (hit.transform.TryGetComponent(out PhotoCopier photoCopier)) photoCopier.PaperCollectTimer();
        if (hit.transform.TryGetComponent(out Desk desk)) desk.PaperDropTimer();
    }

    private void HandleMovement()
    {
        if (Input.touchCount <= 0) return;
        _touch = Input.GetTouch(0);

        if (_touch.phase == TouchPhase.Began)
        {
            _dragStarted = true;
            _isMoving = true;
            _touchFirst = _touch.position;
        }

        if (!_dragStarted) return;

        if (_touch.phase == TouchPhase.Moved) _touchLast = _touch.position;
        if (_touch.phase == TouchPhase.Ended) _dragStarted = _isMoving = false;

        Vector3 moveDir = CalculateNormalizedDirection();

        float moveDistance = speed;

        if (CanMove(ref moveDir, moveDistance))
        {
            transform.position += transform.forward * speed;
            transform.forward = moveDir * rotSpeed;
        }
    }

    private bool CanMove(ref Vector3 moveDir, float moveDistance)
    {
        if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius, moveDir, moveDistance))
            return true;

        Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
        if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius, moveDirX, moveDistance))
        {
            moveDir = moveDirX;
            return true;
        }

        Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
        if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius, moveDirZ, moveDistance))
        {
            moveDir = moveDirZ;
            return true;
        }

        return false;
    }
}
