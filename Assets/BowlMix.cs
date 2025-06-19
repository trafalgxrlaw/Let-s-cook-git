using UnityEngine;

public class BowlMix : MonoBehaviour
{
    public Transform dropPointBowl;        
    public GameObject eggFondPrefab;       

    void Update()       /*fonction propre � la classe MonoBehavior qui permet d'�tre appel�e � chaque frame*/
    {
        if (Input.GetKeyDown(KeyCode.M) && JoueurTientCuillere())                       /*si j�appuie sur M et que je tiens une cuill�re*/
        {
            Collider[] objets = Physics.OverlapSphere(dropPointBowl.position, 0.2f);

            foreach (var obj in objets)
            {
                if (obj.CompareTag("EggHand"))
                {
                    Destroy(obj.gameObject);

                    Vector3 pos = dropPointBowl.position + new Vector3(0f, -0.33f, 0f);
                    Instantiate(eggFondPrefab, pos, Quaternion.identity);

                    Debug.Log("EggFond cr��");
                    break;
                }
            }
        }
    }

    bool JoueurTientCuillere()
    {
        Transform main = Camera.main.transform.Find("main");                        /*je r�cup�re l�objet "main" (main du joueur devant la cam�ra)*/
        if (main != null && main.childCount > 0)                                    /*je v�rifie si quelque chose est tenu*/
        {
            Transform objet = main.GetChild(0);                                     /*je check l'enfant de main (si c'est bien la cuill�re ou pas)*/
            return objet.CompareTag("Spoon") || objet.name.Contains("Spoon");       /*je v�rifie si c�est une cuill�re par tag ou nom (�a nous �vite d'�tre trop restreint), return true*/
        }
        return false;                                                               /*sinon la fonction retourne false*/
    }
}
