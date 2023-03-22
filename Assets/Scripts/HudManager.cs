using Creature.V3;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    // We will use this class to manage the user interface.
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ShowHud()
    {
        // Show the user interface.
    }
    
    public void HideHud()
    {
        // Hide the user interface.
    }
    
    void OnGUI()
    {
        // Draw the object name on every LifeBaseV3 object.
        foreach (LifeBaseV3 lifeBase in FindObjectsOfType<LifeBaseV3>()) // Refactor TODO
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(lifeBase.transform.position);
            screenPosition.y = Screen.height - screenPosition.y;
            GUI.Label(new Rect(screenPosition.x, screenPosition.y, 200, 200), lifeBase.name);

        }
    }
}
