using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passage : InteractionTarget
{
    Smartphone phone;
    Location current;
    public Location destination;

    private void Awake()
    {
        phone = GameObject.Find("Smartphone").GetComponent<Smartphone>();
    }

    public override void Interact()
    {
        
        base.Interact();
        current = phone.currentLoc;
        if (destination != current) 
        {
            destination.Visit();
            current.Leave();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (destination != null)
        {
            Gizmos.DrawLine(transform.position, destination.transform.position);
        }
    }
}
