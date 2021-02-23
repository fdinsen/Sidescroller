using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] bool movementDirectionIsXAxis = true;
    [SerializeField] float movementSpeed = -1;
    [SerializeField] private GameObject deathParticleSystem;
    [SerializeField] private float playerPush = 100;

    Rigidbody rb;
    SphereCollider sc;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move() {
        if (movementDirectionIsXAxis) {
            rb.velocity = new Vector3(movementSpeed * Time.deltaTime, rb.velocity.y, rb.velocity.z);
        }else {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, movementSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        movementSpeed = movementSpeed * -1;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            sc.enabled = false;
            other.gameObject.GetComponent<PlayerController>().PushUp(playerPush);
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die() {
        GameObject deathAnimation = Instantiate(deathParticleSystem, transform);
        gameObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(1);
        Destroy(deathAnimation);
        Destroy(gameObject);
    }

}
