public class SlotEntry
{
    public InventoryCell Cell { get; private set; }
	public InventoryItem Item { get; set; }
	public int ItemsCount { get; set; }

	public SlotEntry(InventoryCell cell) => Cell = cell;
}
