using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour
{
    private GameObject beltItem;
    private ConveyerManager conveyerManager;
    private BuildingData.Dir dir;

    public void Create()
    {

    }

    private void Awake()
    {
        conveyerManager = ConveyerManager.Instance;
    }

    public GameObject GetItem()
    {
        if(HasItem())
        {
            return beltItem;    
        }
        else
        {
            return null;
        }
    }

    public bool HasItem()
    {
        return beltItem != null;
    }
}
