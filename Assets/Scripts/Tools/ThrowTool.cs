using System.Collections;
using UnityEngine;

public class ThrowTool : Tool {

    public GameObject prefab;

    public float delay = 0.2f, endlag = 0.8f;
    private PlayerTools playerTools;
    private bool isThrowing;

    void Awake() {
        playerTools = GetComponentInParent<PlayerTools>();
    }

    public override void SetActive(bool active) {

    }

    public override void UpdateTool() {

        if (Input.GetMouseButtonDown(0) && !isThrowing) {

            isThrowing = true;
            StartCoroutine(Throw());
        }
    }

    IEnumerator Throw() {
        var root = playerTools.cameraRoot;

        yield return new WaitForSeconds(delay);

        GameObject projectile = Instantiate(prefab);
        projectile.transform.position = root.position;
        projectile.transform.rotation = root.rotation;

        projectile.GetComponent<Projectile>().Fire(root.forward);

        yield return new WaitForSeconds(endlag);
        isThrowing = false;

    }
}
