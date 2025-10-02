using Godot;
using System;

public partial class CombatActor : CharacterBody2D
{
	public int MaxHealth = 10;         // 最大生命值
	[Export] public PackedScene BulletScene;    
	public float ShootCD = 0.3f; // 射击冷却时间(秒)
	public float BulletSpeed = 400f;   // 子弹速度

	protected int currentHealth;
	protected double shootTimer = 0.0;

	public override void _Ready()
	{
		currentHealth = MaxHealth;
	}

	public override void _Process(double delta)
	{
		shootTimer -= delta;
	}


	// 扣血

	public virtual void TakeDamage(int amount)
	{
		currentHealth -= amount;
		GD.Print(Name + " took damage: " + amount);

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

			bullet.GlobalPosition = GetNode<Marker2D>("GunPoint").GlobalPosition;
			bullet.Direction = direction.Normalized();
			bullet.Speed = BulletSpeed;
			bullet.ShooterFaction = faction;
		}

		shootTimer = ShootCD;
	}
}
