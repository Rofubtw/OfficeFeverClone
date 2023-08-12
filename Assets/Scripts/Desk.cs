using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Desk : MonoBehaviour
{

    private const float PaperDropTimeMax = .2f;
    private const float MoneySpawnTimeMax = .7f;
    private const int ObjectsPerRow = 3;
    private const int RowsPerColumn = 3;

    [SerializeField] private MoneyCollect moneyCollect;
    [SerializeField] private Transform paperPlace;
    [SerializeField] private int deskPaperLimit;
    [SerializeField] private GameObject moneyPrefab;
    [SerializeField] private List<Transform> deskPaperList;
    [SerializeField] private Transform moneyTarget;
    


    private List<Transform> _money = new List<Transform>();
    private float _paperDropTimer;
    private float _moneySpawnTimer;
    private float _floatOffsety = .1f;
    private float _attractDistance = .5f;
    private float _duration = .1f;
    private Vector3 _offsetx = new Vector3(.8f, 0, 0);
    private Vector3 _offsety = new Vector3(0, .2f, 0);
    private Vector3 _offsetz = new Vector3(0, 0, -.4f);


    private void Update()
    {
        MoneySpawnTimer();
        OrganizePapers();
        moneyCollect.AttractMoney(Player.Instance.transform, _money, _attractDistance, _duration);
    }

    private Transform SpawnMoney(int moneySpawnPlaceIndex)
    {
        Vector3 position = moneyTarget.transform.position;

        GameObject newMoney = Instantiate(moneyPrefab, position, Quaternion.identity);

        _money.Add(newMoney.transform);

        return newMoney.transform;
    }

    public void PaperDropTimer()
    {
        _paperDropTimer -= Time.deltaTime;

        if (_paperDropTimer <= 0f)
        {
            _paperDropTimer = PaperDropTimeMax;

            if (TryDropPapers(Player.Instance.carriedPaperList, paperPlace.position, .1f))
            {

            }
        }
    }
    public void MoneySpawnTimer()
    {
        _moneySpawnTimer -= Time.deltaTime;

        if (_moneySpawnTimer <= 0f)
        {
            _moneySpawnTimer = MoneySpawnTimeMax;

            if (TryConvertPaperToMoney())
            {

            }
        }
    }
    
    private void ConvertPaper()
    {
        foreach (Transform paper in deskPaperList)
        {
            deskPaperList.Remove(paper);

            Destroy(paper.gameObject);

            break;
        }
    }


    /*private bool TryMovePaperFromPlayerToDesk()
    {
        int lastIndex = Player.Instance.carriedPaperList.Count - 1;

        if (lastIndex < 0 || deskPaperList.Count >= deskPaperLimit) return false;

        Transform paper = Player.Instance.carriedPaperList[lastIndex];

        Player.Instance.carriedPaperList.RemoveAt(lastIndex);
        deskPaperList.Add(paper);

        paper.transform.position = Vector3.Lerp(paperPlace.position, paper.transform.position, 0.1f * Time.deltaTime);
        paper.transform.SetParent(papersParent);

        paperPlace.position += new Vector3(0, _carryYAxis, 0);

        return true;
    }*/

    public bool TryDropPapers(List<Transform> paperList, Vector3 dropPoint, float offset)
    {
        for (int i = paperList.Count - 1; i >= 0; i--)
        {
            Transform currentPaper = paperList[i];
            // Calculate new drop point for each paper, stack them on top of each other
            Vector3 newDropPoint = new Vector3(dropPoint.x, dropPoint.y + (offset * i), dropPoint.z);
            currentPaper.position = newDropPoint;

            paperList.RemoveAt(i);
            deskPaperList.Add(currentPaper);
            break;
        }
        return true;
    }

    private bool TryConvertPaperToMoney()
    {
        int moneyPlaceIndex = _money.Count;
        
        if(deskPaperList.Count <= 0) return false;

        ConvertPaper();
        SpawnMoney(moneyPlaceIndex);
        OrganizeMoney();

        return true;
    }
    private void OrganizeMoney()
    {
        int i = 0;
        foreach (Transform money in _money)
        {
            Vector3 position = moneyTarget.position
                               + ((i / ObjectsPerRow) % RowsPerColumn) * _offsetx  // x eksenindeki konum
                               + (i / (ObjectsPerRow * RowsPerColumn)) * _offsety // y eksenindeki konum
                               + (i % ObjectsPerRow) * _offsetz;// z eksenindeki konum
            money.transform.position = position;
            i++;
        }
    }
    private void OrganizePapers()
    {
        int i = 0;
        foreach (Transform paper in deskPaperList)
        {
            Vector3 position = paperPlace.position + new Vector3(0, i * _floatOffsety, 0); // y eksenindeki konum
            paper.transform.position = position;
            i++;
        }
    }

}
