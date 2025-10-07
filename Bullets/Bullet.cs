using Godot;
using System;
public enum Faction
{
	Player,
	Enemy
}
public partial class Bullet : Area2D
{
	public float Speed = 300f;
	public Vector2 Direction = Vector2.Zero;

	public bool HasHit = false;
	public float LifeTime = 1.5f; // 子弹存在时间
	public int ATK = 1;
	public Faction ShooterFaction;  // 发射方阵营
	private const int WALL_LAYER_BIT=1<<1;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	public override void _Process(double delta)
	{
		LifeTime -= (float)delta;
		if (LifeTime <= 0f)
		{
			QueueFree();
			return;
		}
		Position += Direction * Speed * (float)delta;
	}

	private void OnBodyEntered(Node body)
	{
		// GD.Print("子弹碰撞检测");
		// GD.Print($"Type: {body.GetClass()}");

		if (ShooterFaction == Faction.Player && body is Enemy enemy)
		{
			QueueFree();
			if (!HasHit)
			{
				enemy.TakeDamage(ATK, GlobalPosition);
				HasHit = true;
			}
		}
		else if (ShooterFaction == Faction.Enemy && body is Player player)
		{
			QueueFree();
			if (!HasHit)
			{
				player.TakeDamage(ATK);
				HasHit = true;
			}
		}
		// 碰撞墙壁
		else if (body is TileMapLayer || body is Stone)
		{
			QueueFree();
		}

	}

}
