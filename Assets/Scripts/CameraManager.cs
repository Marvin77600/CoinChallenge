using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    [SerializeField] CinemachineFreeLook cinemachine;

    public IEnumerator SwitchToCamera(Player _player, Camera _newCamera, VisualEffect _visualEffect, GameObject _key)
    {
        _player.BlockMovement = true;
        _player.Camera.gameObject.SetActive(false);
        _newCamera.gameObject.SetActive(true);
        _visualEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        _key.SetActive(true);
        _visualEffect.Stop();
        yield return new WaitForSeconds(5);
        _player.Camera.gameObject.SetActive(true);
        _newCamera.gameObject.SetActive(false);
        _player.BlockMovement = false;
    }

    public void BlockCamera(bool _flag)
    {
        cinemachine.gameObject.SetActive(!_flag);
    }
}