using UnityEngine;

public class BoidsBehaviour : MonoBehaviour
{
    public GameObject boid;
    public int boidAmount = 10;


    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;
    public float radius = 5f;
    public float maxSpeed = 10f;
    public float acceleration = 0.1f;
    [SerializeField] private Rigidbody2D[] rbs;


    private void Start()
    {
        for (int i = 0; i < boidAmount; i++)
        {
            GameObject obj = Instantiate(boid, this.transform);
            obj.transform.localPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
            obj.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        }
        rbs = gameObject.GetComponentsInChildren<Rigidbody2D>();
    }

   
    private void FixedUpdate()
    {
        for (int i = 0; i < rbs.Length; i++)
        {
            GameObject currentBoid = rbs[i].gameObject;

            Vector2 averageDir = rbs[i].transform.up;
            Vector2 averagePos = rbs[i].transform.position;
            int closeCount = 0;

            for (int j = 0; j < rbs.Length; j++)
            {
                GameObject otherBoid = rbs[j].gameObject;
                if (i == j) continue;

                float distance = Vector2.Distance(currentBoid.gameObject.transform.position, otherBoid.gameObject.transform.position);
                if (radius >= distance)
                {
                    closeCount += 1;
                    averageDir += (Vector2)otherBoid.transform.up;
                    averagePos += (Vector2)rbs[i].position;

                }
            }
            Vector2 centroid = averagePos / (closeCount + 1);
            Vector2 offsetToCentroid = centroid - rbs[i].position;
            Vector2 cohesionForce = offsetToCentroid.normalized * acceleration * Time.fixedDeltaTime;
            
            averageDir = averageDir / (closeCount + 1);
            Vector2 currentAccel = averageDir * acceleration * Time.fixedDeltaTime;
            Vector2 vel = rbs[i].linearVelocity + currentAccel * alignmentWeight + cohesionForce * cohesionWeight;


            rbs[i].linearVelocity = Vector3.ClampMagnitude(vel, maxSpeed);
            rbs[i].transform.up = rbs[i].linearVelocity;
        }
    }
}
