using UnityEngine;

[CreateAssetMenu(fileName = "New Plant Data", menuName = "Farm Defense/Plant Data")]
public class PlantData : ScriptableObject
{
    [Header("Plant Settings")]
    public GameObject prefab;
    public float growTime = 10f;
    public float waterDuration = 5f;
    
    [Header("Harvest Settings")]
    public InventoryItem fruitItem;
    public int fruitCount = 1;
}
