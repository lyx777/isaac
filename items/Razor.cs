using Godot;
using System;

public class Razor : IUsableItem
{
    public string ItemName => "剃刀片";
    public Texture2D Icon => GD.Load<Texture2D>("res://pic/items/Razor.png");
    public string DropScenePath => "res://items/RazorPickup.tscn";
    public void Use(Player player)
    {
        // 玩家扣血
        player.TakeDamage(2);
        player.ATKBuffed = true;
        // 提升当前房间内的攻击力
        player.ATK *=2; // 临时加成
        player.ActiveItemInHand = null; // Clear the active item
        var ItemIcon = player.GetTree().Root.GetNode<TextureRect>("Main/UI/ItemIcon");
        ItemIcon.Texture = null; // Clear the icon
        ItemIcon.Visible = false;
        GD.Print($"{ItemName} 使用：玩家受伤 -1 心，当前房间伤害 +1 当前伤害 {player.ATK}");
    }
}
