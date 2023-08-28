using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    List<GameObject> players = new List<GameObject>();

    private void Start()
    {
        GetÑamerasTransform();
        StartCoroutine(CheckDistance());
    }

    private void GetÑamerasTransform()
    {
        foreach (GameObject camera in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(camera);
        }
    }

    private IEnumerator CheckDistance()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(3);

            foreach (GameObject t in players)
            {
                if (Mathf.Abs(t.transform.position.x - transform.position.x) > 50)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}