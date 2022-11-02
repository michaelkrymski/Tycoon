using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerManager : MonoBehaviour
{
    public static ConveyerManager Instance{get; private set;}

    private void Awake()
    {
        Instance = this;
    }
}
