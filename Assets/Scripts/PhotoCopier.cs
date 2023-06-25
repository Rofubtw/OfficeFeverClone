using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhotoCopier : MonoBehaviour
{

    private const int MaximumPapers = 50;
    private const float PaperSpawnHeight = -3f;
    private const float JumpPower = 2f;
    private const int JumpTimes = 1;
    private const float JumpDuration = .5f;
    private const int MaximumHeightIndex = 9;
    private const float HeightIncrease = .11f;
    private const float PaperCollectTimeMax = .2f;
    private const int ObjectsPerRow = 5; // Bir sýradaki obje sayýsý
    private const int RowsPerColumn = 2;

    [SerializeField] private Player player;
    [SerializeField] private Transform[] papersSpawnPlace;
    [SerializeField] private GameObject paperPrefab;
    [SerializeField] private Transform paperParent;
    [SerializeField] private Transform target;
    [SerializeField] private Transform movingTarget;
    [SerializeField] private Transform secondTarget;
    [SerializeField] private Transform thirdTarget;
    [SerializeField] private Transform fourthTarget;
    [SerializeField] private float paperDeliveryTime;
    
    private List<Transform> _papers = new List<Transform>();
    private Stack<Transform> _deskPapers = new Stack<Transform>();
    private float paperCollectTimer;
    private Vector3 _offsetx = new Vector3(.5f, 0, 0);
    private Vector3 _offsety = new Vector3(0, .2f, 0);
    private Vector3 _offsetz = new Vector3(0, 0, .8f);

    private void Start() => StartCoroutine(PaperSpawningRoutine(paperDeliveryTime));

    
    public IEnumerator PaperSpawningRoutine(float time)
    {
        while (true)
        {
            if (_papers.Count < MaximumPapers)
            {
                Transform newPaper = SpawnPaper();

                OrganizePapers();

            }
            yield return new WaitForSecondsRealtime(time);
        }
    }

    private Transform SpawnPaper()
    {
        Vector3 position = target.position;

        GameObject newPaper = Instantiate(paperPrefab, position, Quaternion.identity);
        _deskPapers.Push(newPaper.transform);
        _papers.Add(_deskPapers.Pop());
        newPaper.transform.SetParent(paperParent);
        return newPaper.transform;
    }

    public void PaperCollectTimer()
    {
        paperCollectTimer -= Time.deltaTime;

        if (paperCollectTimer > 0f) return;

        paperCollectTimer = PaperCollectTimeMax;

        if (TryMovePaperFromCopierToPlayer())
        {

        }
    }

    private bool TryMovePaperFromCopierToPlayer()
    {
        if (_papers.Count < 0 || Player.Instance.carriedPaperList.Count >= Player.Instance.carryablePaper || _papers.Count <= 2)
            return false;

        Transform paper = _papers[^1];
        Player.Instance.carriedPaperList.Add(paper);
        _papers.Remove(paper);

        return true;
    }
    private void OrganizePapers()
    {
        int i = 0;
        foreach (Transform paper in _papers)
        {
            Vector3 position = target.position
                               + (i % ObjectsPerRow) * _offsetx  // x eksenindeki konum
                               + (i / (ObjectsPerRow * RowsPerColumn)) * _offsety // z eksenindeki konum
                               + ((i / ObjectsPerRow) % RowsPerColumn) * _offsetz; // y eksenindeki konum
            paper.transform.position = position;
            i++;
        }
    }
}
