using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] private int targetSceneIndex;
    [SerializeField] private Timer timer;
    [SerializeField] private GameObject victoryParticleSystem;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Player")) {
            if(timer != null) {
                timer.StopTimer();
            }
            collision.gameObject.GetComponent<PlayerController>().StopMovement();
            StartCoroutine(LoadScene());
        }
    }

    private IEnumerator LoadScene() {
        GameObject victoryAnimation = Instantiate(victoryParticleSystem, transform);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(targetSceneIndex);
    }
}
