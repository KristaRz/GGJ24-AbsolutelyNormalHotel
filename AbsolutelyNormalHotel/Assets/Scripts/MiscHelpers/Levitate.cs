// Created by Krista Plagemann, 21.12.2022 //
// Moves an object up and down via sinus over time //

using System;
using System.Collections;
using UnityEngine;

public class Levitate : MonoBehaviour
{

    [SerializeField] private float startDelay = 0.3f;
    [SerializeField] private float distance = 1;
    [SerializeField] private float speed = 1;
    private float yPos;

    private bool startLevitate = false;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(startDelay);

        yPos = transform.position.y;
        startLevitate =true;
    }

    void Update()
    {
        if (startLevitate)
        {
            float value = Mathf.Sin(Time.time * speed) * distance + yPos;
            transform.position = new Vector3(transform.position.x, value, transform.position.z);
        }
    }

}
