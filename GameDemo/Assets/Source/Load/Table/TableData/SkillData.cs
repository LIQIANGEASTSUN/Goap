using System;
using System.Collections.Generic;
using System.Text;
public class SkillData{
	/// <summary>
	///编号
	/// <summary>
	public int id {get;private set;}
	/// <summary>
	///名字
	/// <summary>
	public string name {get;private set;}
	/// <summary>
	///攻击力
	/// <summary>
	public float damage {get;private set;}
	/// <summary>
	///攻击范围
	/// <summary>
	public float attackRange {get;private set;}
	/// <summary>
	///技能 CD
	/// <summary>
	public float coolDown {get;private set;}
	/// <summary>
	///攻击作用类型
	/// <summary>
	public int attackType {get;private set;}
}
