using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpecialSkills : MonoBehaviour
{
    [SerializeField]
    private GameObject knightBall, mageSpell;
    [SerializeField]
    private int ammo;
    [SerializeField]
    private float mageForce = 500, mageRadius = 10, mageEffectTime = 2;
    [SerializeField]
    private TextMeshProUGUI tmp;

    private bool _activateSkill;
    private SaveData.PlayerSkin ps;
    private PlayerModel pm;

    void Start()
    {
        ps = PlayerSaveProfile.Instance.saveData.playerSkin;
        pm = FindObjectOfType<PlayerModel>();
    }

    void Update()
    {
        if (_activateSkill && ammo > 0)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began && touch.position.y <= (Screen.height * 0.8))
                {
                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, pm.transform.position.z));
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.layer == 0 || 
                            hit.collider.gameObject.layer == 8 || 
                            hit.collider.gameObject.layer == 12)
                        {
                            UseSkill(hit.point);
                            ammo--;
                            if (ammo <= 0)
                                tmp.color = Color.red;
                        }
                    }
                }

            }
        }
    }

    public void SwitchBool()
    {
        StartCoroutine(SwitchCoroutine());
    }

    void UseSkill(Vector3 position)
    {
        switch (ps)
        {
            case SaveData.PlayerSkin.knight:
                ShootCannonBall(position);
                break;
            case SaveData.PlayerSkin.mage:
                MagicSpell(position);
                break;
        }
    }

    void ShootCannonBall(Vector3 targetPosition)
    {
        var bullet = Instantiate(knightBall);
        bullet.transform.position = Camera.main.transform.position;
        var heading = targetPosition - bullet.transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        bullet.GetComponent<KnightsCannonBall>().direction = direction;
    }

    void MagicSpell(Vector3 targetPosition)
    {
        var spell = Instantiate(mageSpell);
        spell.transform.position = targetPosition;
        Destroy(spell, mageEffectTime);
        Collider[] colliders = Physics.OverlapSphere(targetPosition, mageRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(mageForce, targetPosition, mageRadius);
            }

        }
    }

    IEnumerator SwitchCoroutine()
    {
        yield return new WaitForFixedUpdate();
        _activateSkill = !_activateSkill;
        if (_activateSkill && ammo > 0)
            tmp.color = Color.green;
        if (!_activateSkill)
            tmp.color = Color.red;
    }
}
