using Godot;
using System;

public partial class Enemy : CombatActor
{
	private Player player;

	public bool IsActive = false;
	public bool IsControlledExternally = false; // 外部动作控制（如小跳、冲刺）

	public float Speed = 120f;
	public override void _Ready()
	{
		player = GetTree().Root.GetNode<Player>("Main/Player");

		BulletScene = GD.Load<PackedScene>("res://Bullets/Bullet.tscn");

		base._Ready();
	}
	public override void _Process(double delta)
	{
		if(!IsActive) return;
		//GD.Print("Enemy HP:" + currentHealth);
		base._Process(delta);
		if (IsControlledExternally) return;
		if (Speed <= 0) return;
		Vector2 dir = (player.GlobalPosition - GlobalPosition).Normalized();
		Velocity = dir * Speed;
		MoveAndSlide();
		
	}
	public Vector2 GetDirectionToPlayer()
	{
		Vector2 toPlayer = (player.GlobalPosition - GlobalPosition).Normalized();
		return toPlayer;
	}
	protected override void Die()
	{
		GD.Print("Enemy down!");
		base.Die();
	}
}
