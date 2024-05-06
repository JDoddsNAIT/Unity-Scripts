using UnityEngine;

[RequireComponent(typeof(FollowTransform))]
public class Missile : MonoBehaviour
{
    public float moveSpeed = 1;

    private FollowTransform m_FollowTransform;
    // Start is called before the first frame update
    void Start()
    {
        m_FollowTransform = GetComponent<FollowTransform>();
        m_FollowTransform.targets.Add(GameObject.FindGameObjectWithTag("Player").transform);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * transform.forward);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}
