using Godot;
using System;

public partial class BookofsinPickUp : Items
{
	[Export] public PackedScene[] DropPool;
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
				var book = new BookOfSin(DropPool);
				player.PickupActiveItem(book);
				QueueFree();
			}
		}
	}
}
