using Godot;
using System;

public partial class Door : StaticBody2D
{

	[Export]public Sprite2D[] ClosedAndLockedTexture;
	[Export]public Sprite2D[] ClosedTexture;

	private Sprite2D sprite;
	private CollisionShape2D collider;

	private BasicRoom basicroom;
	private Player player;
	[Export] public int state = 0;//0 locked 1 closed 2 open

	public override void _Ready()
	{
		basicroom = GetParent().GetParent<BasicRoom>();
		player = GetTree().Root.GetNode<Player>("Main/Player");
		collider = GetNode<CollisionShape2D>("CollisionShape2D");
		UpdateDoor();
	}

	private void HideAll()
	{

		foreach (var pic in ClosedAndLockedTexture)
		{
			if(pic==null)GD.Print("null pic in ClosedAndLockedTexture");
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
			collider.SetDeferred("disabled", false);
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
			collider.SetDeferred("disabled", false);
		}
		else if (state == 2)
		{
			collider.SetDeferred("disabled", true);
		}

	}
}
