using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SPhoneButton { notes = 1, home = 2, gallery = 3, options = 4, quit = 5 }
public class SmartphoneButton : InteractionTarget
{
    public SPhoneButton buttonType;

    public override void Interact()
    {
        base.Interact();

        switch (buttonType)
        {
            case SPhoneButton.notes:
                break;

            case SPhoneButton.home:
                break;

            case SPhoneButton.gallery:
                break;

            case SPhoneButton.options:
                break;

            case SPhoneButton.quit:
                GameController.state.LoadLevel(1);
                break;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
