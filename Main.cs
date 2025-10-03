using Godot;
using System;


public partial class Main : Node2D
{
	private Random random = new Random();

	private Player player;

	// 生成子弹的预制体
	[Export] public PackedScene BulletScene;
	[Export] public PackedScene EnemyScene;
	public override void _Ready()
	{
		player=GetTree().Root.GetNode<Player>("Main/Player");
		
		GD.Print("Main Loaded");
	}


	public override void _Process(double delta)
	{

	}


	/// <summary>
	/// 统一生成子弹
	/// </summary>
	public Bullet SpawnBullet(Vector2 position, Vector2 direction, Faction faction)
	{
		if (BulletScene == null) return null;

		var bullet = BulletScene.Instantiate<Bullet>();
		AddChild(bullet);
		bullet.GlobalPosition = position;
		bullet.Direction = direction.Normalized();
		bullet.ShooterFaction = faction;
		return bullet;
	}

	/// <summary>
	/// 重新开始关卡
	/// </summary>
	public void RestartGame()
	{
		GetTree().ReloadCurrentScene();
	}


}
