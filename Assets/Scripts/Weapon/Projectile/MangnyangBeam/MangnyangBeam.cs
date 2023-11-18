using UnityEngine;

public class MangnyangBeam : BaseWeapon
{
    [SerializeField] private GameObject MNBCenter;
    [SerializeField] private GameObject MNBScream;
    [SerializeField] private GameObject MNBLight;
    [SerializeField] private GameObject MNBBeam;
    private GameObject centerInstance;
    private GameObject screamInstance;
    private WeaponState weaponState;
    private enum WeaponState
    {
        Ready,
        Splash,
        Beam
    }

    public new void Init()
    {
        base.Init();
        weaponState = WeaponState.Ready;
        // 1. 자식 다 없애기
        for(int i = 0; i < transform.childCount; i++) Destroy(transform.GetChild(i).gameObject);
        centerInstance = null;
        screamInstance = null;
        // 2. 힘을 모으는 액션
        centerInstance = Instantiate(MNBCenter, transform, false);
        centerInstance.transform.localScale = Vector2.zero;
    }

    private void Update()
    {
        transform.position = weaponUser.transform.position;
        switch(weaponState)
        {
            case WeaponState.Ready:
                if(Mathf.Approximately(centerInstance.transform.localScale.y, 1.2f))
                {
                    ChangeState(WeaponState.Splash);
                    return;
                }
                centerInstance.transform.localScale += Vector3.one * 0.05f;
                break;
            case WeaponState.Splash:
                if(screamInstance.GetComponent<SpriteRenderer>().color.a <= 0)
                    ChangeState(WeaponState.Beam);
                break;
            case WeaponState.Beam:
                break;
        }
    }

    private void OnDisable()
    {
        Game.cameraAgent.Reset();
    }

    private void ChangeState(WeaponState state)
    {
        switch(state)
        {
            case WeaponState.Splash:
                screamInstance = Instantiate(MNBScream, transform, false);
                screamInstance.transform.localScale = Vector3.zero;
                for(int i = 0; i < 4; i++)
                {
                    GameObject mnbLight = Instantiate(MNBLight, transform, false);
                    mnbLight.transform.localScale = new Vector3(1, 0, 1);
                    mnbLight.transform.localPosition = transform.up;
                    mnbLight.transform.localRotation = Quaternion.Euler(0, 0, 360 / 4 * i + Random.Range(-1.2f, 1.2f));
                    mnbLight.GetComponent<MNBLight>().Init();
                }
                break;
            case WeaponState.Beam:
                GameObject mnbBeam = Instantiate(MNBBeam, transform, false);
                mnbBeam.transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
                MNBBeam script = mnbBeam.GetComponent<MNBBeam>();
                script.stats = stats;
                script.weaponUser = weaponUser;
                script.Init();
                break;
        }
        weaponState = state;
    }
}
