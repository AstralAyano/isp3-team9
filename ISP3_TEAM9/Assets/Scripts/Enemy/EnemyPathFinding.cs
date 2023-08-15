using UnityEngine;
using Pathfinding;

public class EnemyPathFinding : MonoBehaviour
{
    [HideInInspector] public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public bool reachedEndOfPath = false;

    private Path path;
    private int currentWaypoint = 0;
    private float initialScale = 0f;
    private float invertScale = 0f;

    private Seeker seeker;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale.x;
        invertScale = -transform.localScale.x;

        InvokeRepeating("UpdatePath", 0f, 1.0f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            //AstarPathEditor.MenuScan();
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = dir * speed * Time.deltaTime;
        rb.AddForce(force);

        float dist = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (dist <= nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= 0.001f)
        {
            transform.localScale = new Vector3(initialScale, transform.localScale.y, transform.localScale.z);
        }
        if (rb.velocity.x <= -0.001f)
        {
            transform.localScale = new Vector3(invertScale, transform.localScale.y, transform.localScale.z);
        }
    }
}
