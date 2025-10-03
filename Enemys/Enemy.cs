using Godot;
using System;

public partial class Enemy : CombatActor
{
	private Player player;
	
	public float Speed=120f;
	public override void _Ready()
	{
		player = GetTree().Root.GetNode<Player>("Main/Player");

		MaxHealth = 5;
		BulletSpeed = 500f;
		ShootCD = 2f;

		BulletScene = GD.Load<PackedScene>("res://Bullets/Bullet.tscn");

		base._Ready();
	}
	public override void _Process(double delta)
	{
		//GD.Print("Enemy HP:" + currentHealth);
		base._Process(delta);
		if (Speed <= 0) return;
		Vector2 dir = (player.GlobalPosition - GlobalPosition).Normalized();
		Velocity = dir * Speed;
		MoveAndSlide();
		
	}
	
	protected override void Die()
	{
		GD.Print("Enemy down!");
		base.Die();
	}
}
