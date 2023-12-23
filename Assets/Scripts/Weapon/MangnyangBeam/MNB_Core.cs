using UnityEngine;

public class MNB_Core : MonoBehaviour
{
    public GameObject weaponUser {private get; set;}
    [SerializeField] private GameObject Beam;


    public void LaunchBeam()
    {
        Weapon weapon = WeaponBundle.GetWeapon("MangnyangBeam");
        GameObject beam = Instantiate(Beam, transform.parent, false);
        beam.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
        MangnyangBeam script = beam.GetComponent<MangnyangBeam>();
        script.stats = weapon.stats;
        script.weaponUser = weaponUser;
        script.Init();
    }

    public void DestroyCore()
    {
        transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.parent.transform.position = weaponUser.transform.position;
    }
}
