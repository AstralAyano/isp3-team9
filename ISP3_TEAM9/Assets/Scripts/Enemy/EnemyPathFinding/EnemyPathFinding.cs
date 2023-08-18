using UnityEngine;
using Pathfinding;
using System.Runtime.CompilerServices;

public class EnemyPathFinding : MonoBehaviour
{
    [HideInInspector] public Transform target;

    public float nextWaypointDistance = 3f;
    public bool reachedEndOfPath = false;

    private Path path;
    private int currentWaypoint = 0;

    private Seeker seeker;
    private Rigidbody2D rb;

    [SerializeField] private float speed = 100f;
    [HideInInspector] public bool attackToResolve = false;
    [HideInInspector] public string animDir = "Down";

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
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
        rb.velocity = force;

        if (attackToResolve)
        {
            rb.velocity = Vector2.zero;
        }

        float dist = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (dist <= nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= 0.5f) // left
        {
            animDir = "Right";
        }
        else if (rb.velocity.x <= -0.5f) // right
        {
            animDir = "Left";
        }
        else if (rb.velocity.y >= 0.5f) // up
        {
            animDir = "Up";
        }
        else if (rb.velocity.y <= -0.5f) // down
        {
            animDir = "Down";
        }
    }
}
