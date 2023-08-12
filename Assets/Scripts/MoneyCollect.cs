using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCollect : MonoBehaviour
{
    IEnumerator MoveTowardsAndDestroy(Transform objectToMove, Transform target, float duration)
    {
        float startTime = Time.time;
        Vector3 startPos = objectToMove.position;
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            objectToMove.position = Vector3.Lerp(startPos, target.position, t);
            yield return null;
        }

        objectToMove.position = target.position;

        // Increase money in PlayerPrefs
        int currentMoney = PlayerPrefs.GetInt("Money", 0);
        int moneyToAdd = objectToMove.GetComponent<Money>().value;
        PlayerPrefs.SetInt("Money", currentMoney + moneyToAdd);

        Destroy(objectToMove.gameObject);
    }

    public void AttractMoney(Transform player, List<Transform> moneyList, float attractDistance, float duration)
    {
        for (int i = moneyList.Count - 1; i >= 0; i--)
        {
            Transform money = moneyList[i];
            if (Vector3.Distance(player.position, money.position) < attractDistance)
            {
                StartCoroutine(MoveTowardsAndDestroy(money, player, duration));
                moneyList.RemoveAt(i);
            }
        }
    }
}
