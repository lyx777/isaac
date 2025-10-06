using Godot;
using System;

public partial class CombatActor : CharacterBody2D
{
	public int MaxHealth = 10;         // 最大生命值
	[Export] public PackedScene BulletScene;
	public float ShootCD = 0.3f; // 射击冷却时间(秒)
	public float BulletSpeed = 400f;   // 子弹速度
	public int ATK = 1; //攻击力
	public float BloodTime = 0f;// 受伤无敌时间
	public float BloodDuration = 0f;
	public int currentHealth;
	public int energy=0;
	protected double shootTimer = 0.0;

	public int BoomNum = 0; // 炸弹数量
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

	public virtual void TakeDamage(int amount, Vector2 hitFrom = default)
	{
		if (BloodTime > 0) return; // 无敌时间
		currentHealth -= amount;
		GD.Print(Name + " took damage: " + amount + " from " + hitFrom);
		BloodTime = BloodDuration;
		if (currentHealth <= 0)
		{
			Die();
		}
	}



	// 死亡

	protected virtual void Die()
	{
		QueueFree();

	}


	/// 射击

	public virtual void Shoot(Vector2 direction, Faction faction)
	{
		if (shootTimer > 0) return; // 冷却中

		if (BulletScene != null)
		{
			var bullet = BulletScene.Instantiate<Bullet>();
			GetTree().CurrentScene.AddChild(bullet);
			bullet.ATK = ATK;
			bullet.GlobalPosition = GetNode<Marker2D>("GunPoint").GlobalPosition;
			bullet.Direction = direction.Normalized();
			bullet.Speed = BulletSpeed;
			bullet.ShooterFaction = faction;
		}

		shootTimer = ShootCD;
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
