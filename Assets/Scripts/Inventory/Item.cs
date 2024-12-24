using System;
using System.Linq;

[Serializable]
public class Item {
    public SerializableGuid Id;
    public ItemDetails Details;
    public int Quantity;

    public Item(ItemDetails details, int quantity) {
        Id = SerializableGuid.NewGuid();
        this.Details = details;
        this.Quantity = quantity;
    }

    public string GetTooltipText() {
        var text = string.Empty;
        text += Details.Name + "\n";
        text += Details.Type + "\n";
        var equippedItems = Player.Singleton.PlayerEquipment.GetEquippedItems();
        text += Details.GetItemStatDetails(equippedItems?.Where(item => item.Details.Type == Details.Type).ToList());
        return text;
    }
}
