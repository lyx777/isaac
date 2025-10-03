/*
LeaveTODO

1.Player移动具有惯性

2.Player向各个方向行走动画(美术)

3.眼泪交替从左眼和右眼射出
*/

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
		MaxHealth = 8;
		BulletSpeed = 500f;
		ShootCD = 0.5f;
		BloodDuration = 1f;
		BulletScene = GD.Load<PackedScene>("res://Bullets/Bullet.tscn");
		BoomNum = 30;
		base._Ready();
	}

	public override void _Process(double delta)
	{
		UpdateHearts();
		base._Process(delta);

		Godot.Vector2 inputDir = Godot.Vector2.Zero;

		if (Input.IsActionPressed("right")) inputDir.X += 1;
		if (Input.IsActionPressed("left")) inputDir.X -= 1;
		if (Input.IsActionPressed("down")) inputDir.Y += 1;
		if (Input.IsActionPressed("up")) inputDir.Y -= 1;

		Velocity = inputDir.Normalized() * 200f;
		var count = MoveAndSlide();

		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);
			var collider = collision.GetCollider() as Node;
			if (collider.IsInGroup("BodyEnteredCostHP"))
			{
				TakeDamage(1);
				GD.Print("玩家被敌人碰撞伤害！");
			}
		}

		if (Input.IsActionPressed("ar_left")) Shoot(new Godot.Vector2(-1, 0), Faction.Player);
		else if (Input.IsActionPressed("ar_right")) Shoot(new Godot.Vector2(1, 0), Faction.Player);
		else if (Input.IsActionPressed("ar_up")) Shoot(new Godot.Vector2(0, -1), Faction.Player);
		else if (Input.IsActionPressed("ar_down")) Shoot(new Godot.Vector2(0, 1), Faction.Player);

		if (Input.IsActionJustPressed("use_boom") && BoomNum > 0)
		{
			Explode();
		}

	}

	protected override void Die()
	{
		UpdateHearts();
		GD.Print("Player Dead!");
		base.Die();
		(GetTree().Root.GetNode<Label>("Main/CanvasLayer/EndGame")).Visible = true;
	}

	private void UpdateHearts()
	{
		var hearts =GetTree().Root.GetNode<Control>("Main/UI/Hearts");
		int tmp = currentHealth;
		for (int i = 0; i < hearts.GetChildCount(); i++, tmp -= 2)
		{
			var heart = hearts.GetChild<TextureRect>(i);
			if (tmp >= 2)
			{
				heart.Texture = GD.Load<Texture2D>("res://pic/ui/full_heart.png");
			}
			else if (tmp <= 0)
			{
				heart.Texture = GD.Load<Texture2D>("res://pic/ui/empty_heart.png");
			}
			else if(tmp == 1)
			{
				heart.Texture = GD.Load<Texture2D>("res://pic/ui/half_heart.png");
			}
		}
	}
}
