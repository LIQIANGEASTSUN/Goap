using System;
using System.Collections.Generic;
using System.Text;
public class NpcData{
	/// <summary>
	///编号
	/// <summary>
	public int id {get;private set;}
	/// <summary>
	///名字
	/// <summary>
	public string name {get;private set;}
	/// <summary>
	///类型
	/// <summary>
	public int type {get;private set;}
	/// <summary>
	///预设
	/// <summary>
	public string prefab {get;private set;}
	/// <summary>
	///移动速度
	/// <summary>
	public float speed {get;private set;}
	/// <summary>
	///技能ID1
	/// <summary>
	public int skillID {get;private set;}
	/// <summary>
	///技能ID2
	/// <summary>
	public int skillID1 {get;private set;}
	/// <summary>
	///技能ID3
	/// <summary>
	public int skillID2 {get;private set;}
	/// <summary>
	///血量
	/// <summary>
	public float hp {get;private set;}
	/// <summary>
	///AI目标
	/// <summary>
	public int aiTarget {get;private set;}
}
