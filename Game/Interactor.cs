using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    ControlsManager controls;
    Transform highlight;
    RaycastHit raycastHit;

    void Awake()
    {
        controls = GameController.controls;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 m_position = controls.currentMouse.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(m_position);
        bool hit = Physics.Raycast(ray, out raycastHit);
        if (hit)
        {
            Transform t = raycastHit.transform;
            if (t.CompareTag("Interactable") && t != highlight)
            {
                if (highlight != null) highlight.GetComponent<Outline>().enabled = false;
                if (t.GetComponent<InteractionTarget>().InteractionAllowed())
                {
                    highlight = t;
                    highlight.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    highlight = null;
                }
            }
            else if (!t.CompareTag("Interactable") && highlight != null)
            {
                highlight.GetComponent<Outline>().enabled = false;
                highlight = null;
            }
        }
        else if(highlight != null)
        {
            highlight.GetComponent<Outline>().enabled = false;
            highlight = null;
        }

        controls.interact.started += ctx => StartInteraction(ctx);
    }

    private void StartInteraction(InputAction.CallbackContext ctx)
    {
        if (highlight != null)
        {
            highlight.GetComponent<InteractionTarget>().Interact();
        }
    }
}
