using UnityEngine;

public class Shovel : Tool
{
    [Header("Dig Settings")]
    public GameObject digIndicator;
    public GameObject invalidIndicator;
    public GameObject plantSitePrefab;
    public float maxDigDistance = 5f;
    public LayerMask groundLayerMask = 1;

    private bool isDigging = false;
    private bool isDigSiteValid = false;
    private Vector3 currentDigPosition;
    private PlayerTools playerTools;

    void Awake()
    {
        playerTools = GetComponentInParent<PlayerTools>();
    }

    public override void SetActive(bool active) {

    }

    public override void UpdateTool()
    {
        if(Input.GetMouseButtonDown(0)){
            BeginDig();
        }
        if(Input.GetMouseButtonUp(0)){
            EndDig();
        }

        if (isDigging)
        {
            UpdateDigIndicator();
        }
    }

    private void BeginDig()
    {
        isDigging = true;
    }

    private void EndDig()
    {
        if (isDigging)
        {
            isDigging = false;

            // If the dig site is valid, spawn the plant site
            if (isDigSiteValid)
            {
                Instantiate(plantSitePrefab, currentDigPosition, Quaternion.identity);
            }

            // Hide both indicators
            digIndicator.SetActive(false);
            invalidIndicator.SetActive(false);
        }
    }

    private void UpdateDigIndicator()
    {

        // Create ray from camera through mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check for ground collision
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
        {
            currentDigPosition = hit.point;
            float distance = Vector3.Distance(transform.position, currentDigPosition);

            // Determine if dig site is valid (single source of truth)
            isDigSiteValid = (distance <= maxDigDistance);

            // Show appropriate indicator
            if (isDigSiteValid)
            {
                digIndicator.SetActive(true);
                digIndicator.transform.position = currentDigPosition;
                invalidIndicator.SetActive(false);
            }
            else
            {
                invalidIndicator.SetActive(true);
                invalidIndicator.transform.position = currentDigPosition;
                digIndicator.SetActive(false);
            }
        }
        else
        {
            // No ground hit, hide both indicators
            isDigSiteValid = false;
            digIndicator.SetActive(false);
            invalidIndicator.SetActive(false);
        }
    }
}
