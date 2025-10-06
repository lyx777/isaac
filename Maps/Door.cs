using Godot;
using System;

public partial class Door : StaticBody2D
{

	[Export]public Sprite2D[] ClosedAndLockedTexture;
	[Export]public Sprite2D[] ClosedTexture;

	private Sprite2D sprite;
	private CollisionShape2D collider;

	private Room room;
	private Player player;
	[Export] public int state = 0;//0 locked 1 closed 2 open

	public override void _Ready()
	{
		room = GetOwner<Room>();
		player = GetTree().Root.GetNode<Player>("Main/Player");
		collider = GetNode<CollisionShape2D>("CollisionShape2D");
		UpdateDoor();
	}

	public override void _Process(double delta)
	{
		if (player == null) return;
		if (room.isLocked==false &&state == 0 && player.KeyNum > 0&& GlobalPosition.DistanceTo(player.GlobalPosition) < 40f) // 有钥匙且靠近门
		{
			state = 2; // 开门
			player.KeyNum--;
			UpdateDoor();
		}
	}
	private void HideAll()
	{

		foreach (var pic in ClosedAndLockedTexture)
		{
			if (pic != null) pic.Visible = false;
		}
		foreach (var pic in ClosedTexture)
		{
			if (pic != null) pic.Visible = false;
		}
	}
	public void UpdateDoor()
	{
		HideAll();

		if (state == 0)
		{
			foreach (var pic in ClosedAndLockedTexture)
			{
				if (pic != null)
				{
					pic.Visible = true;
				}
			}
			collider.Disabled = false;
		}
		else if (state == 1)
		{
			foreach (var pic in ClosedTexture)
			{
				if (pic != null)
				{
					pic.Visible = true;
				}
			}
			collider.Disabled = false;
		}
		else if (state == 2)
		{
			collider.Disabled = true;
		}

	}
}
