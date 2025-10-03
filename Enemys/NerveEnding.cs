using Godot;
using System;

public partial class NerveEnding : Enemy
{
	public Player player;

	public override void _Ready()
	{
		Speed = 0;
		MaxHealth = 30;
		var anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		anim.Play("default");
		base._Ready();
	}
	

}
