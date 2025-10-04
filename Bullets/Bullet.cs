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
	public int ATK=1;
	public Faction ShooterFaction;  // 发射方阵营
	private const int WALL_LAYER_BIT=1<<1;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	public override void _Process(double delta)
	{
		Position += Direction * Speed * (float)delta;
	}

	private void OnBodyEntered(Node body)
	{
		GD.Print("子弹碰撞检测");
		GD.Print("碰到对象：", body.GetType().Name, " | Layer=", (body as Node2D)?.Get("collision_layer"));

		if (ShooterFaction == Faction.Player && body is Enemy enemy)
		{
			enemy.TakeDamage(ATK);
			QueueFree();
		}
		else if (ShooterFaction == Faction.Enemy && body is Player player)
		{
			player.TakeDamage(ATK);
			QueueFree();
		}
		else if (body is PhysicsBody2D pb && (pb.CollisionLayer & WALL_LAYER_BIT) != 0)
		{
			GD.Print(pb.CollisionLayer);
			GD.Print("子弹撞墙 -> 销毁");
			QueueFree();
		}
	}

}
