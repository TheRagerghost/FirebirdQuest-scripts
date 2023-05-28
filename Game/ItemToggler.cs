using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemToggler : MonoBehaviour
{
    public List<string> key_values;
    DataManager data;
    void Start()
    {
        data = GameController.data;
        data.OnKeysUpdated.AddListener(AskForUpdate);
    }

    void AskForUpdate()
    {
        if (key_values == null || key_values.Count < 1) return;
        if (data.ContainsAllKeys(key_values)) gameObject.SetActive(!gameObject.activeSelf);
    }
}
