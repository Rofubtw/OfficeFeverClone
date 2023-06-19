/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk : MonoBehaviour
{

    [SerializeField] private Transform paperPlace;
    [SerializeField] private Transform papersParent;
    [SerializeField] private int deskPaperLimit;
    [SerializeField] private List<Transform> deskPaperList = new List<Transform>();

    private float _paperCollectTimer;
    private float _paperCollectTimerMax = .1f;
    private float _carryYAxis;

    public void PaperDropTimer()
    {
        _paperCollectTimer -= Time.deltaTime;

        if (_paperCollectTimer <= 0f)
        {

            _paperCollectTimer = _paperCollectTimerMax;

            int lastIndex = Player.Instance.carriedPaperList.Count - 1; // Last member's index

            for (int i = lastIndex; i >= 0; i--)
            {
                Transform paper = Player.Instance.carriedPaperList[i];

                if (deskPaperList.Count < deskPaperLimit)
                {
                    Player.Instance.carriedPaperList.RemoveAt(i);
                    deskPaperList.Add(paper);

                    float transitionSpeed = 0.1f * Time.deltaTime;
                    paper.position = Vector3.Lerp(paperPlace.position, paper.position, transitionSpeed);
                    
                    paper.SetParent(papersParent);

                    Vector3 paperTransportingLocation = new Vector3(paperPlace.position.x, paperPlace.position.y + _carryYAxis, paperPlace.position.z);
                    paperPlace.position = paperTransportingLocation;
                    _carryYAxis = 0.1f;

                }
                break;
            }
        }
    }
}*/
