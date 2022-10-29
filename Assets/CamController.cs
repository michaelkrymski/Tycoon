using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public float panSpeed = 30f;

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;

        pos.x += x * panSpeed * Time.deltaTime;
        pos.y += y * panSpeed * Time.deltaTime;

        transform.position = pos;

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }
}
