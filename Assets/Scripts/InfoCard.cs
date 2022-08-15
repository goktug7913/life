using System;
using Creature.V2;
using TMPro;
using UnityEngine;

public class InfoCard : MonoBehaviour
{
    // This class is a component for the creatures in the game.
    // It will display the information of the creature over it.

    // the parent is of class animal, so we will need to refactor this later to use
    // the same infoCard class on different life objects.
    AnimalV2 parentObj;
    public bool isVisible = false;

    // These are set in the editor.
    // If something is not working properly, check these values!
    public GameObject infoCardRoot;
    public Canvas infoCardCanvas;
    public CanvasRenderer infoCardCanvasRenderer;
    public TextMeshProUGUI textObj;
    public String text;

    // Start is called before the first frame update
    private void Start()
    {
        SetVisibility(false); // hide the infoCard by default.
        UpdateInfo(); // update once to initialize the text.
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isVisible) return;
        
        // Update the information of the creature.
        UpdateInfo();

        // Always face the camera.
        infoCardRoot.transform.LookAt(Camera.main.transform);
    }

    public void Attach(AnimalV2 parent){
        // This function will attach the infoCard to the parent.
        // It's called from the parent object.
        //This might be unnecessary, but I'm keeping it for now.
        parentObj = parent;
    }

    public void SetVisibility(bool visible){
        // Might be a better way to do this, performance wise.
        infoCardCanvasRenderer.SetAlpha(visible ? 1 : 0);
        isVisible = visible;
    }

    private void UpdateInfo()
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
