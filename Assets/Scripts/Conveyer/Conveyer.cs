using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour
{
    private ConveyerItem beltItem;
    private ConveyerManager conveyerManager;
    private BuildingData.Dir dir;
    private Conveyer conveyerInFront;

    public void Create(BuildingData.Dir dir)
    {
        this.dir = dir;
    }

    private void Awake()
    {
        conveyerManager = ConveyerManager.Instance;
    }

    public Conveyer SetFront()
    {  
        if (conveyerInFront != null)
        {
            SetFront(conveyerInFront);
            return conveyerInFront;
        }
        return null;
    }

    public void SetFront(Conveyer backConveyer)
    {
        backConveyer.conveyerInFront = this;
    }

    private void Update()
    {
        if (beltItem != null && conveyerInFront.beltItem == null)
        {
            StartCoroutine(LerpPosition(conveyerInFront.transform.position, 1f));
        }
    }

    private IEnumerator LerpPosition(Vector2 targetPosition, float duration)
    {
        float time = 0;
        Vector2 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        beltItem = null;
        conveyerInFront.beltItem = beltItem;
    }
}
