using Godot;
using System;

public partial class Hp : Area2D
{
	[Export] public int HealAmount = 2;   // 回复量

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		GD.Print("红心被触碰");
		if (body is Player player)
		{
			// 增加玩家血量
			player=GetTree().Root.GetNode<Player>("Main/Player");
			player.AddHP(HealAmount);
			GD.Print("玩家拾取红心，当前血量：" + player.currentHealth);

			QueueFree(); // 拾取后消失
		}
	}
}
