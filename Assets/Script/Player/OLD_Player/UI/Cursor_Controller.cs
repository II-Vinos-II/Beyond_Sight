using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor_Controller : MonoBehaviour
{
    PlayerController player;
    //float speed = 1000f;
    public Vector2 cursorTransform;
    RectTransform cursorRectTransform;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        cursorRectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 movementR = new Vector3(Input.GetAxis("AimV"), Input.GetAxis("AimH"));
        Vector3 movementL = new Vector3(-Input.GetAxis("AimV"), Input.GetAxis("AimH"));

        if (player.transform.rotation == Quaternion.Euler(0, 90, 0))
            cursorRectTransform.SetLocalPositionAndRotation(player.transform.position + movementR * 500, Quaternion.identity);   
        if (player.transform.rotation == Quaternion.Euler(0, -90, 0))
            cursorRectTransform.SetLocalPositionAndRotation(player.transform.position + movementL * 500, Quaternion.identity);*/
    }
}
