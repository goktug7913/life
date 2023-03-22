using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Creature.V3.Components
{
    public class Sensors : ComponentBase
    {
        float _visionRange;
        float _horizontalFov;
        
        float _hearingRange;

        [SerializeField] List<LifeBaseV3> _lifeInVisionRange;
        [SerializeField] List<LifeBaseV3> _lifeInFieldOfView;
        
        List<LifeBaseV3> _lifeInHearingRange;

        void Awake()
        {
            
        }

        void Start()
        {
            base.Start();
            // TODO: get from creature
            _visionRange = 10;
            _horizontalFov = 90;
            _hearingRange = 10;
        }

        void Update()
        {
            GetVisibleLife();
        }

        void GetVisibleLife()
        {
            // get LifeBaseV3 in vision range
            _lifeInVisionRange = Physics.OverlapSphere(transform.position, _visionRange, 1 << LayerMask.NameToLayer("Life")).Select(x => x.GetComponent<LifeBaseV3>()).ToList();
            // remove self from list
            _lifeInVisionRange.RemoveAll(x => x == GetComponent<LifeBaseV3>());
        }
    }
}
