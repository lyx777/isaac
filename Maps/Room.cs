using Godot;
using System;

public partial class Room : TileMapLayer
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
				if(door.state==2)
					door.state=1;
			}
		}

		GD.Print("门已关闭");
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
					door.state=2; 
			}
		}

		GD.Print("门已打开");
	}

	// 可以在敌人全部死亡后调用 UnlockDoors
	public void CheckEnemies()
	{
		var enemies = GetNode("Enemys").GetChildren();
		if (enemies.Count == 0)
		{

			UnlockDoors();
			ClearRoom = true;
		}
	}
}
