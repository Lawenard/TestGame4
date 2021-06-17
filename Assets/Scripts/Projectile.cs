using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    private bool active, charHit;
    private Slingshot slingshot;
    private Rigidbody body;
    private ParticleSystem.MainModule effectMain;

    [SerializeField]
    private MeshRenderer ballMesh;
    [SerializeField]
    private Vector3 launchOffsetVector;
    [SerializeField]
    private float power;
    [SerializeField]
    private Transform respawnTransform;
    [SerializeField]
    private ParticleSystem effect;
    [SerializeField]
    private HitCount hitCount;

    [SerializeField]
    private Color[] ballColors;

    [SerializeField]
    private LayerMask charLayer;
    private int charLayerId, colorId;

    private void Awake()
    {
        charLayerId = (int)Mathf.Log(charLayer.value, 2);

        effectMain = effect.main;
        effect.transform.parent = null;
        effect.gameObject.SetActive(false);
        body = GetComponent<Rigidbody>();
        ballMesh = GetComponent<MeshRenderer>();

        if (GameManager.UseSettings)
        {
            power = GameManager.Settings.shotPower;
            ballColors = GameManager.Settings.ballColors;
        }

        colorId = -1;
        SetRandomColor();
    }

    private void SetRandomColor()
    {
        int _colorId;
        if (ballColors.Length > 1)
        {
            do _colorId = Random.Range(0, ballColors.Length);
            while (_colorId == colorId);
        }
        else
        {
            _colorId = 0;
        }

        colorId = _colorId;
        ballMesh.material.color = ballColors[colorId];
    }

    public void SetSlingshot(Slingshot slingshot)
    {
        if (!this.slingshot)
            this.slingshot = slingshot;
    }

    // read slingshot state and direction to launch properly on player prompt
    private void FixedUpdate()
    {
        if (!active && slingshot.State == Slingshot.SlingshotState.empty)
        {
            effect.gameObject.SetActive(false);
            body.isKinematic = false;
            Vector3 v = launchOffsetVector + slingshot.LaunchDirection;
            body.AddForce(v * power, ForceMode.Impulse);
            active = true;
        }
    }

    // ball respawn
    private void ProjectileReset()
    {
        transform.position = respawnTransform.position;
        active = charHit = false;
        body.isKinematic = true;
        body.velocity = Vector3.zero;

        SetRandomColor();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == charLayerId)
        {
            // showing hit effect with a color of the ball
            effect.transform.position = transform.position;
            effect.gameObject.SetActive(true);
            effectMain.startColor = ballColors[colorId];

            // counting collision once per shot
            if (!charHit)
            {
                charHit = true;
                hitCount.Count();
            }

            // searching character component through the armature and telling that he got hit by a ball
            Transform obj = collision.transform;
            CharBehaviour charBehaviour;
            while (obj.parent != null)
            {
                charBehaviour = obj.GetComponentInParent<CharBehaviour>();
                if (charBehaviour)
                {
                    charBehaviour.ProjectileHit();
                    break;
                }
                else
                {
                    obj = obj.parent;
                }
            }
        }
        else
        {
            StartCoroutine(SlingshotReset());
        }
    }

    // Made this so effect wont translate to the respawn position
    private IEnumerator SlingshotReset()
    {
        yield return new WaitForEndOfFrame();
        slingshot.SlingshotReset();
        ProjectileReset();
    }
}
