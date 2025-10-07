using Godot;
using System;

public class BookOfSin : IUsableItem
{
    public string ItemName => "七原罪之书";

    public Texture2D Icon => GD.Load<Texture2D>("res://pic/items/bookofsin.png");
    public string DropScenePath => "res://items/BookofsinPickUp.tscn";
    private PackedScene[] dropPool;

    public BookOfSin(PackedScene[] pool)
    {
        dropPool = pool;
    }

    public void Use(Player player)
    {
        if (dropPool == null || dropPool.Length == 0) return;

        int index = (int)(GD.Randi() % dropPool.Length);
        var drop = dropPool[index].Instantiate<Node2D>();

        drop.GlobalPosition = player.GlobalPosition + new Vector2(0, 32);
        player.GetTree().CurrentScene.AddChild(drop);
        var ItemIcon = player.GetTree().Root.GetNode<TextureRect>("Main/UI/ItemIcon");
        ItemIcon.Texture = null; // Clear the icon
        ItemIcon.Visible = false;
        player.ActiveItemInHand = null; // Clear the active item
        GD.Print($"{ItemName} 使用，生成了 {drop.Name}");
    }
}
