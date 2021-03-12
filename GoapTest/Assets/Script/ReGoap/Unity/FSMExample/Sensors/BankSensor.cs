using System.Collections.Generic;
using ReGoap.Unity.FSMExample.OtherScripts;
using UnityEngine;

namespace ReGoap.Unity.FSMExample.Sensors
{
    public class BankSensor : ReGoapSensor
    {
        private Dictionary<Bank, Vector3> banks;

        public float MinPowDistanceToBeNear = 1f;

        void Start()
        {
            Bank[] bankArr = (Bank[])GameObject.FindObjectsOfType(typeof(Bank));

            banks = new Dictionary<Bank, Vector3>(bankArr.Length);
            foreach (var bank in bankArr)
            {
                banks[bank] = bank.transform.position;
            }
        }

        public override void UpdateSensor()
        {
            var worldState = reGoapAgent.GetWorldState();
            worldState.Set("seeBank", banks.Count > 0);

            Bank nearestBank = Utilities.GetNearest(transform.position, banks);
            worldState.Set("nearestBank", nearestBank);

            Vector3 nearestBankPos = (nearestBank != null ? nearestBank.transform.position : Vector3.zero);
            worldState.Set("nearestBankPosition", nearestBankPos);
        }
    }
}