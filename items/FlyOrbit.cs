using Godot;
using System;

public partial class FlyOrbit : Area2D
{
	[Export] public float OrbitRadius = 60f;
	[Export] public float AngularSpeed = 2f;
	[Export] public int ContactDamage = 1;

	private float angle = 0f;
	private Player player;

	public override void _Ready()
	{
		player = GetTree().Root.GetNode<Player>("Main/Player");
		if (player == null) GD.Print("FlyOrbit: Player node not found!");
		BodyEntered += OnBodyEntered;
	}

	public override void _Process(double delta)
	{
		if (player == null) return;

		angle += AngularSpeed * (float)delta;
		GlobalPosition = player.GlobalPosition + new Vector2(
			Mathf.Cos(angle), Mathf.Sin(angle)) * OrbitRadius;
	}

	private void OnBodyEntered(Node body)
	{
		GD.Print("FlyOrbit 碰撞检测");
		if (body is Enemy enemy)
		{
			enemy.TakeDamage(ContactDamage);
		}
	}
}
