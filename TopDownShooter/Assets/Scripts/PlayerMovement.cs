﻿using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    [SerializeField]
    private float moveSpeed = 8f;

    private Camera cam;
    private Rigidbody2D rb;

    private Vector2 movement;
    private Vector2 mousePos;

    void Start()
    {
        player = Player.instance;
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }
    void FixedUpdate()
    {
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        if (player.onMainMenu)
            return;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }
}