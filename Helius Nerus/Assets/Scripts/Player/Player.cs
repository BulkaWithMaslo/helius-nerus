﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public static event System.Action PlayerTookDamage = delegate { };

    [SerializeField] private int _maxHealth = 4;
    [SerializeField] private float _invinsibilityTime = 1.0f;
    private int _health = 4;
    private float _invinsibilityLeft = 0.0f;
    private bool _isInvincible = false;

    private Rigidbody2D _rigidbody = null;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        PlayerTookDamage += BecomeInvincible;

        _health = _maxHealth;
    }

    private void Update()
    {
        if (_isInvincible)
        {
            _invinsibilityLeft -= Time.deltaTime;
            if (_invinsibilityLeft <= 0.0f)
            {
                _isInvincible = false;
                _rigidbody.simulated = true;
#if UNITY_EDITOR
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
#endif
            }
        }
    }

    private void BecomeInvincible()
    {
        _invinsibilityLeft = _invinsibilityTime;
        _isInvincible = true;
        _rigidbody.simulated = false; // Disable rigidbody (collider), so player is not able to take damej
                                      // For demonstration purposes
#if UNITY_EDITOR
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
#endif
    }

    private void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            // die script
            Destroy(gameObject);
        }
        else
        {
            PlayerTookDamage.Invoke();
        }
    }

    private void OnDestroy()
    {
        PlayerTookDamage -= BecomeInvincible;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDealDamageToPlayer dealDamageToPlayer = (collision.gameObject.GetComponent(typeof(IDealDamageToPlayer)) as IDealDamageToPlayer);
        if (dealDamageToPlayer != null)
        {
            TakeDamage(dealDamageToPlayer.GetMyDamage());
        }
    }
}