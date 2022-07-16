using UnityEngine;

public class AutonomousMovementController2D : MonoBehaviour
{
    public static Vector2 lastWantedDirection = Vector2.zero;
    public float movementSpeed = 1f;
    
    // IsometricCharacterRenderer isoRenderer;

    Rigidbody2D rbody;

    Vector2 LastDirection;
    Transform OwnTransform;
    Vector3 MousePosition;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        // isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
        OwnTransform = transform;
        BoidsManager.Instance.Boids.Add(gameObject);

        // isoRenderer.AverageMaxSpeed = BoidsManager.Instance.Data.MaxVelocity;
    }

    bool bIsFleeing = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        //m_ownedActor.AddMovementInput(MathUtils.ForwardVector.Rotate(m_ownedActor.Rotation), 1);

        Vector2 groupingAcc, separationAcc, cohesionAcc;
        BoidsManager.Instance.BoidsGSC(gameObject, out groupingAcc, out separationAcc, out cohesionAcc);
        Vector2 fleeingAcc = BoidsManager.Instance.AvoidPredator(gameObject);
        //Vector2 borderBouncingAcc = BoidsManager.BorderBouncing(gameObject);

        // POLISH: if near to predator, break the grouping beahviour 
        // and increase the border bouncing in order to better avoid walls in panic.
        if (fleeingAcc != Vector2.zero)
        {
            groupingAcc *= -1.0f;
            bIsFleeing = true;
            //borderBouncingAcc *= 5.0f;
            // AudioManager.Instance.PlayHumanYellingRandomAudio(transform.position);
        }
        else
        {
            bIsFleeing = false;
        }

        float randV = Mathf.Cos(Time.timeSinceLevelLoad);
        Vector2 forceSum = new Vector2(Random.Range(-randV, randV), Random.Range(-randV, randV)) * Time.deltaTime + groupingAcc + separationAcc + cohesionAcc + fleeingAcc;
        rbody.AddForce(forceSum * BoidsManager.Instance.Data.BoidSpeed, ForceMode2D.Force);
        // isoRenderer.SetDirection(rbody.velocity, rbody.velocity);
        rbody.velocity = Vector2.ClampMagnitude(rbody.velocity, BoidsManager.Instance.Data.MaxVelocity);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("GoodPlace"))
        {
            //float radius = collision.gameObject.GetComponent<CircleCollider2D>().radius;
            //Vector3 pos = collision.ClosestPoint(transform.position);
            Vector3 pos = collision.transform.position;
            Vector3 finalPos = pos - transform.position;
            finalPos.z = 0;
            finalPos = finalPos * BoidsManager.Instance.Data.GoodPlaceAttractionFactor;
            finalPos = Vector2.ClampMagnitude(finalPos, BoidsManager.Instance.Data.MaxVelocity);
            rbody.AddForce(finalPos, ForceMode2D.Force);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("InvisibleWall"))
        {
            Vector3 pos = collision.ClosestPoint(transform.position);
            //Vector3 pos = collision.transform.position;
            Vector3 finalPos = transform.position - pos;
            finalPos.z = 0;
            finalPos = finalPos * BoidsManager.Instance.Data.BorderBouncingForce * (bIsFleeing ? 2f : 1f);
            //finalPos = Vector2.ClampMagnitude(finalPos, BoidsManager.Instance.Data.MaxVelocity);
            rbody.AddForce(finalPos, ForceMode2D.Force);
        }
    }

    //private void OnGUI()
    //{
    //    GUILayout.BeginVertical(GUI.skin.box);
    //    UIDebugDrawLabelValue("Current Vel", rbody.velocity.ToString());
    //    UIDebugDrawLabelValue("Mov Speed", movementSpeed.ToString());
    //    UIDebugDrawLabelValue("Max Delta Vel", (movementSpeed * Time.fixedDeltaTime).ToString());
    //    GUILayout.EndVertical();
    //}

    //private void UIDebugDrawLabelValue(string label, string value)
    //{
    //    GUILayout.BeginHorizontal();
    //    {
    //        GUILayout.Label(label + ":");
    //        GUILayout.Label(value);
    //    }
    //    GUILayout.EndHorizontal();
    //}
}
