using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoCard : MonoBehaviour
{
    // This class is a component for the creatures in the game.
    // It will display the information of the creature over it.

    // the parent is of class animal, so we will need to refactor this later to use
    // the same infoCard class on different life objects.
    AnimalV2 parentObj;
    public bool isVisible = true;


    public GameObject infoCardRoot;
    public Canvas infoCardCanvas;
    public TextMeshProUGUI textObj;
    public String text;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible)
        {
            // Update the information of the creature.
            UpdateInfo();

            // Always face the camera.
            infoCardRoot.transform.LookAt(Camera.main.transform);
        }        
    }

    public void Attach(AnimalV2 parent){
        parentObj = parent;
    }

    void SetVisibility(bool visible){
        gameObject.SetActive(visible);
    }

    void UpdateInfo()
    {
        // Update the information of the creature.
        text = "Creature ID: "  + parentObj.creatureId + "\n" +
                "Age: "         + parentObj.age/60 + " Minutes" + "\n" +
                "Sex: "         + parentObj.sex + "\n" +
                "Health: "      + parentObj.health + "\n" +
                "Hunger: "      + parentObj.hunger + "/" + parentObj.maxHunger + "\n" +
                "Generation: "  + parentObj.generation + "\n" + 
                "State: "       + parentObj.state + "\n" +
                "Gestation: "   + parentObj.gestationProgress + "/" + parentObj.gestationTime + "\n";
        
        // Update the text.
        textObj.text = text;
    }
}
