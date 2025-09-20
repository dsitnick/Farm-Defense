using UnityEngine;
using UnityEngine.Events;

public class Explosion : MonoBehaviour {

    public float radius = 5;

    public UnityEvent onExplode;

    public void Explode() {
        onExplode.Invoke();
    }



}
