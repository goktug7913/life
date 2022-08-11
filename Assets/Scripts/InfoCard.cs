using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoCard : MonoBehaviour
{
    // This class is a component for the creatures in the game.
    // It will display the information of the creature over it.

    LifeBaseV2 parentObj;
    public bool isVisible = true;


    public GameObject infoCardRoot;
    public Canvas infoCardCanvas;
    public TextMeshPro textObj;
    public MeshRenderer meshRenderer;
    public String text;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer.enabled = isVisible;
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

    public void Attach(LifeBaseV2 parent){
        parentObj = parent;
    }

    void SetVisibility(bool visible){
        gameObject.SetActive(visible);
    }

    void UpdateInfo()
    {
        // Update the information of the creature.
        text = "Creature ID: " + parentObj.creatureId + "\n" +
                "Age: " + parentObj.age + "\n" +
                "Health: " + parentObj.health + "\n" +
                "Generation: " + parentObj.generation + "\n";
        
        // Update the text.
        textObj.text = text;
        
        // The state line needs to be fixed, lifebase doesn't have a state.
        // But derived classes have a state, so we probably need to cast it.
        //stateText.text = parentObj.state.ToString();

        // Update the position of the card.
        //transform.position = parentObj.transform.position;
    }
}
