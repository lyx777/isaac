using Godot;
using System;

public partial class Distantadmiration : Items
{
	[Export] public PackedScene FlyOrbitScene; // 预制的苍蝇环绕物

	public override void _Ready()
	{
		base._Ready();
	}
	public override void _Process(double delta)
	{
		if (GetNode<TextureRect>("TextureRect").Visible)
		{
			if (Input.IsActionJustPressed("get_items"))
			{
				Player player = GetTree().Root.GetNode<Player>("Main/Player");
				GiveToPlayer(player);
				QueueFree(); 
			}
		}
	}
	protected override void OnBodyEntered(Node body)
	{
		base.OnBodyEntered(body); // 保留父类逻辑（显示提示）

	}

	private void GiveToPlayer(Player player)
	{
		// 生成环绕苍蝇
		var fly = FlyOrbitScene.Instantiate<FlyOrbit>();
		player.AddChild(fly); // 让苍蝇跟随玩家
		GD.Print("玩家获得仰慕之交，被动苍蝇生成！");
	}
}
