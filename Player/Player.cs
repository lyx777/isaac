/*
LeaveTODO

1.Player移动具有惯性

2.Player向各个方向行走动画(美术)

3.眼泪交替从左眼和右眼射出
*/

using Godot;
using System;
using System.Numerics;
using System.Security.Cryptography;

public partial class Player : CombatActor
{
	public float moveSpeed = 250f;

	public bool CanCharging = false;
	private bool isCharging = false;
	private float chargeTime = 0f;

	public bool ATKBuffed = false;
	public int KeyNum = 1;

	private Godot.Vector2 lastMoveDir = new Godot.Vector2(0, -1); // 默认向上

	public IUsableItem ActiveItemInHand;

	public override void _Ready()
	{


		// 初始化玩家参数
		MaxHealth = 8;
		BulletSpeed = 500f;
		ShootCD = 0.5f;
		BloodDuration = 1f;

		BulletScene = GD.Load<PackedScene>("res://Bullets/Bullet.tscn");
		BoomNum = 3;
		base._Ready();
	}


	public override void _Process(double delta)
	{
		if (currentHealth <= 0)
		{
			Die();
			return;
		}
		UpdateHearts();
		UpdateKeys();
		UpdateBoom();
		GetShootDirection();
		base._Process(delta);

		Godot.Vector2 inputDir = Godot.Vector2.Zero;

		if (Input.IsActionPressed("right")) inputDir.X += 1;
		if (Input.IsActionPressed("left")) inputDir.X -= 1;
		if (Input.IsActionPressed("down")) inputDir.Y += 1;
		if (Input.IsActionPressed("up")) inputDir.Y -= 1;

		Velocity = inputDir.Normalized() * 200f;
		var count = MoveAndSlide();

		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);
			var collider = collision.GetCollider() as Node;
			if (collider.IsInGroup("BodyEnteredCostHP"))
			{
				TakeDamage(1);
				GD.Print("玩家被敌人碰撞伤害！");
			}
			if (collider is Door door && KeyNum > 0 && door.state == 0)
			{
				KeyNum--;
				door.state = 2;
				door.UpdateDoor();
			}
		}
		if (CanCharging)
		{
			if (Input.IsActionJustPressed("shoot"))
			{
				isCharging = true;
				chargeTime = 0f;
			}

			if (isCharging && Input.IsActionPressed("shoot"))
			{
				chargeTime += (float)delta;
				// 可选：更新蓄力特效
			}

			if (isCharging && Input.IsActionJustReleased("shoot"))
			{
				GD.Print($"蓄力时间: {chargeTime} 秒");
				Shoot(lastMoveDir, Faction.Player, chargeTime);
				isCharging = false;
			}
		}
		else
		{
			if (Input.IsActionJustPressed("shoot"))
			{
				Shoot(lastMoveDir, Faction.Player);
			}

		}
		if (Input.IsActionJustPressed("use_boom") && BoomNum > 0)
		{
			Explode();
		}
		if (Input.IsActionJustPressed("use_item") && ActiveItemInHand != null)
		{
			ActiveItemInHand.Use(this);
		}

		//Debug 专用宝典
		if (Input.IsActionJustPressed("debug_add_boom"))
		{
			BoomNum++;
			GD.Print("Debug: Boom +1");
		}
		if (Input.IsActionJustPressed("debug_add_health"))
		{
			AddHP(2);
			GD.Print("Debug: Health +2");
		}
	}

	public void ResetRoomBuffs()
	{
		ATK = ATK / 2; // 每次进入新房间时清空
		GD.Print("ATK重置");
		ATKBuffed = false;
	}
	public void PickupActiveItem(IUsableItem item)
	{

		ActiveItemInHand = item;
		// 这里可以添加UI更新逻辑，比如显示当前持有的道具图标
		var ItemIcon = GetTree().Root.GetNode<TextureRect>("Main/UI/ItemIcon");
		ItemIcon.Texture = item.Icon; // 假设 IUsableItem 有一个 Icon 属性
		ItemIcon.Visible = true;
		GD.Print($"玩家获得主动道具：{item.ItemName}");
	}
	public void InnerEyeEffect()
	{
		ShotConfig = new MultiShotConfig
				{
					Shots = 3,
					AngleStep = 15f,
					IsCircle = false,
					FireRateMultiplier = 2f
				};
	}
	public void ChocolateMilkEffect()
	{
		CanCharging = true;
	}
	public void GetShootDirection()
	{
		if (Input.IsActionPressed("ar_left")) lastMoveDir = new Godot.Vector2(-1, 0);
		else if (Input.IsActionPressed("ar_right")) lastMoveDir = new Godot.Vector2(1, 0);
		else if (Input.IsActionPressed("ar_up")) lastMoveDir = new Godot.Vector2(0, -1);
		else if (Input.IsActionPressed("ar_down")) lastMoveDir = new Godot.Vector2(0, 1);
	}
	protected override void Die()
	{
		UpdateHearts();
		GD.Print("Player Dead!");
		base.Die();
	}
	public void AddHP(int amount)
	{
		currentHealth = Math.Min(currentHealth + amount, MaxHealth);
		UpdateHearts();
	}
	private void UpdateHearts()
	{
		var hearts = GetTree().Root.GetNode<Control>("Main/UI/Hearts");
		int tmp = currentHealth;
		for (int i = 0; i < hearts.GetChildCount(); i++, tmp -= 2)
		{
			var heart = hearts.GetChild<TextureRect>(i);
			if (tmp >= 2)
			{
				heart.Texture = GD.Load<Texture2D>("res://pic/ui/full_heart.png");
			}
			else if (tmp <= 0)
			{
				heart.Texture = GD.Load<Texture2D>("res://pic/ui/empty_heart.png");
			}
			else if (tmp == 1)
			{
				heart.Texture = GD.Load<Texture2D>("res://pic/ui/half_heart.png");
			}
		}
	}


	private void UpdateBoom()
	{
		var boomLabel = GetTree().Root.GetNode<Label>("Main/UI/BoomNum");
		boomLabel.Text = $"Boom : {BoomNum}";
	}

	private void UpdateKeys()
	{
		var keyLabel = GetTree().Root.GetNode<Label>("Main/UI/KeyNum");
		keyLabel.Text = $"Key : {KeyNum}";
	}
}
