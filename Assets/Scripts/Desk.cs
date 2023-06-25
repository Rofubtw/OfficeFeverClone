using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk : MonoBehaviour
{

    [SerializeField] private Transform paperPlace;
    [SerializeField] private Transform papersParent;
    [SerializeField] private int deskPaperLimit;
    [SerializeField] private Transform[] moneySpawnPlace;
    [SerializeField] private int moneySpawnPlaceMaxIndex = 36;
    [SerializeField] private GameObject moneyPrefab;
    [SerializeField] private Transform moneyParent;
    [SerializeField] private List<Transform> deskPaperList;



    private const float PaperDropTimeMax = .2f;
    private const float MoneySpawnTimeMax = .7f;
    private const float CarryYAxisIncrement = 0.1f;

    private List<Transform> _moneyList = new List<Transform>();
    private float _paperDropTimer;
    private float _moneySpawnTimer;
    private float _carryYAxis;


    private void Update()
    {
        MoneySpawnTimer();
    }

    private Transform SpawnMoney(int moneySpawnPlaceIndex)
    {
        if (moneySpawnPlaceIndex >= moneySpawnPlaceMaxIndex) return null;
        
        Vector3 position = moneySpawnPlace[moneySpawnPlaceIndex].position;

        GameObject newMoney = Instantiate(moneyPrefab, position, Quaternion.identity);

        _moneyList.Add(newMoney.transform);
        newMoney.transform.SetParent(moneyParent);

        return newMoney.transform;
    }

    public void PaperDropTimer()
    {
        _paperDropTimer -= Time.deltaTime;

        if (_paperDropTimer <= 0f)
        {
            _paperDropTimer = PaperDropTimeMax;
            
            if (TryMovePaperFromPlayerToDesk())
            {
                _carryYAxis = CarryYAxisIncrement;
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
    
    private void ConvertPaper() // azoya push nasýl yapýlýyor sor her 3 kaðýda 1 para
    {
        Transform paper = deskPaperList[deskPaperList.Count - 1];
        
        deskPaperList.Remove(paper);
        
        Destroy(paper.gameObject);

        _carryYAxis -= CarryYAxisIncrement;
    }


    private bool TryMovePaperFromPlayerToDesk()
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
    }

    private bool TryConvertPaperToMoney()
    {
        int moneyPlaceIndex = _moneyList.Count;
        
        if(deskPaperList.Count <= 0) return false;

        SpawnMoney(moneyPlaceIndex);
        ConvertPaper();

        return true;
    }

}
