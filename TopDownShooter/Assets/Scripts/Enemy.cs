﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private Player player;

    [SerializeField]
    private GameObject bloodEffect;
    [SerializeField]
    private GameObject[] blood;

    private HealthSystem healthSystem;
    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 movement;

    private float experience = 20f;
    private float reward = 30f;

    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private Image healthbar;

    public float Experience { get => experience;}
    public float Reward { get => reward;}

    public event EventHandler KillEnemy;

    public void TakeDamage(float damage)
    {
        this.healthSystem.Damage(damage);
    }
    void Start()
    {
        player = Player.instance;
        healthSystem = new HealthSystem(500);
        healthSystem.Death += HealthSystem_Death;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        healthbar.fillAmount = healthSystem.GetHealth() / healthSystem.GetMaxHealth();
        anim.Play("get_damage", 0);
    }

    private void HealthSystem_Death(object sender, EventArgs e)
    {
        var effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
        var temp = UnityEngine.Random.Range(0, blood.Length);
        var _blood = Instantiate(blood[temp], transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(effect, 2f);
        player.AddMoney(UnityEngine.Random.Range(reward - 10, reward + 10));
        KillEnemy?.Invoke(this, EventArgs.Empty);
    }

    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;

        direction.Normalize();
        movement = direction;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var target = collision.gameObject.GetComponent<HealthScript>();
            collision.gameObject.GetComponent<Animator>().Play("GetDamage", 0);
            if (player.OnMainMenu)
                return;
            target.healthSystem.Damage(UnityEngine.Random.Range(5, 10));
            Debug.Log("Health " + target.healthSystem.GetHealth());
            anim.Play("EnemyAttack", 0);
        }
    }
    void FixedUpdate()
    {
        Move(movement);
    }
    void Move(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }
}
