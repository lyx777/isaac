using Godot;

public interface IUsableItem
{
	string ItemName { get; }
	
	Texture2D Icon { get; } // Optional: Add an icon property if needed
	void Use(Player player);
}
