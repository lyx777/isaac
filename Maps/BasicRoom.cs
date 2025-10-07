using Godot;
using System;

public partial class BasicRoom : Node2D
{
	public bool isLocked = false;

	public bool ClearRoom = false;
	public override void _Ready()
	{
		var area = GetNode<Area2D>("EntryArea");
		area.BodyEntered += OnPlayerEntered;
	}

	public override void _Process(double delta)
	{
		CheckEnemies();
	}

	private void OnPlayerEntered(Node body)
	{
		if (body is Player && !isLocked && !ClearRoom)
		{
			LockDoors();
			foreach (Node child in GetNode("Enemys").GetChildren())
			{
				if (child is Enemy enemy)
				{
					//输出是哪个房间敌人激活了

					GD.Print($"房间{GetName()}敌人激活");
					enemy.IsActive = true;
				}
			}
		}
		
	}

	private void LockDoors()
	{
		isLocked = true;

		// 找到所有门并关闭
		foreach (Node child in GetNode("Doors").GetChildren())
		{
			if (child is Door door)
			{
				if (door.state == 2)
				{
					door.state = 1;
					door.UpdateDoor();
				}
			}
			GD.Print("门已关闭");
		}

		
	}

	public void UnlockDoors()
	{
		if(ClearRoom) return;
		isLocked = false;

		foreach (Node child in GetNode("Doors").GetChildren())
		{
			if (child is Door door)
			{
				if(door.state==1)
				{
					door.state = 2;
					door.UpdateDoor();
				}
			}
		}

		GD.Print("门已打开");
	}

	// 可以在敌人全部死亡后调用 UnlockDoors
	public void CheckEnemies()
	{
		var enemies = GetNode("Enemys").GetChildren();
		if (enemies.Count == 0&&isLocked)
		{

			UnlockDoors();
			Player player = GetTree().Root.GetNode<Player>("Main/Player");
			if (player.ATKBuffed)
				player.ResetRoomBuffs();
			ClearRoom = true;
		}
	}
}
