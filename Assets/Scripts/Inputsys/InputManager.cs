//using UnityEngine;
//using UnityEngine.InputSystem;
//public class InputManager : Singleton<InputManager>
//{
//    #region Events
//    public delegate void StartTouch(Vector2 position, float time);
//    public event StartTouch OnStartTouch;
//    public delegate void EndTouch(Vector2 position, float time);
//    public event EndTouch OnEndTouch;
//    #endregion

//    private TouchControl touchControl;
//    private Camera mainCamera;

//    private void Awake()
//    {
//        touchControl = new TouchControl();
//        mainCamera = Camera.main;
//    }

//    private void OnEnable()
//    {
//        touchControl.Enable();
//    }

//    private void OnDisable()
//    {
//        touchControl.Disable();
//    }

//    void Start()
//    {
//        touchControl.Touch.PrimaryContact.started += ctx => => StartTouchPrimary(ctx);
//        touchControl.Touch.PrimaryContact.canceled += ctx => => StartTouchPrimary(ctx);
//    }

//    private void StartTouchPrimary(InputAction.CallbackContext context)
//    {

//        if (OnStartTouch != null) OnStartTouch(Utils.ScreenToWorld(mainCamera, touchControl.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.startTime);
//    }
//    private void EndTouchPrimary(InputAction.CallbackContext context)
//    {

//        if (OnEndTouch != null) OnEndTouch(Utils.ScreenToWorld(mainCamera, touchControl.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.time);

 
//    }

//    public Vector2 PrimaryPosition()
//    {
//        return Utils.ScreenToWorld(mainCamera, touchControl.Touch.PrimaryPosition.ReadValue<Vector2>());
//    }
//}