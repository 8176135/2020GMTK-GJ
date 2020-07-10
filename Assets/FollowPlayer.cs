using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        var pos = _player.transform.position;
        var transform1 = transform;
        transform1.position = new Vector3(pos.x, pos.y, transform1.position.z);
    }
}
