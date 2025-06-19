using UnityEngine;

public class RamasseCuillere : MonoBehaviour
{
    public float distanceMax = 3f;
    public Transform main;              
    private GameObject cuillereTenue;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (cuillereTenue == null)  /*si j'ai pas de cuill�re en main*/
            {
                Ray ray = new Ray(transform.position, transform.forward);               /*je lance un rayon devant moi*/
                if (Physics.Raycast(ray, out RaycastHit hit, distanceMax))              /*si le rayon hit en dessous de la distanceMax (3m)*/
                {
                    GameObject cible = hit.collider.gameObject;             
                    if (cible.CompareTag("Spoon"))                                      /*et que c'est une cuill�re*/
                    {
                        Debug.Log("cuill�re ramass�e");
                        cuillereTenue = cible;                                          /*�a sert de flag, cuillereTenue n'est plus = � null, elle contient l'objet (spoon) de la sc�ne*/
                        cuillereTenue.transform.SetParent(main);                        /*l'attache � la main*/
                        cuillereTenue.transform.localPosition = Vector3.zero;           
                        cuillereTenue.transform.localRotation = Quaternion.identity;    /*je r�initialise sa rotation*/

                        Rigidbody rb = cuillereTenue.GetComponent<Rigidbody>();         
                        if (rb != null) rb.isKinematic = true;                          /*je d�sactive la physique (pas de gravit� etc..)*/

                        Collider col = cuillereTenue.GetComponent<Collider>();          
                        if (col != null) col.enabled = false;                           /*et ses collisions pour qu�elle suive la main*/
                    }
                }
            }
            else /*j'ai d�j� cuillereTenue=true*/
            
            {
                cuillereTenue.transform.SetParent(null);                                /*j'inverse les param�tres au moment de la pose*/
                Rigidbody rb = cuillereTenue.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = false;

                Collider col = cuillereTenue.GetComponent<Collider>();
                if (col != null) col.enabled = true;

                cuillereTenue = null;

                Debug.Log("cuill�re pos�e");
            }
        }
    }
}
