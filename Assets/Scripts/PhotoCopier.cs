using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    [SerializeField] private Player player;
    [SerializeField] private Transform[] papersSpawnPlace;
    [SerializeField] private GameObject paperPrefab;
    [SerializeField] private Transform paperParent;
    [SerializeField] private float paperDeliveryTime;

    private float paperCollectTimer;
    private float yAxis;
    private int countPapers;
    private List<Transform> paperList = new List<Transform>();

    private void Start() => StartCoroutine(PaperSpawningRoutine(paperDeliveryTime));

    private Transform SpawnPaper(int heightIndex)
    {
        Vector3 position = new Vector3(transform.position.x, PaperSpawnHeight, transform.position.z);

        GameObject newPaper = Instantiate(paperPrefab, position, Quaternion.identity);
        paperList.Add(newPaper.transform);

        newPaper.transform.SetParent(paperParent);
        return newPaper.transform;
    }

    public IEnumerator PaperSpawningRoutine(float time)
    {
        int heightIndex = 0;

        while (true)
        {
            if (paperList.Count < MaximumPapers)
            {
                Transform newPaper = SpawnPaper(heightIndex);

                JumpPaperToDestination(newPaper, heightIndex);

                heightIndex = (heightIndex < MaximumHeightIndex) ? heightIndex + 1 : 0;
                if (heightIndex == 0)
                {
                    yAxis += HeightIncrease;
                    countPapers++;
                }
            }
            yield return new WaitForSecondsRealtime(time);
        }
    }

    private void JumpPaperToDestination(Transform paper, int heightIndex)
    {
        Vector3 jumpDestination = papersSpawnPlace[heightIndex].position;
        jumpDestination.y += yAxis;

        paper.transform.DOJump(jumpDestination, JumpPower, JumpTimes, JumpDuration).SetEase(Ease.OutQuad);
    }

    public void PaperCollectTimer()
    {
        paperCollectTimer -= Time.deltaTime;

        if (paperCollectTimer > 0f) return;

        paperCollectTimer = PaperCollectTimeMax;

        if (TryMovePaperFromCopierToPlayer())
        {
            countPapers--;
            if (yAxis > 0)
                yAxis -= HeightIncrease;
        }
    }

    private bool TryMovePaperFromCopierToPlayer()
    {
        int lastIndex = paperList.Count - 1;

        if (lastIndex < 0 || Player.Instance.carriedPaperList.Count >= Player.Instance.carryablePaper || paperList.Count <= 2)
            return false;

        Transform paper = paperList[lastIndex];
        paperList.RemoveAt(lastIndex);
        Player.Instance.carriedPaperList.Add(paper);

        paper.SetParent(null);
        return true;
    }
}
