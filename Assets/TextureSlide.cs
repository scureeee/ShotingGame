using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureSlide : MonoBehaviour
{
    /// <summary>
    /// ”wŒi‚ªc‚ÉƒXƒNƒ[ƒ‹‚·‚é‘¬“x
    /// </summary>
    private float speed = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, Time.deltaTime * speed);

        if(transform.position.y <= -11)
        {
            transform.position = new Vector3(0, 21.1f);
        }
    }
}
