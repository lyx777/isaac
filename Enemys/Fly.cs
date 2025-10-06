using Godot;
using System;

public partial class Fly : Enemy
{
	public override void _Ready()
	{
		Speed = 80f;
		MaxHealth = 3;
		var anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		anim.Play("default");

		base._Ready();
	}
	
}
