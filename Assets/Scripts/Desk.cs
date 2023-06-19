using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk : MonoBehaviour
{
    [SerializeField] private Transform paperPlace;
    [SerializeField] private Transform papersParent;
    [SerializeField] private int deskPaperLimit;

    private const float PaperDropTimeMax = .1f;
    private const float CarryYAxisIncrement = 0.1f;

    private List<Transform> deskPaperList = new List<Transform>();
    private float paperDropTimer;
    private float carryYAxis;

    public void PaperDropTimer()
    {
        paperDropTimer -= Time.deltaTime;

        if (paperDropTimer <= 0f)
        {
            paperDropTimer = PaperDropTimeMax;
            if (TryMovePaperFromPlayerToDesk())
            {
                carryYAxis = CarryYAxisIncrement;
            }
        }
    }

    private bool TryMovePaperFromPlayerToDesk()
    {
        int lastIndex = Player.Instance.carriedPaperList.Count - 1;

        if (lastIndex < 0 || deskPaperList.Count >= deskPaperLimit) return false;

        Transform paper = Player.Instance.carriedPaperList[lastIndex];

        Player.Instance.carriedPaperList.RemoveAt(lastIndex);
        deskPaperList.Add(paper);

        paper.position = Vector3.Lerp(paperPlace.position, paper.position, 0.1f * Time.deltaTime);
        paper.SetParent(papersParent);

        paperPlace.position += new Vector3(0, carryYAxis, 0);

        return true;
    }
}
