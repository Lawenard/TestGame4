using UnityEngine;

public class Slingshot : MonoBehaviour
{
    private Camera cam;

    [SerializeField]
    private Transform slingshotViewTransform, arrowTransform;
    [SerializeField]
    private LayerMask launchAreaLayer, projectileLayer;
    [SerializeField]
    private Projectile projectile;

    public SlingshotState State { get; private set; }
    public Vector3 LaunchDirection => slingshotViewTransform.forward;

    public enum SlingshotState { ready, loaded, empty };

    private void Awake()
    {
        cam = Camera.main;
        State = SlingshotState.ready;
        projectile.SetSlingshot(this);
    }

    // reset sligshot state and orientation, feels better that way
    public void SlingshotReset()
    {
        slingshotViewTransform.eulerAngles = Vector3.zero;
        State = SlingshotState.ready;
    }

    // working with player input and slingshot state here
    private void FixedUpdate()
    {
        if (Input.touchCount < 1)
        {
            if (State == SlingshotState.loaded)
            {
                arrowTransform.gameObject.SetActive(false);
                State = SlingshotState.empty;
            }
            return;
        }

        RaycastHit hit;
        switch (State)
        {
            case SlingshotState.ready:
                hit = ReadPlayerTouch(projectileLayer);
                if (hit.collider)
                {
                    arrowTransform.gameObject.SetActive(true);
                    State = SlingshotState.loaded;
                }
                break;
            case SlingshotState.loaded:
                hit = ReadPlayerTouch(launchAreaLayer);
                if (hit.collider)
                {
                    slingshotViewTransform.LookAt(hit.point);
                    slingshotViewTransform.Rotate(Vector3.up, 180);
                    projectile.transform.position = hit.point;
                }
                break;
            default:
                return;
        }
    }

    private RaycastHit ReadPlayerTouch(LayerMask layer)
    {
        var ray = cam.ScreenPointToRay(Input.GetTouch(0).position);
        Physics.Raycast(ray, out RaycastHit hit, 1000, layer);
        return hit;
    }
}
