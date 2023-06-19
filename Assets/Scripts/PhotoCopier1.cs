/*using DG.Tweening;
using OWS.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PhotoCopier : MonoBehaviour
{
    
    private const int MaximumPapers = 50;
    private const float PaperSpawnHeight = -3f;
    private const float JumpPower = 2f;
    private const int JumpTimes = 1;
    private const float JumpDuration = .5f;
    private const int MaximumHeightIndex = 9;
    private const float HeightIncrease = .11f;

    private static ObjectPool<PoolObject> paperPool;

    [SerializeField] private Player player;
    [SerializeField] private Transform[] papersSpawnPlace;
    [SerializeField] private GameObject paperPrefab;
    [SerializeField] private Transform paperParent;
    [SerializeField] private float paperDeliveryTime;
    [SerializeField] private float YAxis;

    public List<Transform> paperList = new List<Transform>();

    private float _paperCollectTimer;
    private float _paperCollectTimerMax = .2f;
    private int _countPapers;


    private void Awake()
    {
        paperPool = new ObjectPool<PoolObject>(paperPrefab);
    }
    private void Start()
    {
        StartCoroutine(PaperSpawningRoutine(paperDeliveryTime));
    }


    public IEnumerator PaperSpawningRoutine(float time)
    {
        int heightIndex = 0;

        while (true)
        {
            if(paperList.Count < MaximumPapers)
            {
                Transform newPaper = SpawnPaper(heightIndex);

                JumpPaperToDestination(newPaper, heightIndex);

                UpdateHeightIndex(ref heightIndex);

            }
            yield return new WaitForSecondsRealtime(time);
        }
    }
    public void PaperCollectTimer()
    {
        _paperCollectTimer -= Time.deltaTime;

        if (_paperCollectTimer <= 0f)
        {

            _paperCollectTimer = _paperCollectTimerMax;

            int lastIndex = paperList.Count - 1; // Last member's index

            for (int i = lastIndex; i >= 0; i--)
            {
                Transform paper = paperList[i];

                if (Player.Instance.carriedPaperList.Count < Player.Instance.carryablePaper)
                {
                    if (paperList.Count > 2)
                    {
                        paperList.RemoveAt(i);
                        Player.Instance.carriedPaperList.Add(paper);

                        paper.parent = null;

                    }

                }
                break;
            }
            
            if (_countPapers > 1) // Bu kýsmý yeniden yaz kötü çalýþýyor.
                _countPapers--;
            if (YAxis > 0)
                YAxis -= HeightIncrease;
        }
    }

    private Transform SpawnPaper(int heightIndex)
    {
        Vector3 position = new Vector3(transform.position.x, PaperSpawnHeight, transform.position.z);

        Transform newPaper = paperPool.PullGameObject(position, Quaternion.identity).transform;
        
        paperList.Add(newPaper);
        
        newPaper.SetParent(paperParent); // This code is gonna make new papers a child of desk
        return newPaper;
    }

    private void JumpPaperToDestination(Transform paper, int heightIndex)
    {
        Vector3 jumpDestination = new Vector3(papersSpawnPlace[heightIndex].position.x, papersSpawnPlace[heightIndex].position.y + YAxis, papersSpawnPlace[heightIndex].position.z);
        paper.transform.DOJump(jumpDestination, JumpPower, JumpTimes, JumpDuration).SetEase(Ease.OutQuad);
    }

    private void UpdateHeightIndex(ref int heightIndex)
    {
        if (heightIndex < MaximumHeightIndex)
        {
            heightIndex++;
        }
        else
        {
            heightIndex = 0;
            YAxis += HeightIncrease;
            _countPapers++;

        }
    }

}*/
