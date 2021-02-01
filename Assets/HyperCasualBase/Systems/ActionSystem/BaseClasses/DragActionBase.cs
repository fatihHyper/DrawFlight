using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.EventSystems;

public abstract class DragActionBase : ActionBase, IDragable
{
    private Collider collider;
    public Collider Collider { get { return (collider == null) ? collider = GetComponent<Collider>() : collider; } }

    float startY;

    protected Vector2 ClampX = new Vector2(-1f, 1f);
    protected Vector2 ClampZ = new Vector2(-2f, 1.8f);

    


    protected virtual void OnMouseDown()
    {
        if (isComplate)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        foreach (Touch touch in Input.touches)
        {
            int id = touch.fingerId;
            if (EventSystem.current.IsPointerOverGameObject(id))
            {
                return;
            }
        }

        Begin();
        ActionManager.Instance.CurrentAction = this;
    }

    protected virtual void OnMouseUp()
    {
        if (isComplate)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        foreach (Touch touch in Input.touches)
        {
            int id = touch.fingerId;
            if (EventSystem.current.IsPointerOverGameObject(id))
            {
                return;
            }
        }


        transform.DOScale(Vector3.one, 0.5f);
        ActionManager.Instance.CurrentAction = null;
        //Collider.enabled = true;
    }

    public override void Begin()
    {
        startY = transform.position.y;
        transform.DOScale(Vector3.one * 1.2f, 0.5f);
        transform.position = Vector3.up;
        //Collider.enabled = false;
    }

    public override void Do()
    {
        if(InputManager.Instance.GetDragPosition() != Vector3.zero)
        {
            
            Vector3 mousePos = new Vector3(InputManager.Instance.GetDragPosition().x, transform.position.y, InputManager.Instance.GetDragPosition().z);
            float clampX = Mathf.Clamp(mousePos.x, ClampX.x, ClampX.y);
            float clampZ = Mathf.Clamp(mousePos.z, ClampZ.x, ClampZ.y);

            transform.position = new Vector3(clampX, transform.position.y, clampZ);
            //transform.position = new Vector3(InputManager.Instance.GetDragPosition().x, transform.position.y, InputManager.Instance.GetDragPosition().z);
        }
    }
}
