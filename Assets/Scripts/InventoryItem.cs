using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Farm Defense/Inventory Item")]
public class InventoryItem : ScriptableObject {
    [Header("Item Settings")]
    public Sprite sprite;
    public int sellPrice;
    public GameObject toolPrefab;
}
