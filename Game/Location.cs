using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    CameraController holder;
    Smartphone smartphone;
    public string title = "Default";
    public float camDistance = 20f;
    public bool isStartingPoint;

    private void Start()
    {
        holder = Camera.main.transform.parent.GetComponent<CameraController>();
        transform.gameObject.SetActive(isStartingPoint);
        smartphone = GameObject.Find("Smartphone").GetComponent<Smartphone>();

        if (isStartingPoint) 
        { 
            smartphone.currentLoc = this; 
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void Visit()
    {
        transform.gameObject.SetActive(true);
        holder.SetFromTransform(GetPivot());
        Camera.main.transform.localPosition = new(0f, 0f, -camDistance);
        smartphone.SetCurrentLocation(this);
    }
    public void Leave()
    {
        transform.gameObject.SetActive(false);
    }

    public Transform GetPivot()
    {
        Transform? t_pivot = transform.Find("pivot");
        return t_pivot != null ? t_pivot : transform;
    }

}
