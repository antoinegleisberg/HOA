using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Productionsite : MonoBehaviour
{
    public ScriptableItem producedItem;
    public int productionTime;
    public int maxStorage;
    public int nbItems;

    public void StartProduction()
    {
        StartCoroutine(Produce());
    }

    public void StopProduction()
    {
        StopCoroutine(Produce());
    }

    private IEnumerator Produce()
    {
        yield return new WaitForSeconds(productionTime);
        if (nbItems < maxStorage) nbItems++;
    }
}
