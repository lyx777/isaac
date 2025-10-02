using Godot;
using System;
using System.Numerics;
using System.Security.Cryptography;

public partial class Player : CombatActor
{
	public float moveSpeed = 250f;

	public override void _Ready()
	{
		// 初始化玩家参数
		MaxHealth = 15;
		BulletSpeed = 500f;
		ShootCD = 0.1f;
		BulletScene = GD.Load<PackedScene>("res://Bullets/Bullet.tscn");

		base._Ready();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		Godot.Vector2 inputDir = Godot.Vector2.Zero;

		if (Input.IsActionPressed("right")) inputDir.X += 1;
		if (Input.IsActionPressed("left")) inputDir.X -= 1;
		if (Input.IsActionPressed("down")) inputDir.Y += 1;
		if (Input.IsActionPressed("up")) inputDir.Y -= 1;

		Velocity = inputDir.Normalized() * 200f;
		MoveAndSlide();

		if (Input.IsActionPressed("ar_left")) Shoot(new Godot.Vector2(-1, 0), Faction.Player);
		else if(Input.IsActionPressed("ar_right")) Shoot(new Godot.Vector2(1, 0), Faction.Player);
		else if(Input.IsActionPressed("ar_up")) Shoot(new Godot.Vector2(0, -1), Faction.Player);
		else if(Input.IsActionPressed("ar_down")) Shoot(new Godot.Vector2(0, 1), Faction.Player);
		
	}

	protected override void Die()
	{
		GD.Print("Player Dead!");
		base.Die();
		(GetTree().Root.GetNode<Label>("Main/CanvasLayer/EndGame")).Visible = true;
	}
}
