using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    private Transform currentObj = null;

    public static HoverEffect Instance{get; private set;}

    private void Awake()
    {
        Instance = this;
    }

    public void RefreshHoverEffect(Transform hoverObj, Vector3 pos, Quaternion angle)
    {
        if (currentObj != null)
        {
            Destroy(currentObj.gameObject);
        }
        currentObj = Instantiate(hoverObj, pos, angle);
        currentObj.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.75f);
    }

    public void DestroyHoverEffect()
    {
        if (currentObj != null)
        {
            Destroy(currentObj.gameObject);
        }
    }

    public Vector3 ReturnPosition()
    {
        if(currentObj != null)
        {
            return currentObj.position;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public void SetPosition(Vector3 position)
    {
        currentObj.position = position;
    }

    public Quaternion ReturnAngle()
    {
        return currentObj.rotation;
    }

    public void SetAngle(Quaternion angle)
    {
        currentObj.rotation = angle;
    }
}
