using Godot;

public interface IUsableItem
{
	string ItemName { get; }
	
	Texture2D Icon { get; }

	string DropScenePath{ get; }
	
	void Use(Player player);
}
