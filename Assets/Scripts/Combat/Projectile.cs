using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour {

    public float speed;
    public float gravity;
    public float explosionDuration = 1;

    public UnityEvent onCollide;

    public Rigidbody rb;

    public void Fire(Vector3 direction) {
        rb.isKinematic = false;
        rb.velocity = direction * speed;
    }

    void FixedUpdate() {
        rb.velocity -= Vector3.up * gravity * Time.fixedDeltaTime;
    }

    void OnCollisionEnter(Collision collision) {
        rb.isKinematic = true;
        onCollide.Invoke();
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay() {
        yield return new WaitForSeconds(explosionDuration);
        Destroy(gameObject);
    }

}
