using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObject : MonoBehaviour
{
    Transform cam;
    MeshRenderer m_renderer;
    MeshRenderer[] c_renderers;
    Vector3 origin;
    Vector3 offset;
    bool hasShadows = true;

    [Tooltip("”гол сокрыти€."), Range(0, 360)]
    public int hideAngle = 120;
    public float fromAngle = 0f;

    public float height = 30f;
    public float time = 8f;
    void Start()
    {
        cam = Camera.main.transform.parent;
        m_renderer = GetComponent<MeshRenderer>();
        c_renderers = GetComponentsInChildren<MeshRenderer>();
        origin = offset = transform.position;
        offset.y += height;

        Location parentLoc = transform.parent.GetComponent<Location>();

        if(parentLoc != null)
        {
            Transform pivot = parentLoc.GetPivot();

            Vector3 wallAngle = transform.position - pivot.position;
            wallAngle.y = 0;

            fromAngle = (Vector3.SignedAngle(Vector3.back, wallAngle, Vector3.up) - hideAngle / 2 + 1440f) % 360f;
        }
    }

    
    void Update()
    {
        if (BelongsToRange())
        {
            transform.position = Vector3.Lerp(transform.position, offset, time * Time.deltaTime * GameController.instance.mod_wall_time);
            if (hasShadows)
            {
                m_renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                foreach (var cr in c_renderers) { cr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; }
                hasShadows = false;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, origin, time * Time.deltaTime * GameController.instance.mod_wall_time);
            if (!hasShadows)
            {
                m_renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                foreach (var cr in c_renderers) { cr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On; }
                hasShadows = true;
            }
        }
    }

    bool BelongsToRange()
    {
        float rot = cam.rotation.eulerAngles.y;
        float n_angle = rot < 0 ? 360f + (rot % 360f) : rot % 360f;

        float x_angle = fromAngle % 360;
        float y_angle = (x_angle + hideAngle) % 360;

        if (x_angle > y_angle)
        {
            return n_angle >= x_angle || n_angle <= y_angle;
        }
        else
        {
            return n_angle >= x_angle && n_angle <= y_angle;
        }
    }
}
