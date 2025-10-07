using Godot;
using System;

public partial class Enemy : CombatActor
{
	private Player player;

	public bool IsActive = false;
	public bool IsControlledExternally = false; // 外部动作控制（如小跳、冲刺）

	private Random rnd=new Random();
	public float Speed = 120f;
	public override void _Ready()
	{
		player = GetTree().Root.GetNode<Player>("Main/Player");

		BulletScene = GD.Load<PackedScene>("res://Bullets/Bullet.tscn");

		base._Ready();
	}
	public override void _Process(double delta)
	{
		
		if (currentHealth <= 0)
		{
			GD.Print("Enemy Die");
			Die();
			return;
		}
		if (!IsActive) return;
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
		//TODO:生成3种pickups之一
		GD.Print("Enemy Die, Drop Item");
		int st=rnd.Next(0,3);
		switch(st)
		{
			case 0:
				GD.Print("Drop Heart");
				var res=(GD.Load<PackedScene>("res://pickups/Hp.tscn").Instantiate<Area2D>());
				res.GlobalPosition=GlobalPosition;
				GetTree().Root.AddChild(res);
				break;
			case 1:
				GD.Print("Drop Key");
				var res2=(GD.Load<PackedScene>("res://pickups/Key.tscn").Instantiate<Area2D>());
				res2.GlobalPosition=GlobalPosition;
				GetTree().Root.AddChild(res2);
				break;
			case 2:
				GD.Print("Drop Bomb");
				var res3=(GD.Load<PackedScene>("res://pickups/Bomb.tscn").Instantiate<Area2D>());
				res3.GlobalPosition=GlobalPosition;
				GetTree().Root.AddChild(res3);
				break;
		}
		GD.Print("Enemy down!");
		base.Die();
	}
}
