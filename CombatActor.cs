using Godot;
using System;

public partial class CombatActor : CharacterBody2D
{
	public int MaxHealth = 10;         // 最大生命值
	[Export] public PackedScene BulletScene;
	public float ShootCD = 0.3f; // 射击冷却时间(秒)
	public float BulletSpeed = 400f;   // 子弹速度
	public int ATK = 1; //攻击力
	[Export] public float MaxChargeTime = 1f;

	public float BloodTime = 0f;// 受伤无敌时间
	public float BloodDuration = 0f;
	public int currentHealth;
	public int energy=0;
	protected double shootTimer = 0.0;

	public int BoomNum = 0; // 炸弹数量

	public class MultiShotConfig
	{
		public int Shots = 1;              // 发射数量
		public float AngleStep = 0f;       // 每颗子弹之间的角度间隔（度数）
		public bool IsCircle = false;      // 是否环形（360°均分）
		public float FireRateMultiplier = 1f; // 射速倍率（>1 表示更慢）

		public static MultiShotConfig Single => new MultiShotConfig { Shots = 1 };
	}

	public override void _Ready()
	{

		GD.Print(Name + " is ready with MaxHealth: " + MaxHealth);
		currentHealth = MaxHealth;
	}

	public override void _Process(double delta)
	{
		//GD.Print(Name + " HP:" + currentHealth);
		BloodTime -= (float)delta;
		shootTimer -= delta;


	}

	// 扣血

	public virtual void TakeDamage(int amount, Vector2 hitFrom = default,bool isBomb=false)
	{
		if (BloodTime > 0) return; // 无敌时间
		currentHealth -= amount;
		GD.Print(Name + " took damage: " + amount + " from " + hitFrom);
		BloodTime = BloodDuration;
	}



	// 死亡

	protected virtual void Die()
	{

		QueueFree();

	}


	/// 射击
	public MultiShotConfig ShotConfig = MultiShotConfig.Single;
	public void Shoot(Vector2 Dir,Faction faction,float chargeTime=0f)
	{

		if(shootTimer > 0) return; // 射速冷却中
		// 计算蓄力伤害
		float t = Mathf.Clamp(chargeTime / MaxChargeTime, 0f, 1f);
		int damage = Mathf.RoundToInt(Mathf.Lerp(ATK, ATK*2, t));

		// 获取射击方向（例如鼠标方向或移动方向）
		Vector2 direction = Dir;

		// 调用多重射击逻辑
		if (ShotConfig.Shots <= 1)
		{
			SpawnBullet(direction, damage,faction);
		}
		else
		{
			if (ShotConfig.IsCircle)
			{
				float step = 360f / ShotConfig.Shots;
				for (int i = 0; i < ShotConfig.Shots; i++)
				{
					float angle = Mathf.DegToRad(step * i);
					Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
					SpawnBullet(dir, damage,faction);
				}
			}
			else
			{
				float halfSpread = (ShotConfig.Shots - 1) * ShotConfig.AngleStep / 2f;
				for (int i = 0; i < ShotConfig.Shots; i++)
				{
					float angle = -halfSpread + i * ShotConfig.AngleStep;
					Vector2 dir = direction.Rotated(Mathf.DegToRad(angle));
					SpawnBullet(dir, damage,faction);
				}
			}
		}

		// 射速冷却
		shootTimer = ShootCD * ShotConfig.FireRateMultiplier;
	}

	private void SpawnBullet(Vector2 dir, int damage, Faction faction)
	{
		GD.Print("SpawnBullet");
		var bullet = BulletScene.Instantiate<Bullet>();


		bullet.ATK = damage;
		bullet.GlobalPosition = GetNode<Marker2D>("GunPoint").GlobalPosition;
		bullet.Direction = dir.Normalized();
		bullet.Speed = BulletSpeed;
		bullet.ShooterFaction = faction;
		
		GetTree().CurrentScene.AddChild(bullet);
	}
	public void Explode()
	{
		if (BoomNum <= 0) return;
		BoomNum--;
		var boom = GD.Load<PackedScene>("res://tools/Boom.tscn").Instantiate<Boom>();
		boom.GlobalPosition = GlobalPosition;
		GetTree().Root.AddChild(boom);
	}
}
