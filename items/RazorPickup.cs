using Godot;
using System;

public partial class RazorPickup : Items
{
	public override void _Ready()
	{
		base._Ready();
	}
	public override void _Process(double delta)
	{
		if(GetNode<TextureRect>("TextureRect").Visible)
		{
			if(Input.IsActionJustPressed("get_items"))
			{
				Player player=GetTree().Root.GetNode<Player>("Main/Player");
				player.PickupActiveItem(new Razor());
				QueueFree();
			}
		}
	}
}
