using UnityEngine;

namespace Goap
{
    public class GoapAgent : MonoBehaviour
    {
        protected GoapPlanManager goapPlanManager;
        protected GoapActionManager goapActionManager;
        protected GoapStateManager goapStateManager;
        protected StateMachine stateMachine;
        protected AnimationManager animationManager;
        protected SkillManager skillManager;
        protected AttackManager attackManager;

        private GoapAgent target;
        private float hpValue = 0;
        private int npcId;
        private NpcData npcData = null;

        private Hp hp = null;

        protected virtual void Awake()
        {
        }

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

        public virtual void Init()
        {
            goapPlanManager = new GoapPlanManager( this);
            goapActionManager = new GoapActionManager(this);

            goapStateManager = new GoapStateManager(this);
            stateMachine = new StateMachine(this);
            animationManager = new AnimationManager(transform);

            skillManager = new SkillManager(this);
            attackManager = new AttackManager(this);

            if (HpControllerPanel.instance != null)
            {
                hp = HpControllerPanel.instance.GetHp(this);
            }
        }

        protected virtual void Controller()
        {
            if (!IsAlive())
            {
                UnitManager.RemoveMonster(this);
                GameObject.Destroy(gameObject);
                return;
            }

            goapActionManager.OnFrame();

            goapStateManager.OnFrame();

            stateMachine.OnFrame();
            skillManager.OnFrame();
        }

        public NpcData NpcData { get { return npcData; } }

        public int NpcID {
            get { return npcId; }
            set {
                npcId = value;
                npcData = TableTool.GetTableDataRow<NpcData>(TableType.Npc, value);
                hpValue = npcData.hp;
            }
        }

        public Camp Camp { get; set; }

        public GoapAgent Target
        {
            get { return target; }
            set { target = value; }
        }

        public bool HasTarget { get { return target != null; } }

        /// <summary>
        /// 记录攻击使用的技能
        /// </summary>
        public Skill AttackUseSkill { get; set; }

        public bool IsAlive()
        {
            return hpValue > 0;
        }

        public float Hp { get { return hpValue; } }

        public void Damage(float value)
        {
            hpValue -= value;

            RefreshHP(value);
        }

        public void RefreshHP(float value)
        {
            if (hp == null)
            {
                return;
            }

            hp.SetHp((int)value);

            if (!IsAlive())
            {
                hp.Release();
            }
        }

        public void LookTarget()
        {
            transform.LookAt(target.transform);
        }

        public Vector3 Position { get { return transform.position; } }

        public AnimationManager AnimationManager { get { return animationManager; } }

        /// <summary>
        /// 行为管理器
        /// </summary>
        public GoapPlanManager GoapPlanManager { get { return goapPlanManager; } }

        /// <summary>
        /// 行为管理器
        /// </summary>
        public GoapActionManager GoapActionManager { get { return goapActionManager; } }

        /// <summary>
        /// 目标、状态 管理器
        /// </summary>
        public GoapStateManager GoapStateManager { get { return goapStateManager; } }

        /// <summary>
        /// 有限状态机
        /// </summary>
        public StateMachine StateMachine { get { return stateMachine; } }

        /// <summary>
        /// 技能管理
        /// </summary>
        public SkillManager SkillManager { get { return skillManager; } }

        /// <summary>
        /// 攻击管理
        /// </summary>
        public AttackManager AttackManager { get { return attackManager; } }
    }
}