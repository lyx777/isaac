using Godot;
using System;

public partial class Bomb : Area2D
{
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player player)
		{
			// 增加玩家血量
			player=GetTree().Root.GetNode<Player>("Main/Player");
			player.BoomNum += 1;
			QueueFree(); // 拾取后消失
		}
	}
}
