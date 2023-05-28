using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionTarget : MonoBehaviour, IInteractable
{
    DataManager data;
    public string key_value = "_";
    public bool isInteractableOnceOnly;
    public bool saveOnInteraction;
    public List<string> allowIfContains, forbidIfContains;

    bool hasBeenInteractedWith = false;

    private void Start()
    {
        data = GameController.data;
        HideObjectIfKeyExists();

        if (gameObject.GetComponent<Outline>() == null)
        {
            Outline outline = gameObject.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
            outline.OutlineColor = new Color(183f / 255f, 147f / 255f, 49f / 255f);
            outline.OutlineWidth = 4f;
            outline.enabled = false;
        }

        if (!gameObject.CompareTag("Interactable")) gameObject.tag = "Interactable";
    }

    public bool InteractionAllowed()
    {
        if (isInteractableOnceOnly && hasBeenInteractedWith) return false;

        bool allowsEmpty = allowIfContains == null || allowIfContains.Count < 1;
        bool forbidsEmpty = forbidIfContains == null || forbidIfContains.Count < 1;

        if (allowsEmpty && forbidsEmpty)
        {
            return true;
        } 
        else if (!forbidsEmpty)
        {
            return !data.ContainsAllKeys(forbidIfContains);
        }
        else if (!allowsEmpty)
        {
            return data.ContainsAllKeys(allowIfContains);
        } 
        else
        {
            return data.ContainsAllKeys(allowIfContains) && !data.ContainsAllKeys(allowIfContains);
        }


    }

    public void HideObjectIfKeyExists() {
        if (isInteractableOnceOnly && data.key_values.Contains(key_value)) gameObject.SetActive(false);
    }

    public virtual void Interact()
    {
        print("Interacted with " + gameObject.name);
        if (key_value != "_") data.AddKey(key_value);
        if (isInteractableOnceOnly) gameObject.SetActive(false);
        if (saveOnInteraction) GameController.data.SaveGame();
        hasBeenInteractedWith = true;
    }
}
