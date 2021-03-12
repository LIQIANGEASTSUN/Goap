using UnityEngine;

namespace Goap
{
    public class GoapAgent : MonoBehaviour
    {
        private GoapGoal goapGoal;
        private GoapPlanManager goapPlanManager;

        private GoapAction currentGoapAction = null;

        // Use this for initialization
        protected virtual void Start()
        {
            Init();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            Controller();
        }

        private void Init()
        {
            goapGoal = GetComponent<GoapGoal>();
            goapPlanManager = new GoapPlanManager(goapGoal);
        }
        
        private void Controller()
        {
            if (currentGoapAction != null)
            {
                currentGoapAction.Run();
            }
            else
            {
                ChangeAction();
            }
        }

        private void ActionFinishCallBack(GoapAction goapAction)
        {
            EndAction(goapAction);
        }

        private void ActionFailCallBack(GoapAction goapAction)
        {
            EndAction(goapAction);
        }

        private void EndAction(GoapAction goapAction)
        {
            if (currentGoapAction != goapAction)
            {
                return;
            }

            ChangeAction();
        }

        private void ChangeAction()
        {
            currentGoapAction = goapPlanManager.GetPerformerAction();
            if (currentGoapAction != null)
            {
                currentGoapAction.SetCallBack(ActionFinishCallBack, ActionFailCallBack);
            }
        }
    }
}