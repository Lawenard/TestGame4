using UnityEngine;
using System.Collections;

public class CharBehaviour : MonoBehaviour
{
    private float sinPosition;
    private bool stunned;
    private Vector3 direction;

    private Animator animator;

    [SerializeField]
    private float xCoef, zCoef, sinMax, moveSpeed, stunTime, runSpeed;
    [SerializeField]
    private Vector3 movementBounds;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sinPosition = 0;
        stunned = false;

        if (GameManager.Instance.useSettings)
        {
            stunTime = GameManager.Instance.settings.stunTime;
            moveSpeed = GameManager.Instance.settings.charMoveSpeed;
            runSpeed = GameManager.Instance.settings.charRunSpeed;
        }
        animator.SetFloat(nameof(runSpeed), runSpeed);
    }

    private void Update()
    {
        if (!stunned)
            CharMovement();
    }

    private void CharMovement()
    {
        sinPosition += moveSpeed * Time.deltaTime;
        if (sinPosition > sinMax)
            sinPosition -= sinMax;

        Vector3 newPosition = Vector3.zero;
        newPosition.x = Mathf.Sin(sinPosition * xCoef) * movementBounds.x;
        newPosition.z = Mathf.Sin(sinPosition * zCoef) * movementBounds.z;

        direction = (newPosition - transform.position).normalized;
        transform.position = newPosition;
        transform.forward += direction;
    }

    public void ProjectileHit()
    {
        StartCoroutine(Stun());
    }

    // stun and recover
    private IEnumerator Stun()
    {
        stunned = true;
        animator.enabled = false;
        yield return new WaitForSeconds(stunTime);
        stunned = false;
        animator.enabled = true;
    }
}
