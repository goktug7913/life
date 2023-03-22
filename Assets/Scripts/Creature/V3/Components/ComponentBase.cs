using UnityEngine;

namespace Creature.V3.Components
{
    public class ComponentBase : MonoBehaviour
    {
        // The component base is a base class for all components.
        // This class is responsible for adding the component to the parent object's list of components.
        // This is done in the Start() method, which children of this class can override, and call base.Start() to ensure the component is added to the list.
        // Components which need to access the parent or other components can do so by accessing the ParentObject variable.

        LifeBaseV3 ParentObject;

        // Start is called before the first frame update
        public virtual void Start()
        {
            ParentObject = GetComponentInParent<LifeBaseV3>();
            ParentObject.components.Add(this);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
