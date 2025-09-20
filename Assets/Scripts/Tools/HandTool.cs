using UnityEngine;

public class HandTool : Tool
{
    [Header("Hand Tool Settings")]
    public float interactionRadius = 2f;
    public float maxRange = 5f;
    public LayerMask plantSiteLayer = 1;

    private PlayerTools playerTools;
    private RaycastHit[] hits = new RaycastHit[10];

    void Awake()
    {
        playerTools = GetComponentInParent<PlayerTools>();
    }

    public override void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public override void UpdateTool()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HarvestPlant();
        }
    }

    private void HarvestPlant()
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
            if (plantSite != null)
            {
                float distance = hits[i].distance;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlantSite = plantSite;
                }
            }
        }

        // Harvest from closest site
        if (closestPlantSite != null)
        {
            closestPlantSite.Harvest();
        }
    }
}
