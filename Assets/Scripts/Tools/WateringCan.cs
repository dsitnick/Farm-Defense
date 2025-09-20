using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WateringCan : Tool {
    [Header("Watering Can Settings")]
    public float interactionRadius = 2f;
    public float maxRange = 5f;
    public LayerMask plantSiteLayer = 1;
    public float waterDelay = 0.1f;
    public float waterDuration = 2f;

    [SerializeField]
    public UnityEvent<bool> onSetWaterActive;

    private PlayerTools playerTools;
    private RaycastHit[] hits = new RaycastHit[10];
    private bool isWatering = false, isWaterActive = false;

    void Awake() {
        playerTools = GetComponentInParent<PlayerTools>();
    }

    public override void SetActive(bool active) {

    }

    public override void UpdateTool() {
        if (Input.GetMouseButtonDown(0) && !isWatering) {
            StartCoroutine(WaterPlants());
        }
    }

    IEnumerator WaterPlants() {
        isWatering = true;
        yield return new WaitForSeconds(waterDelay);
        SetWaterActive(true);

        yield return new WaitForSeconds(waterDuration);
        SetWaterActive(false);

        isWatering = false;
    }

    void SetWaterActive(bool isWaterActive) {
        this.isWaterActive = isWaterActive;
        onSetWaterActive.Invoke(isWaterActive);
    }

    void FixedUpdate() {
        if (!isWaterActive) return;

        var root = playerTools.cameraRoot;

        // Use NonAlloc spherecast to find all hits
        int hitCount = Physics.SphereCastNonAlloc(root.position, interactionRadius, root.forward, hits, maxRange, plantSiteLayer);

        // Water all PlantSites found
        for (int i = 0; i < hitCount; i++) {
            PlantSite plantSite = hits[i].collider.GetComponentInParent<PlantSite>();
            if (plantSite != null) {
                plantSite.Water();
            }
        }
    }
}
