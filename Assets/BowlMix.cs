using UnityEngine;

public class BowlMix : MonoBehaviour
{
    public Transform dropPointBowl;        
    public GameObject eggFondPrefab;       

    void Update()       /*fonction propre à la classe MonoBehavior qui permet d'être appelée à chaque frame*/
    {
        if (Input.GetKeyDown(KeyCode.M) && JoueurTientCuillere())                       /*si j’appuie sur M et que je tiens une cuillère*/
        {
            Collider[] objets = Physics.OverlapSphere(dropPointBowl.position, 0.2f);

            foreach (var obj in objets)
            {
                if (obj.CompareTag("EggHand"))
                {
                    Destroy(obj.gameObject);

                    Vector3 pos = dropPointBowl.position + new Vector3(0f, -0.33f, 0f);
                    Instantiate(eggFondPrefab, pos, Quaternion.identity);

                    Debug.Log("EggFond créé");
                    break;
                }
            }
        }
    }

    bool JoueurTientCuillere()
    {
        Transform main = Camera.main.transform.Find("main");                        /*je récupère l’objet "main" (main du joueur devant la caméra)*/
        if (main != null && main.childCount > 0)                                    /*je vérifie si quelque chose est tenu*/
        {
            Transform objet = main.GetChild(0);                                     /*je check l'enfant de main (si c'est bien la cuillère ou pas)*/
            return objet.CompareTag("Spoon") || objet.name.Contains("Spoon");       /*je vérifie si c’est une cuillère par tag ou nom (ça nous évite d'être trop restreint), return true*/
        }
        return false;                                                               /*sinon la fonction retourne false*/
    }
}
