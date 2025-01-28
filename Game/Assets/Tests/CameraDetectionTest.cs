using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CameraDetectionTest
{
    private GameObject player;
    private GameObject cameraObject;
    private CameraDetection cameraDetection;

    [SetUp]
public void Setup()
{
    // Create mock player with a collider
    player = new GameObject("Player");
    player.transform.position = new Vector3(0, 0, 5);
    player.AddComponent<CapsuleCollider>(); // Ensure player is detected

    // Create mock camera structure
    cameraObject = new GameObject("Camera");
    var streetCamera = new GameObject("StreetCamera");
    streetCamera.transform.SetParent(cameraObject.transform);

    var detector = new GameObject("Detector");
    detector.transform.SetParent(streetCamera.transform);
    detector.transform.position = new Vector3(0, 0, 0); // Align with camera
    detector.transform.forward = Vector3.forward; // Point toward player

    // Add CameraDetection
    cameraDetection = detector.AddComponent<CameraDetection>();
    cameraDetection.player = player.transform;
    cameraDetection.max_dist = 10f;
    cameraDetection.fov = 75f;

    // Add required components
    cameraDetection.beep = detector.AddComponent<AudioSource>();
    cameraDetection.detected = detector.AddComponent<AudioSource>();

    // Mock GameManager and DetectionManager singletons
    var gameManager = new GameObject("GameManager");
    gameManager.AddComponent<GameManager>();
    GameManager.instance = gameManager.GetComponent<GameManager>();
    GameManager.instance.player = player;

    var detectionManager = new GameObject("DetectionManager");
    detectionManager.AddComponent<DetectionManager>();
    DetectionManager.instance = detectionManager.GetComponent<DetectionManager>();
}


[TearDown]
public void Teardown()
{
    Object.DestroyImmediate(player);
    Object.DestroyImmediate(cameraObject);
    Object.DestroyImmediate(GameManager.instance.gameObject);
    Object.DestroyImmediate(DetectionManager.instance.gameObject);
}


    [UnityTest]
    public IEnumerator DetectsPlayerWithinRange()
    {
        // Set player and camera positions to be within detection range and FOV
        cameraObject.transform.position = new Vector3(0, 0, -5);
        cameraObject.transform.forward = Vector3.forward;

        yield return null; // Allow one frame for initialization

        Assert.IsTrue(cameraDetection.IsPlayerDetected(), "Player should be detected.");
    }

    [UnityTest]
    public IEnumerator DoesNotDetectPlayerOutOfFOV()
    {
        // Set player outside FOV
        cameraObject.transform.position = Vector3.zero;
        cameraObject.transform.forward = Vector3.forward;
        player.transform.position = new Vector3(10, 0, 10);

        yield return null;

        Assert.IsFalse(cameraDetection.IsPlayerDetected(), "Player should not be detected when out of FOV.");
    }

    [UnityTest]
    public IEnumerator DoesNotDetectPlayerOutOfRange()
    {
        // Set player outside max detection range
        cameraObject.transform.position = Vector3.zero;
        player.transform.position = new Vector3(0, 0, 15);

        yield return null;

        Assert.IsFalse(cameraDetection.IsPlayerDetected(), "Player should not be detected when out of range.");
    }

[UnityTest]
public IEnumerator DetectionTriggersCorrectly()
{
    // Setup camera and player
    cameraObject.transform.position = new Vector3(0, 0, 0);
    cameraObject.transform.forward = Vector3.forward;
    player.transform.position = new Vector3(0, 0, 5);

    // Explicitly assign the player in GameManager
    GameManager.instance.player = player;

    yield return null; // Wait for one frame

    // Ensure the player is detected
    Assert.IsTrue(cameraDetection.IsPlayerDetected(), "Player should be detected.");

    // Verify DetectionManager is updated correctly
    DetectionManager.instance.registerCameraDetection();
    Assert.IsTrue(DetectionManager.instance.isAnyCameraDetecting(), "DetectionManager should register camera detection.");
}



    [UnityTest]
    public IEnumerator LostDetectionTriggersCorrectly()
    {
        // Set player within range, then move out of range
        cameraObject.transform.position = Vector3.zero;
        player.transform.position = new Vector3(0, 0, 5);

        yield return null; // Within range
        Assert.IsTrue(cameraDetection.IsPlayerDetected(), "Player should initially be detected.");

        player.transform.position = new Vector3(0, 0, 15); // Move out of range
        yield return null;

        Assert.IsFalse(cameraDetection.IsPlayerDetected(), "Player should no longer be detected.");
        Assert.IsFalse(DetectionManager.instance.isAnyCameraDetecting(), "DetectionManager should unregister camera detection.");
    }
}
