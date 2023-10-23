using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleBehaviour : MonoBehaviour
{
    private new Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!renderer.isVisible)
        {
            gameObject.SetActive(false);
        }
        else if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }
}
