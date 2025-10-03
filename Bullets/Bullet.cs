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

	public override void _Process(double delta)
	{
		Position += Direction * Speed * (float)delta;
	}

	private void OnBodyEntered(Node body)
	{

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
	}

}
