using UnityEngine;

namespace Creature.V3.Components
{
    public class Sensors : MonoBehaviour
    {
        float _visionRange;
        float _horizontalFov;
        
        float _hearingRange;

        LifeBaseV3[] _lifeInVisionRange;
        LifeBaseV3[] _lifeInHearingRange;
        
        
    }
}
