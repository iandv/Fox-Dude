using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    AudioSource _au;
    PlayerModel _pm;
    bool _paused, _canShoot, _endGame;
    float _pressDownStartTime = 0f, _axisValue, _zRotation;
    int _bulletsInGame;

    LayerMask _bulletColMask;

    private void Awake()
    {
        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(0, i))
            {
                _bulletColMask |= 1 << i;
            }
        }
    }

    private void Start()
    {
        _au = GetComponent<AudioSource>();
        _pm = GetComponent<PlayerModel>();
        _canShoot = true;
        _bulletsInGame = 0;
        EventManager.Instance.Subscribe("pause", PauseGame);
        EventManager.Instance.Subscribe("can shoot", CanShoot);
        EventManager.Instance.Subscribe("bullet airborne", BulletCount);
    }

    private void Update()
    {
        if (!_endGame)
        {
            if (!_paused && _axisValue != 0)
            {
                RotateGun();
            }

            if (_paused)
                _pm.powerBar.fillAmount = 0;

            if (_pressDownStartTime > 0)
            {
                ShowBarFill();
            }
        }
    }

    //SHOOT

    public void SelectAmmoBasic()
    {
        _pm.selectedAmmo = PlayerModel.AmmoType.basic;
    }
    public void SelectAmmoTriple()
    {
        _pm.selectedAmmo = PlayerModel.AmmoType.triple;
    }
    public void SelectAmmoExplosive()
    {
        _pm.selectedAmmo = PlayerModel.AmmoType.explosive;
    }

    public void PressDownFire()
    {
        if (!_paused && _canShoot && HaveAmmoBool())
        {
            _pressDownStartTime = Time.time;
        }
    }

    public void PressKeyFire()
    {
        if (!_paused && _canShoot && HaveAmmoBool())
        {
            _pressDownStartTime = 0;
            _pm.powerBar.fillAmount = 0;
        }
    }

    void ShowBarFill()
    {
        float pressDownTime = Time.time - _pressDownStartTime;
        float force = CalculatePressDownForce(pressDownTime);
        ShowPower(force);
        DrawProjection(force);
    }

    private float CalculatePressDownForce(float pressTime)
    {
        float pressTimeNormalized = Mathf.Clamp01(pressTime / _pm.maxForcePressTime);
        float force = pressTimeNormalized * _pm.maxForce;
        return force;
    }

    void ShowPower(float force)
    {
        _pm.powerBar.fillAmount = force / _pm.maxForce;
    }

    public void PressUpFire()
    {
        if (!_paused && _canShoot && HaveAmmoBool())
        {
            _pm.lineRenderer.enabled = false;
            switch (_pm.selectedAmmo)
            {
                case PlayerModel.AmmoType.basic:
                    Fire(0);
                    break;

                case PlayerModel.AmmoType.triple:
                    Fire(1);
                    break;

                case PlayerModel.AmmoType.explosive:
                    Fire(2);
                    break;

                default:
                    break;
            }
        }
    }

    void Fire(int num)
    {
        EventManager.Instance.Trigger("fire");
        _canShoot = false;
        _pm.muzzleFlash.Play();
        _au.PlayOneShot(_pm.cannonFireSound);
        float pressDownTime = Time.time - _pressDownStartTime;
        _pm.powerBar.fillAmount = 0;
        _pressDownStartTime = 0;
        GameObject projectileTemp = Instantiate(_pm.ammoPrefabs[num], _pm.muzzle.transform.position, _pm.muzzle.transform.rotation);
        projectileTemp.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * CalculatePressDownForce(pressDownTime), ForceMode.Impulse);
        _pm.ammoStowage[num]--;
    }

    void PauseGame(params object[] parameters)
    {
        _paused =! _paused;
    }

    void CanShoot(params object[] parameters)
    {
        --_bulletsInGame;
        if (_bulletsInGame <= 0)
        {
            EventManager.Instance.Trigger("all clear");
            _canShoot = true;
            _au.PlayOneShot(_pm.reloadSound);
            NoAmmo();
        }
    }

    void BulletCount(params object[] parameters)
    {
        ++_bulletsInGame;
    }

    bool HaveAmmoBool()
    {
        bool result = default;
        switch (_pm.selectedAmmo)
        {
            case PlayerModel.AmmoType.basic:
                if (_pm.ammoStowage[0] > 0)
                {
                    result = true;
                }
                else
                    result = false;
                break;
            case PlayerModel.AmmoType.triple:
                if (_pm.ammoStowage[1] > 0)
                {
                    result = true;
                }
                else
                    result = false;
                break;
            case PlayerModel.AmmoType.explosive:
                if (_pm.ammoStowage[2] > 0)
                {
                    result = true;
                }
                else
                    result = false;
                break;
        }

        return result;
    }

    void NoAmmo()
    {
        int i = _pm.ammoStowage.Length;
        int e = 0;
        foreach(int item in _pm.ammoStowage)
        {
            if (item <= 0)
                e++;
            if (e >= i)
            {
                EventManager.Instance.Trigger("end game");
                _endGame = true;
            }
        }
    }

    //ROTATION

    void RotateGun()
    {
        float z = _axisValue * _pm.rotationSpeed * Time.fixedDeltaTime;
        _zRotation += z;
        _zRotation = Mathf.Clamp(_zRotation, _pm.minRotation, _pm.maxRotation);
        transform.localRotation = Quaternion.Euler(0f, transform.localRotation.eulerAngles.y, _zRotation);
    }

    public void PressDownPositiveAxis()
    {
        _axisValue = 1;
    }
    public void PressDownNegativeAxis()
    {
        _axisValue = -1;
    }

    public void PressUpRotate()
    {
        _axisValue = 0;
    }

    public void PressDownAxisJS(float i)
    {
        _axisValue = i;
    }

    //LINE RENDER

    private void DrawProjection(float force)
    {
        if (_pm.lineRenderer.enabled == false)
            _pm.lineRenderer.enabled = true;

        _pm.lineRenderer.positionCount = Mathf.CeilToInt(_pm.linePoints / _pm.timeBetweenPoints) + 1;
        Vector3 startPosition = _pm.muzzle.position;
        float massOfBullet = 1f;
        Vector3 startVelocity = force * -_pm.muzzle.forward / - massOfBullet;
        int i = 0;
        _pm.lineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < _pm.linePoints; time += _pm.timeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2 * time * time);
            _pm.lineRenderer.SetPosition(i, point);

            Vector3 lastPosition = _pm.lineRenderer.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit, (point - lastPosition).magnitude, _bulletColMask))
            {
                _pm.lineRenderer.SetPosition(i, hit.point);
                _pm.lineRenderer.positionCount = i + 1;
                return;
            }
        }
    }
}
