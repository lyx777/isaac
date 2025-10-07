using Godot;
using System;

public partial class Key : Area2D
{

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{

		if (body is Player player)
		{

			player=GetTree().Root.GetNode<Player>("Main/Player");
			player.KeyNum += 1;
			QueueFree(); // 拾取后消失
		}
	}
}
