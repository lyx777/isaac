using Godot;
using System;

public partial class RoomManager : Node
{
	public void EnterBossRoom()
	{
		GD.Print("Boss has Bind");
		var boss = GetNode<Monstro>("../Boss/Enemys/Monstro");
		var bossUI = GetNode<BossHP>("../UI/BossHP");
		bossUI.BindBoss(boss);
	}

}
