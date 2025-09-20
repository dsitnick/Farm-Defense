using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlantSite : MonoBehaviour {
    [Header("Plant Site Settings")]
    public float wateringCooldown = 5f;

    [Header("Events")]
    public UnityEvent OnPlant;
    public UnityEvent OnWater;

    public PlantData currentPlant { get; private set; }
    private bool isWatered = false;
    private GameObject currentPlantObject;
    private float lastWateredTime = 0f;
    private bool isFullyGrown = false;
    private Animator plantAnimator;
    private float currentGrowth = 0f;

    public bool hasPlant => currentPlant != null;

    public void Plant(PlantData plantData) {
        if (currentPlant == null && plantData != null) {
            currentPlant = plantData;

            // Instantiate the plant prefab as a child
            if (plantData.prefab != null) {
                currentPlantObject = Instantiate(plantData.prefab, transform.position, transform.rotation, transform);
                plantAnimator = currentPlantObject.GetComponent<Animator>();
                isFullyGrown = false;
                currentGrowth = 0f;
            }

            // Trigger plant event
            OnPlant.Invoke();

            Debug.Log($"Planted {plantData.name} at {transform.position}");
        }
    }

    public void Water() {
        if (currentPlant != null && (Time.time - lastWateredTime >= wateringCooldown)) {
            isWatered = true;
            lastWateredTime = Time.time;

            // Trigger water event
            OnWater.Invoke();

            Debug.Log($"Watered plant at {transform.position}");
        }
    }


    public void Harvest() {
        if (currentPlant != null && isFullyGrown) {
            // TODO: Add harvest logic (spawn items, add to inventory, etc.)
            Debug.Log($"Harvested {currentPlant.name} at {transform.position}");

            // Trigger harvest animation
            if (plantAnimator != null) {
                plantAnimator.SetTrigger("Harvest");
            }

            // Clear plant data immediately
            currentPlant = null;
            plantAnimator = null;
            isWatered = false;
            isFullyGrown = false;
            currentGrowth = 0f;

            // Destroy plant object after 0.5 seconds
            StartCoroutine(DestroyPlantAfterDelay(0.5f));
        }
    }

    private IEnumerator DestroyPlantAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        if (currentPlantObject != null) {
            Destroy(currentPlantObject);
            currentPlantObject = null;
        }
    }

    public void Clear() {
        // Remove the plant model
        if (currentPlantObject != null) {
            Destroy(currentPlantObject);
        }

        // Reset all plant-related variables to original state
        currentPlant = null;
        currentPlantObject = null;
        plantAnimator = null;
        isWatered = false;
        isFullyGrown = false;
        currentGrowth = 0f;
    }

    void Update() {
        if (currentPlant != null && plantAnimator != null && !isFullyGrown) {
            // Only grow when watered
            if (isWatered) {
                float growthSpeed = 1f / currentPlant.growTime;
                currentGrowth += growthSpeed * Time.deltaTime;

                if (currentGrowth >= 1f) {
                    isFullyGrown = true;
                    plantAnimator.SetBool("Fully Grown", true);
                }

                currentGrowth = Mathf.Clamp01(currentGrowth);
            }

            plantAnimator.SetFloat("Growth", currentGrowth);
        }
    }
}
