using UnityEngine;

public class Seeds : Tool
{
    [Header("Seeds Settings")]
    public PlantData plantData;
    public InventoryItem requiredItem;
    public float interactionRadius = 2f;
    public float maxRange = 5f;
    public LayerMask plantSiteLayer = 1;

    private PlayerTools playerTools;
    private RaycastHit[] hits = new RaycastHit[10];

    void Awake()
    {
        playerTools = GetComponentInParent<PlayerTools>();
    }

    public override void SetActive(bool active) {

    }

    public override void UpdateTool()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlantSeed();
        }
    }

    private void PlantSeed()
    {
        var root = playerTools.cameraRoot;

        // Use NonAlloc spherecast to find all hits
        int hitCount = Physics.SphereCastNonAlloc(root.position, interactionRadius, root.forward, hits, maxRange, plantSiteLayer);

        // Find closest PlantSite
        PlantSite closestPlantSite = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < hitCount; i++)
        {
            PlantSite plantSite = hits[i].collider.GetComponentInParent<PlantSite>();
            if (plantSite != null && !plantSite.hasPlant)
            {
                float distance = hits[i].distance;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlantSite = plantSite;
                }
            }
        }

        var inventory = playerTools.GetComponent<PlayerInventory>();

        // Plant seed at closest site
        if (closestPlantSite != null && inventory.HasItem(requiredItem)) {
            closestPlantSite.Plant(plantData);
            inventory.RemoveItem(requiredItem);
        }
    }
}
