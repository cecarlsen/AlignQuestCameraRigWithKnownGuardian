# Align Quest Camera Rig With Known Guardian

This is a hack for Meta Quest that allows you to align your virtual environment (Unity world space) with a **site specific** physical space. The solution assumes that you know the expected shape of the Guardian at build time, and then it simply attempts to align the expected with the actual Guardian by translating and rotating the Camera Rig.

Updated using Unity 2021.3.

### Beware
At the time of writing, the only way to access the Guardian bounds points is to disable the OpenXR backend and rely on Legacy LibOVR+VRAPI. See [here](https://forum.unity.com/threads/can-we-reuse-user-s-vr-boundaries.818331/#post-8479355). Unfortunately, only OpenXR supports see-through AR on Quest.

### Dependencies
The following need to be downloaded separately.
- [PlayerGizmos](https://github.com/cecarlsen/PlayerGizmos) 1.0.5
- [Oculus Quest Integration](https://assetstore.unity.com/packages/tools/integration/oculus-integration-82022) 43.0

### How to use
1. Add the component *AlignQuestCameraRigWithKnownGuardian* to an object in your scene and assign your Quest CameraRig to the field *CameraRig*.
1. Either measure or 3D-scan your room floor and bring it into your scene. Add transforms where the corners of the Guardian is expected to be. Assign them to the field *ExpectedGuardianTransforms*.
1. Optionally, enable *ShowExpectedGuardian* and *ShowActualGuardian* to see the former in yellow and the latter in green.
1. On your Quest device recreate your Guardian to match the expected shape.
1. Test directly in the Unity Editor.

![Example](https://github.com/cecarlsen/AlignQuestCameraRigWithKnownGuardian/blob/main/ReadmeImages/HotelRoomTest.jpg)
