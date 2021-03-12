using System.Collections.Generic;
using ReGoap.Unity.FSMExample.OtherScripts;
using UnityEngine;

// using a sensor for this makes everything more dynamic, if the agent is pushed/moved/teleported he
//  will be able to understand dyamically if he's near the wanted objective
//  otherwise you can set such variables directly in the relative GoapAction (faster/simplier but less flexible)
namespace ReGoap.Unity.FSMExample.Sensors
{
    public class WorkstationSensor : ReGoapSensor
    {
        private Dictionary<Workstation, Vector3> workstationDic;

        public float MinPowDistanceToBeNear = 1f;

        void Start()
        {
            Workstation[] workstationArr = (Workstation[])GameObject.FindObjectsOfType(typeof(Workstation));
            workstationDic = new Dictionary<Workstation, Vector3>(workstationArr.Length);
            foreach (var workstation in workstationArr)
            {
                workstationDic[workstation] = workstation.transform.position; // workstations are static
            }
        }

        public override void UpdateSensor()
        {
            var worldState = reGoapAgent.GetWorldState();
            worldState.Set("seeWorkstation", workstationDic.Count > 0);

            var nearestStation = OtherScripts.Utilities.GetNearest(transform.position, workstationDic);
            worldState.Set("nearestWorkstation", nearestStation);

            Vector3 workstationPosition = (nearestStation != null ? nearestStation.transform.position : Vector3.zero);
            worldState.Set("nearestWorkstationPosition", workstationPosition);
        }
    }
}
