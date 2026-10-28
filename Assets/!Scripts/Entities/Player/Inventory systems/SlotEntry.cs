public class SlotEntry
{
    public InventoryCell Cell { get; private set; }
	public ItemData Item { get; set; }
	public int ItemsCount { get; set; }

	public SlotEntry(InventoryCell cell) => Cell = cell;
}
